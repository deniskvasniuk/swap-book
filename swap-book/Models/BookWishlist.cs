using swap_book.Models;

public class BookWishlist
{
	public int BookId { get; set; }

	public int WishlistId { get; set; }

	public Book Book { get; set; }

	public Wishlist Wishlist { get; set; }

	public BookWishlist()
	{
	}

	public BookWishlist(int bookId, int wishlistId)
	{
		BookId = bookId;
		WishlistId = wishlistId;
	}
}