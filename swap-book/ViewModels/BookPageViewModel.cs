
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using swap_book.Models;

public class BookDetailsViewModel
{
    public Book Book { get; set; }
    public ApplicationUser Owner { get; set; }
    public List<Book> RelatedBooks { get; set; }
    public BookDetailsViewModel()
    {
        RelatedBooks = new List<Book>();
    }

    public async Task LoadRelatedBooks(Book book, DatabaseContext _context)
    {
       
            // Get all books with shared category based on join
            var relatedBooks = await _context.Books
                .Join(_context.BookCategories, b => b.BookId, bc => bc.BookId, (b, bc) => new { b, bc })
                .Join(_context.BookCategories, bc1 => bc1.bc.CategoryId, bc2 => bc2.CategoryId, (bc1, bc2) => bc1.b)
                .Where(b => b.BookId != book.BookId)
                .OrderByDescending(b => b.CreatedAt)
                .Take(10)
                .ToListAsync();

            // Set the related books property
            RelatedBooks = relatedBooks;
            
    }
}