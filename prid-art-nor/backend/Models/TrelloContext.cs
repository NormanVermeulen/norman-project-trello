using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;

namespace prid_2021_A08.Models {
    public class TrelloContext : DbContext {
        public TrelloContext(DbContextOptions<TrelloContext> options)
            : base(options) {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Board> Boards { get; set; }

        public DbSet<List> Lists { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Collaboration> Collaborations { get; set; }

        public DbSet<Participation> Participations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);
            structuralConfiguration(modelBuilder);
            addData(modelBuilder);

        }

        private void addData(ModelBuilder modelBuilder) {
            var ben = new User() { Id = 1, Pseudo = "ben", Password = "ben", LastName = "Penelle", FirstName = "Benoît", Email = "benoit@penelle.be" };
            var bruno = new User() { Id = 2, Pseudo = "bruno", Password = "bruno", LastName = "Lacroix", FirstName = "Bruno", Email = "bruno@lacroix.be" };
            var norman = new User() { Id = 3, Pseudo = "norman", Password = "norman", LastName = "Vermeulen", FirstName = "Norman", Email = "norman@vermeulen.be" };
            var jon = new User() { Id = 4, Pseudo = "jon", Password = "jon", LastName = "Deversenne", FirstName = "Jonathan", Email = "jonathan@deversenne.be" };
            var art = new User() { Id = 5, Pseudo = "art", Password = "art", LastName = "Denyse", FirstName = "Arthur", Email = "arthur@denyse.be" };
            var admin = new User() { Id = 6, Pseudo = "admin", Password = "admin", LastName = "Admin", FirstName = "Al", Email = "admin@gros.be", Role = Role.Admin };
            modelBuilder.Entity<User>().HasData(ben, bruno, norman, jon, art, admin);

            var board1 = new Board() { Id = 1, Name = "Board1", OwnerId = 1 };
            var board2 = new Board() { Id = 2, Name = "Board2", OwnerId = 6 };
            var board3 = new Board() { Id = 3, Name = "Board3", OwnerId = 5 };
            modelBuilder.Entity<Board>().HasData(board1, board2, board3);

            modelBuilder.Entity<Collaboration>().HasData(
                new Collaboration { UserId = 4, BoardId = 1 },
                new Collaboration { UserId = 5, BoardId = 1 },
                new Collaboration { UserId = 3, BoardId = 1 },
                new Collaboration { UserId = 2, BoardId = 2 },
                new Collaboration { UserId = 1, BoardId = 2 },
                new Collaboration { UserId = 6, BoardId = 2 }
            );

            var list1 = new List() { Id = 1, Name = "list1", BoardId = 1, Pos = 0 };
            var list2 = new List() { Id = 2, Name = "list2", BoardId = 1, Pos = 1 };
            var list3 = new List() { Id = 3, Name = "list3", BoardId = 2, Pos = 0 };

            modelBuilder.Entity<List>().HasData(list1, list2, list3);

            var card1 = new Card() { Id = 1, Name = "card1", AuthorId = 1, ListId = 2, Pos = 0 };
            var card2 = new Card() { Id = 2, Name = "card2", AuthorId = 1, ListId = 2, Pos = 1 };
            var card3 = new Card() { Id = 3, Name = "card3", AuthorId = 1, ListId = 1, Pos = 0 };
            var card4 = new Card() { Id = 4, Name = "card4", AuthorId = 1, ListId = 1, Pos = 1 };
            modelBuilder.Entity<Card>().HasData(card1, card2, card3, card4);

            modelBuilder.Entity<Participation>().HasData(
                new Participation { UserId = 2, CardId = 1 },
                new Participation { UserId = 3, CardId = 2 },
                new Participation { UserId = 4, CardId = 3 }
            );
        }

        private void addUsers(ModelBuilder modelBuilder) {

            modelBuilder.Entity<User>().HasData(
            new User() { Id = 1, Pseudo = "ben", Password = "ben", LastName = "Penelle", FirstName = "Benoît", Email = "benoit@penelle.be" },
            new User() { Id = 2, Pseudo = "bruno", Password = "bruno", LastName = "Lacroix", FirstName = "Bruno", Email = "bruno@lacroix.be" },
            new User() { Id = 3, Pseudo = "norman", Password = "norman", LastName = "Vermeulen", FirstName = "Norman", Email = "norman@vermeulen.be" },
            new User() { Id = 4, Pseudo = "jon", Password = "jon", LastName = "Deversenne", FirstName = "Jonathan", Email = "jonathan@deversenne.be" },
            new User() { Id = 5, Pseudo = "art", Password = "art", LastName = "Denyse", FirstName = "Arthur", Email = "arthur@denyse.be" },
            new User() { Id = 6, Pseudo = "admin", Password = "admin", LastName = "Admin", FirstName = "Al", Email = "admin@gros.be", Role = Role.Admin }
            );

        }

        private void structuralConfiguration(ModelBuilder modelBuilder) {


            // modelBuilder de USER

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Pseudo)
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(true);

            // modelBuilder de BOARD

            modelBuilder.Entity<Board>()
                .HasIndex(u => u.Name)
                .IsUnique(true);

            modelBuilder.Entity<Board>()
                .HasOne(b =>b.Owner)
                .WithMany(o => o.Boards);

            // modelBuilder de CARD

            modelBuilder.Entity<Card>()
                .HasOne(c => c.Author)
                .WithMany(a => a.Cards);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.List)
                .WithMany(l => l.Cards);

            // modelBuilder de LIST

            modelBuilder.Entity<List>()
                .HasOne(l => l.Board)
                .WithMany(b => b.Lists);
                
            // modelBuilder de COLLABORATION

            modelBuilder.Entity<Collaboration>()
                .HasKey(c => new { c.UserId, c.BoardId });

            modelBuilder.Entity<Collaboration>()
                .HasOne<User>(c => c.User)
                .WithMany(u => u.BoardCollaborations)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Collaboration>()
                .HasOne<Board>(c => c.Board)
                .WithMany(b => b.Collaborations)
                .HasForeignKey(c => c.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder de PARTICIPATION
            
            modelBuilder.Entity<Participation>()
                .HasKey(p => new { p.UserId, p.CardId });

            modelBuilder.Entity<Participation>()
                .HasOne<User>(p => p.User)
                .WithMany(u => u.CardParticipations)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Participation>()
                .HasOne<Card>(p => p.Card)
                .WithMany(c => c.UserParticipations)
                .HasForeignKey(p => p.CardId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}