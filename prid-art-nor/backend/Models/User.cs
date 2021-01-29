using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace prid_2021_A08.Models {

    public enum Role {
        Admin = 2, Manager = 1, Member = 0
    }

    public class User : IValidatableObject {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(10, MinimumLength = 3)]
        public string Pseudo { get; set; }

        [Required, StringLength(10, MinimumLength = 3)]
        public string Password { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        public DateTime? BirthDate { get; set; }

        public Role Role { get; set; } = Role.Member;

        public virtual IList<Board> Boards { get; set; } = new List<Board>();

        public virtual IList<Card> Cards { get; set; } = new List<Card>();

        public virtual IList<Collaboration> BoardCollaborations { get; set;} = new List<Collaboration>();

        public virtual IList<Participation> CardParticipations { get; set; } = new List<Participation>();

        [NotMapped]
        public string Token { get; set; }

        public int? Age {
            get {
                if (!BirthDate.HasValue)
                    return null;
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Value.Year;
                if (BirthDate.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
        
        private bool CheckPseudoUnique(TrelloContext context) {
            var users = context.Users.SingleOrDefault(
                u => u.Pseudo == Pseudo && u.Id != Id);
            return users == null;
        }

        private bool CheckEmailUnique(TrelloContext context) {
            var users = context.Users.SingleOrDefault(
                u => u.Email == Email && u.Id != Id);
            return users == null;
        }

        private bool CheckLastName() {
            return String.IsNullOrEmpty(FirstName);
        }

        private bool CheckFirstName() {
            return String.IsNullOrEmpty(LastName);
        }

        private bool CheckPseudoRegex() {
            var regexPseudo = new Regex("^[a-zA-Z0-9_]*$");
            return regexPseudo.IsMatch(Pseudo) && Pseudo != null;
        }

        private bool CheckEmailRegex() {
            var regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regexMail.IsMatch(Email) && Email != null;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var currContext = validationContext.GetService(typeof(TrelloContext)) as TrelloContext;
            Debug.Assert(currContext != null);
            if (!CheckPseudoUnique(currContext))
                yield return new ValidationResult("Pseudo is already taken", new[] { nameof(Pseudo) });
            if (!CheckEmailUnique(currContext))
                yield return new ValidationResult("Email is already taken", new[] { nameof(Email) });
            if (!CheckPseudoRegex())
                yield return new ValidationResult("Pseudo can't have accentuated letters", new[] { nameof(Pseudo) });
            if (!CheckEmailRegex())
                yield return new ValidationResult("Not in a mail format", new[] { nameof(Email) });
            if (CheckLastName())
                yield return new ValidationResult("FirstName can't be empty", new[] { nameof(LastName) });
            if (CheckFirstName())
                yield return new ValidationResult("LastName can't be empty", new[] { nameof(FirstName) });
            if (BirthDate.HasValue && BirthDate.Value.Date > DateTime.Today)
                yield return new ValidationResult("Can't be born in the future in this reality", new[] { nameof(BirthDate) });
            else if (Age.HasValue && Age < 18)
                yield return new ValidationResult("Must be 18 years old", new[] { nameof(BirthDate) });
        }
    }

    public class UserCollabPart {
        public int Id { get; set; }
        public string pseudo {get; set;}
    }

    
}