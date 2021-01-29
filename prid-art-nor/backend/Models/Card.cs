using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace prid_2021_A08.Models {

    public class Card {

        [Key]
        public int Id { get; set; }
        
        [Required, StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        public int AuthorId { get; set; }

        public virtual List List { get; set; }

        public int ListId { get; set; }

        public virtual User Author { get; set; }

        public virtual IList<User> Participaters { get; set; } = new List<User>();

        public virtual IList<Participation> UserParticipations { get; set; } = new List<Participation>();

        public int Pos { get; set; }

        [NotMapped]
        public IEnumerable<int> ParticipatedUsers {
            get => UserParticipations.Select(p => p.UserId);
        }

    }

}