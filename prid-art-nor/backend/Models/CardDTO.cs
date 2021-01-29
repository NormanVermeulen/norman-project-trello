using System;
using System.Collections.Generic;

namespace prid_2021_A08.Models {
    public class CardDTO {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int ListId { get; set; }
        public IEnumerable<int> UserParticipations { get; set; }
        public int Pos { get; set; }

    }
}