using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;




namespace prid_2021_A08.Models {

    public class Board {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual IList<List> Lists { get; set; } = new List<List>();

        public virtual IList<Collaboration> Collaborations { get; set; } = new List<Collaboration>();

        [NotMapped]
        public IEnumerable<int> Collaboraters {
            get => Collaborations.Select(c => c.UserId);
        }


    }

    public class Collaboration {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int BoardId { get; set; }
        public virtual Board Board { get; set; }
    }

    public class Participation {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CardId { get; set; }
        public virtual Card Card { get; set; }
    }






}