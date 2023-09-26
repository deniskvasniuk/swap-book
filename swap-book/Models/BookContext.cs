using System.Data.Entity;

namespace swap_book.Models
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
    }
}
