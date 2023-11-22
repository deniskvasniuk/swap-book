using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace swap_book.Models
{
	public class DatabaseContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Book> Books { get; set; }
		public DbSet<Exchange> Exchanges { get; set; }

		public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Book>()
				.HasOne(b => b.Owner)
				.WithMany(u => u.Books)
				.HasForeignKey(b => b.OwnerId);
		}
	}
}
