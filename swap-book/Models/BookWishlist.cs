using swap_book.Models;

public class BookWishlist
{
    public int BookId { get; set; }

    public int UserId { get; set; }

    public Book Book { get; set; }

}