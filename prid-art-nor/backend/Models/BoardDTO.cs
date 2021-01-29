using System;
using System.Collections.Generic;

namespace prid_2021_A08.Models {

    public class BoardDTO {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public IEnumerable<int> Collaborations { get; set; }
        public IEnumerable<ListDTO> Lists { get; set; }
    }

    public class CollaborationDTO {

        public int UserId { get; set; }

        public int BoardId { get; set; }
    }

    public class ParticipationDTO {
        
        public int UserId { get; set; }

        public int CardId { get; set; }
    }
}