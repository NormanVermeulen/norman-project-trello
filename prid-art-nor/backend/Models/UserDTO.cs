using System;
using System.Collections.Generic;

namespace prid_2021_A08.Models {
    public class UserDTO {
    public int Id { get; set; }
    public string Pseudo { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Role Role { get; set; }

    public IEnumerable<BoardDTO> Boards { get; set; }

    public IEnumerable<BoardDTO> BoardCollaborations { get; set; }
    public IEnumerable<CardDTO> CardParticipations { get; set;}

    }

    public class UserCollabPartDTO {
        public int Id { get; set; }
        public string Pseudo { get; set; }
    }
}