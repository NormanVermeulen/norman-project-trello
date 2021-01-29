using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prid_2021_A08.Models;
using System.ComponentModel.DataAnnotations;
using PRID_Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Security.Claims;
using prid_2021_A08.Helpers;

namespace prid_2021_A08.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class BoardsController : ControllerBase {
        private readonly TrelloContext _context;
        public BoardsController(TrelloContext context) {
            _context = context;
        }

        // BOARD ---------------------------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardDTO>>> GetBoards() {
            var boards = await _context.Boards.ToListAsync();
            if (boards == null)
                return NotFound();
            return boards.ToDTO();
        }

        [HttpGet("Owner/{userId}")]
        public async Task<ActionResult<IEnumerable<BoardDTO>>> GetBoardsByOwner(int userId) {
            var boards = await _context.Boards.ToListAsync();
            var boardsToReturn = new List<Board>();

            if (boards == null)
                return NotFound();
            else {
                foreach (Board b in boards) {
                    if (b.OwnerId == userId)
                        boardsToReturn.Add(b);
                }
            }
            return boardsToReturn.ToDTO();
        }


        [HttpGet("Collabs/{userId}")]
        public async Task<ActionResult<IEnumerable<BoardDTO>>> GetBoardsByCollab(int userId) {
            var boards = await _context.Boards.ToListAsync();
            var boardsToReturn = new List<Board>();

            if (boards == null)
                return NotFound();
            else {
                foreach (Board b in boards) {
                    if (b.Collaborations != null) {
                        foreach (Collaboration c in b.Collaborations) {
                            if (c.UserId == userId && !boardsToReturn.Contains(b)) {
                                boardsToReturn.Add(b);
                            }
                        }
                    }
                }
            }
            return boardsToReturn.ToDTO();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDTO>> GetOne(int id) {
            var board = await _context.Boards.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (board == null)
                return NotFound();
            return board.ToDTO();
        }

        [HttpGet("collab/{boardId}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetCollaboraters(int boardId) {
            var board = await _context.Boards.Where(b => b.Id == boardId).FirstOrDefaultAsync();
            if (board == null)
                return NotFound();
            var collabs = new List<User>();
            foreach (var c in board.Collaborations) {
                collabs.Add(_context.Users.Find(c.UserId));
            }
            return Ok(collabs.ToCollabPartDTO());
        }

        [HttpPost]
        public async Task<ActionResult<BoardDTO>> PostBoard(BoardDTO data) {
            var board = await _context.Boards.FindAsync(data.Id);
            var user = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();

            if (board != null) {
                var err = new ValidationErrors().Add("Board's name already in use", nameof(board.Name));
                return BadRequest(err);
            }

            var newBoard = new Board() {
                Name = data.Name,
                OwnerId = user.Id

            };

            if (data.Collaborations != null) {
                var collabs = data.Collaborations.Select(co => new Collaboration { UserId = co });
                foreach (var c in collabs) {
                    newBoard.Collaborations.Add(c);
                }
            }

            _context.Boards.Add(newBoard);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);

            return CreatedAtAction(nameof(GetOne), new { id = newBoard.Id }, newBoard.ToDTO());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id) {
            var board = await _context.Boards.Where(b => b.Id == id).FirstOrDefaultAsync();

            if (board == null)
                return NotFound();

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoard(int id, BoardDTO boardDTO) {
            var board = await _context.Boards.Where(b => b.Id == id).FirstOrDefaultAsync();

            if (id != boardDTO.Id)
                return BadRequest();

            board.Name = boardDTO.Name;

            // Partie pour vider les collaborations du board et de la DB
            //des users qui ont été enlevés lors de l'edit

            var collabsInDTO = boardDTO.Collaborations.Select(x => x);
            foreach (var c in board.Collaborations) {
                if (!collabsInDTO.Contains(c.User.Id))
                    _context.Collaborations.Remove(c);
            }

            // Partie pour ajouter les collaborations au board
            //si il ne les contient pas deja alors on les ajoute

            if (boardDTO.Collaborations != null) {
                var collabs = board.Collaborations.Select(c => c.UserId);
                foreach (var uid in boardDTO.Collaborations) {
                    if (!collabs.Contains(uid)) {
                        var newCollab = new Collaboration { UserId = uid };
                        board.Collaborations.Add(newCollab);
                    }
                }
            }

            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);

            return NoContent();
        }

        // LIST--------------------------------------------------------------------------------------

        [HttpPost("{id}")]
        public async Task<ActionResult<ListDTO>> PostList(int id, ListDTO data) {
            var board = await _context.Boards.Where(b => b.Id == id).FirstOrDefaultAsync();
            var list = await _context.Lists.FindAsync(data.Id);

            if (list != null) {
                var err = new ValidationErrors().Add("List already in use", nameof(list.Name));
                return BadRequest(err);
            }
            var newList = new List() {
                Name = data.Name,
                BoardId = id,
                Pos = board.Lists.Count()
            };

            _context.Lists.Add(newList);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);


            return CreatedAtAction(nameof(GetOne), new { id = newList.Id }, newList.ToDTO());
        }

        [HttpPut("updateList")]
        public async Task<IActionResult> PutList(ListDTO listData) {

            var list = await _context.Lists.Where(l => l.Id == listData.Id).FirstOrDefaultAsync();

            if (list == null)
                return NotFound();

            list.Name = listData.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("updatePosList/{list1Id}/{list2Id}")]
        public async Task<IActionResult> PutPosList(ListDTO listDTO, int list1Id, int list2Id) {
            var list1 = await _context.Lists.Where(l => l.Id == list1Id).FirstOrDefaultAsync();
            var list2 = await _context.Lists.Where(l => l.Id == list2Id).FirstOrDefaultAsync();

            if (list1 == null || list2 == null) {
                return NotFound();
            }

            var posTmp = list1.Pos;
            list1.Pos = list2.Pos;
            list2.Pos = posTmp;

            var res = await _context.SaveChangesAsyncWithValidation();

            if (!res.IsEmpty)
                return BadRequest(res);

            return NoContent();
        }

        [HttpDelete("list/{id}")]
        public async Task<IActionResult> DeleteList(int id) {
            var list = await _context.Lists.Where(l => l.Id == id).FirstOrDefaultAsync();
            var board = await _context.Boards.Where(b => b.Lists.Contains<List>(list)).FirstOrDefaultAsync();
            var listPos = list.Pos;

            if (list == null)
                return NotFound();

            _context.Lists.Remove(list);
             board.Lists.Remove(list);

            var lists = board.Lists.OrderBy(l => l.Pos).Select(l => l);
            var listTab = new List<List>();
            foreach(var l in lists){
                listTab.Add(l);
            }
            for(int i = listPos; i < board.Lists.Count(); i++){
                listTab[i].Pos = i;
            }
            board.Lists = listTab;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // CARD --------------------------------------------------

        [HttpPost("{boardId}/{listId}")]
        public async Task<ActionResult<CardDTO>> PostCard(int boardId, int listId, CardDTO data) {

            var board = await _context.Boards.Where(b => b.Id == boardId).SingleOrDefaultAsync();
            var user = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            var list = await _context.Lists.Where(l => l.Id == listId).SingleOrDefaultAsync();

            foreach (List l in board.Lists) {
                foreach (Card c in l.Cards) {
                    if (c.Name == data.Name) {
                        var err = new ValidationErrors().Add("Card's Name already in use on this board", nameof(l.Name));
                        return BadRequest(err);
                    }

                }
            }

            var newCard = new Card() {
                Name = data.Name,
                ListId = listId,
                AuthorId = user.Id,
                Pos = list.Cards.Count()
            };

            _context.Cards.Add(newCard);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);


            return CreatedAtAction(nameof(GetOne), new { id = newCard.Id }, newCard.ToDTO());
        }

        [HttpGet("{boardId}/{cardName}")]
        public async Task<ActionResult<CardDTO>> GetCardByName(int boardId, string cardName) {
            var board = await _context.Boards.Where(b => b.Id == boardId).FirstOrDefaultAsync();

            foreach (List l in board.Lists) {
                foreach (Card c in l.Cards) {
                    if (c.Name == cardName)
                        return c.ToDTO();
                }
            }
            return NotFound();

        }


        [HttpPut("updatePosCard/{cardId}")]
        public async Task<IActionResult> PutPosCard(CardDTO cardDTO, int cardId) {
            var card = await _context.Cards.Where(c => c.Id == cardId).FirstOrDefaultAsync();
            var list = await _context.Lists.Where(l => l.Id == cardDTO.ListId).FirstOrDefaultAsync();

            if (card == null) {
                return NotFound();
            }

            card.ListId = cardDTO.ListId;
            card.Pos = cardDTO.Pos;

            var res = await _context.SaveChangesAsyncWithValidation();

            if (!res.IsEmpty)
                return BadRequest(res);

            return NoContent();
        }

        [HttpPut("updateCard")]
        public async Task<IActionResult> PutCard(CardDTO cardDTO) {
            var card = await _context.Cards.Where(c => c.Id == cardDTO.Id).FirstOrDefaultAsync();
            if (card == null) {
                return NotFound();
            }

            card.Name = cardDTO.Name;

            // Partie pour vider les participations de la carte et de la DB
            //des users qui ont été enlevés lors de l'edit

            var partsInDTO = cardDTO.UserParticipations.Select(x => x);
            foreach (var p in card.UserParticipations) {
                if (!partsInDTO.Contains(p.User.Id))
                    _context.Participations.Remove(p);
            }

            // Partie pour ajouter les participations à la carte
            //si il ne les contient pas deja alors on les ajoute

            if (cardDTO.UserParticipations != null) {
                var parts = card.UserParticipations.Select(c => c.UserId);
                foreach (var uid in cardDTO.UserParticipations) {
                    if (!parts.Contains(uid)) {
                        var newPart = new Participation { UserId = uid };
                        card.UserParticipations.Add(newPart);
                    }
                }
            }

            var res = await _context.SaveChangesAsyncWithValidation();

            if (!res.IsEmpty)
                return BadRequest(res);

            return NoContent();


        }

        [HttpDelete("card/{id}")]
        public async Task<IActionResult> DeleteCard(int id) {
            var card = await _context.Cards.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (card == null)
                return NotFound();

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("part/{cardId}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetParticipaters(int cardId) {
            var card = await _context.Cards.Where(c => c.Id == cardId).FirstOrDefaultAsync();
            if (card == null)
                return NotFound();
            var parts = new List<User>();
            foreach (var c in card.UserParticipations) {
                parts.Add(_context.Users.Find(c.UserId));
            }
            return Ok(parts.ToCollabPartDTO());
        }


    }

}



