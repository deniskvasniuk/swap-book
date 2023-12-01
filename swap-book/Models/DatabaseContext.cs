using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace swap_book.Models
{
	public class DatabaseContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Book> Books { get; set; }
		public DbSet<Exchange> Exchanges { get; set; }
		public DbSet<Subscriber> Subscribers { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{
		}


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        }


    }
}
