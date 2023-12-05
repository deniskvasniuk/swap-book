using swap_book.Models;

namespace swap_book.ViewModels
{
	public class IndexViewModel
	{
		public List<Book> Books { get; set; }

		public List<Book> LatestBooks { get; set; }

		public string CurrentUser { get; set; }

		public IndexViewModel(List<Book> books, List<Book> latestBooks, string currentUser)
		{
			this.Books = books;
			this.LatestBooks = latestBooks;
			this.CurrentUser = currentUser;
		}
	}
}
