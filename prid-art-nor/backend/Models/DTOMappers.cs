using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prid_2021_A08.Models {
    public static class DTOMappers {
        public static UserDTO ToDTO(this User user) {
            return new UserDTO {
                Id = user.Id,
                Pseudo = user.Pseudo,
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                BirthDate = user.BirthDate,
                Role = user.Role,
                Boards = user.Boards.ToDTO(),
                BoardCollaborations = user.BoardCollaborations.Select(c => c.Board).ToDTO(),
                CardParticipations = user.CardParticipations.Select(p => p.Card).ToDTO()
            };
        }

        public static List<UserDTO> ToDTO(this IEnumerable<User> users) {
            return users.Select(u => u.ToDTO()).ToList();
        }

        public static UserCollabPartDTO ToCollabPartDTO(this User user){
             return new UserCollabPartDTO {
                Id = user.Id,
                Pseudo = user.Pseudo
            };
        }

         public static List<UserCollabPartDTO> ToCollabPartDTO(this IEnumerable<User> users) {
            return users.Select(u => u.ToCollabPartDTO()).ToList();
         }


        public static BoardDTO ToDTO(this Board board){
            return new BoardDTO {
                Id = board.Id,
                Name = board.Name,
                OwnerId = board.OwnerId,
                Collaborations = board.Collaborations.Select(u => u.UserId).ToList(),
                Lists = board.Lists.OrderBy(l => l.Pos).ToDTO()
            };
        }

        public static List<BoardDTO> ToDTO(this IEnumerable<Board> boards) {
            return boards.Select(b => b.ToDTO()).ToList();
        }

        public static CardDTO ToDTO(this Card card){
            return new CardDTO {
                Id = card.Id,
                Name = card.Name,
                AuthorId = card.AuthorId,
                ListId = card.ListId,
                UserParticipations = card.UserParticipations.Select(u => u.UserId).ToList()
            };
        }

        public static List<CardDTO> ToDTO(this IEnumerable<Card> cards) {
            return cards.Select(c => c.ToDTO()).ToList();
        }
        
        public static ListDTO ToDTO(this List list){
            return new ListDTO {
                Id = list.Id,
                Name = list.Name,
                BoardId = list.BoardId,
                Pos = list.Pos,
                Cards = list.Cards.OrderBy(c => c.Pos).ToDTO()
            };
        }

        public static List<ListDTO> ToDTO(this IEnumerable<List> lists) {
            return lists.Select(l => l.ToDTO()).ToList();
        }

        public static CollaborationDTO ToDTO(this Collaboration collaboration){
            return new CollaborationDTO{
                UserId = collaboration.UserId,
                BoardId = collaboration.BoardId
            };
        }

        public static List<CollaborationDTO> ToDTO(this IEnumerable<Collaboration> collaborations) {
            return collaborations.Select(c => c.ToDTO()).ToList();
        }

        public static ParticipationDTO ToDTO(this Participation participation){
            return new ParticipationDTO{
                // User = participation.User,
                UserId = participation.UserId,
                // Card = participation.Card,
                CardId = participation.CardId
            };
        }

        public static List<ParticipationDTO> ToDTO(this IEnumerable<Participation> participation) {
            return participation.Select(c => c.ToDTO()).ToList();
        }
    }
}
