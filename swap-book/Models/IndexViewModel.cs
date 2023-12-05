namespace swap_book.Models
{
    public class IndexViewModel
    {
        public List<Book> Books { get; set; }

        public List<Book> LatestBooks { get; set; }

        public ApplicationUser CurrentUser { get; set; }

        public IndexViewModel(List<Book> books, List<Book> latestBooks, ApplicationUser currentUser)
        {
            Books = books;
            LatestBooks = latestBooks;
            CurrentUser = currentUser;
        }
    }
}
