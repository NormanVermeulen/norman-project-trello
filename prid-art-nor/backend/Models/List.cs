using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace prid_2021_A08.Models {

    public class List {

        [Key]
        public int Id { get; set; }
        [Required, StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        public virtual IList<Card> Cards { get; set; } = new List<Card>();

        public int BoardId { get; set; }

        public virtual Board Board { get; set; }

        public int Pos { get; set; }

    }

}