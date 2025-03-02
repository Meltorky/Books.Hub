﻿// <auto-generated />
using System;
using Books.Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Books.Hub.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250218191757_create-domain-entities")]
    partial class createdomainentities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Books.Hub.Domain.Entities.Auther", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("AuthorImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<DateOnly>("DateOfBrith")
                        .HasColumnType("date");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Authers");
                });

            modelBuilder.Entity("Books.Hub.Domain.Entities.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AutherId")
                        .HasColumnType("int");

                    b.Property<byte[]>("BookCover")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("PageCount")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<DateOnly>("PublishedDate")
                        .HasColumnType("date");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<int>("TotalCopiesSold")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AutherId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Books.Hub.Domain.Entities.BookCategory", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("BookId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BookCategories");
                });

            modelBuilder.Entity("Books.Hub.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Crime"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Horror"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Fantasy"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Romance"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Mystery"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Biography"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Cookbooks"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Historical"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Science Fiction"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Health & Fitness"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Self-Improvement"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Business & Finance"
                        });
                });

            modelBuilder.Entity("Books.Hub.Domain.Entities.Book", b =>
                {
                    b.HasOne("Books.Hub.Domain.Entities.Auther", "Auther")
                        .WithMany("Books")
                        .HasForeignKey("AutherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auther");
                });

            modelBuilder.Entity("Books.Hub.Domain.Entities.BookCategory", b =>
                {
                    b.HasOne("Books.Hub.Domain.Entities.Book", "Book")
                        .WithMany("BookCategories")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Books.Hub.Domain.Entities.Category", "Category")
                        .WithMany("BookCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Books.Hub.Domain.Entities.Auther", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Books.Hub.Domain.Entities.Book", b =>
                {
                    b.Navigation("BookCategories");
                });

            modelBuilder.Entity("Books.Hub.Domain.Entities.Category", b =>
                {
                    b.Navigation("BookCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
