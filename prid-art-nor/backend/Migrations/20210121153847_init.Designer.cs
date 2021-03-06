﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using prid_2021_A08.Models;

namespace prid_2021_A08.Migrations
{
    [DbContext(typeof(TrelloContext))]
    [Migration("20210121153847_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("prid_2021_A08.Models.Board", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.ToTable("Boards");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Board1",
                            OwnerId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Board2",
                            OwnerId = 6
                        },
                        new
                        {
                            Id = 3,
                            Name = "Board3",
                            OwnerId = 5
                        });
                });

            modelBuilder.Entity("prid_2021_A08.Models.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("ListId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.Property<int>("Pos")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ListId");

                    b.ToTable("Cards");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            ListId = 2,
                            Name = "card1",
                            Pos = 0
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 1,
                            ListId = 2,
                            Name = "card2",
                            Pos = 1
                        },
                        new
                        {
                            Id = 3,
                            AuthorId = 1,
                            ListId = 1,
                            Name = "card3",
                            Pos = 0
                        },
                        new
                        {
                            Id = 4,
                            AuthorId = 1,
                            ListId = 1,
                            Name = "card4",
                            Pos = 1
                        });
                });

            modelBuilder.Entity("prid_2021_A08.Models.Collaboration", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "BoardId");

                    b.HasIndex("BoardId");

                    b.ToTable("Collaborations");

                    b.HasData(
                        new
                        {
                            UserId = 4,
                            BoardId = 1
                        },
                        new
                        {
                            UserId = 5,
                            BoardId = 1
                        },
                        new
                        {
                            UserId = 3,
                            BoardId = 1
                        },
                        new
                        {
                            UserId = 2,
                            BoardId = 2
                        },
                        new
                        {
                            UserId = 1,
                            BoardId = 2
                        },
                        new
                        {
                            UserId = 6,
                            BoardId = 2
                        });
                });

            modelBuilder.Entity("prid_2021_A08.Models.List", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.Property<int>("Pos")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.ToTable("Lists");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BoardId = 1,
                            Name = "list1",
                            Pos = 0
                        },
                        new
                        {
                            Id = 2,
                            BoardId = 1,
                            Name = "list2",
                            Pos = 1
                        },
                        new
                        {
                            Id = 3,
                            BoardId = 2,
                            Name = "list3",
                            Pos = 0
                        });
                });

            modelBuilder.Entity("prid_2021_A08.Models.Participation", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CardId");

                    b.HasIndex("CardId");

                    b.ToTable("Participations");

                    b.HasData(
                        new
                        {
                            UserId = 2,
                            CardId = 1
                        },
                        new
                        {
                            UserId = 3,
                            CardId = 2
                        },
                        new
                        {
                            UserId = 4,
                            CardId = 3
                        });
                });

            modelBuilder.Entity("prid_2021_A08.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CardId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<string>("Pseudo")
                        .IsRequired()
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Pseudo")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "benoit@penelle.be",
                            FirstName = "Benoît",
                            LastName = "Penelle",
                            Password = "ben",
                            Pseudo = "ben",
                            Role = 0
                        },
                        new
                        {
                            Id = 2,
                            Email = "bruno@lacroix.be",
                            FirstName = "Bruno",
                            LastName = "Lacroix",
                            Password = "bruno",
                            Pseudo = "bruno",
                            Role = 0
                        },
                        new
                        {
                            Id = 3,
                            Email = "norman@vermeulen.be",
                            FirstName = "Norman",
                            LastName = "Vermeulen",
                            Password = "norman",
                            Pseudo = "norman",
                            Role = 0
                        },
                        new
                        {
                            Id = 4,
                            Email = "jonathan@deversenne.be",
                            FirstName = "Jonathan",
                            LastName = "Deversenne",
                            Password = "jon",
                            Pseudo = "jon",
                            Role = 0
                        },
                        new
                        {
                            Id = 5,
                            Email = "arthur@denyse.be",
                            FirstName = "Arthur",
                            LastName = "Denyse",
                            Password = "art",
                            Pseudo = "art",
                            Role = 0
                        },
                        new
                        {
                            Id = 6,
                            Email = "admin@gros.be",
                            FirstName = "Al",
                            LastName = "Admin",
                            Password = "admin",
                            Pseudo = "admin",
                            Role = 2
                        });
                });

            modelBuilder.Entity("prid_2021_A08.Models.Board", b =>
                {
                    b.HasOne("prid_2021_A08.Models.User", "Owner")
                        .WithMany("Boards")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_A08.Models.Card", b =>
                {
                    b.HasOne("prid_2021_A08.Models.User", "Author")
                        .WithMany("Cards")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("prid_2021_A08.Models.List", "List")
                        .WithMany("Cards")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_A08.Models.Collaboration", b =>
                {
                    b.HasOne("prid_2021_A08.Models.Board", "Board")
                        .WithMany("Collaborations")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("prid_2021_A08.Models.User", "User")
                        .WithMany("BoardCollaborations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_A08.Models.List", b =>
                {
                    b.HasOne("prid_2021_A08.Models.Board", "Board")
                        .WithMany("Lists")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_A08.Models.Participation", b =>
                {
                    b.HasOne("prid_2021_A08.Models.Card", "Card")
                        .WithMany("UserParticipations")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("prid_2021_A08.Models.User", "User")
                        .WithMany("CardParticipations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("prid_2021_A08.Models.User", b =>
                {
                    b.HasOne("prid_2021_A08.Models.Card", null)
                        .WithMany("Participaters")
                        .HasForeignKey("CardId");
                });
#pragma warning restore 612, 618
        }
    }
}
