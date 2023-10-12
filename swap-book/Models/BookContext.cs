﻿using Microsoft.EntityFrameworkCore;

namespace swap_book.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        }


    }
}
