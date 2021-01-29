using System;
using System.Collections.Generic;

namespace prid_2021_A08.Models {
    public class ListDTO {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BoardId { get; set; }
        public IEnumerable<CardDTO> Cards { get; set; }
        public int Pos { get; set; }
    }
}