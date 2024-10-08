﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Collections.Generic;

namespace swap_book.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
		public DbSet<Wishlist> Wishlists { get; set; }
		public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<BookWishlist> BookWishlist { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public DatabaseContext() : base()
        {
           
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=ACER;database=swap-book_new;trusted_connection=true; encrypt=false");
            }
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(DefaultCategories.GetDefaultCategories().ToArray());

            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId);

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<Exchange>()
                .HasOne(e => e.Book)
                .WithMany(b => b.Exchanges)
                .HasForeignKey(e => e.BookId);

            modelBuilder.Entity<Exchange>()
                .Ignore(e => e.ExchangedBook); 

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Exchanges)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<BookWishlist>()
                .HasKey(bw => new { bw.BookId, bw.UserId });
        }
    }
}