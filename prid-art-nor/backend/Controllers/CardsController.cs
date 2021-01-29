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

    public class CardsController : ControllerBase {

        private readonly TrelloContext _context;

        public CardsController(TrelloContext context) {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardDTO>> GetOne(int id) {
            var card = await _context.Cards.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (card == null)
                return NotFound();
            return card.ToDTO();
        }

        [HttpPost("{listId}")]
        public async Task<ActionResult<CardDTO>> PostCard(int listId, CardDTO data){
            var card = await _context.Cards.FindAsync(data.Id);
            if (card != null) {
                var err = new ValidationErrors().Add("Card already in use", nameof(card.Name));
                return BadRequest(err);
            }
            var newCard = new Card(){
                Name = data.Name,
                AuthorId = data.AuthorId,
                ListId = listId
            };

            _context.Cards.Add(newCard);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);


            return CreatedAtAction(nameof(GetOne), new { name = newCard.Name }, newCard.ToDTO());
        }
    }
}