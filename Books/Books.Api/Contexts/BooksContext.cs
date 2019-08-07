using Books.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Api.Contexts
{
    public class BooksContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public BooksContext(DbContextOptions<BooksContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author()
                {
                    Id = Guid.Parse("8790941d-a3d9-4a66-acad-d23eb18a564d"),
                    FirstName = "George",
                    LastName = "Martin"
                }
                );
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = Guid.Parse("cc7c8548 - 5c49 - 44dc - b5ac - e61f8bac9c80"),
                    AuthorId = Guid.Parse("8790941d-a3d9-4a66-acad-d23eb18a564d"),
                    Title = "The Winds of Winter",
                    Description = "Book"
                });
        }
    }
}
