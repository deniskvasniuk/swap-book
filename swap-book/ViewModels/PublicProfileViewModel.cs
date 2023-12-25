
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using swap_book.Models;

namespace swap_book.ViewModels
{
    public class PublicProfileViewModel
    {
        public List<Book> BooksOnOfferList { get; set; }
        public List<Book> BooksWishedList { get; set; }
        public ApplicationUser User { get; set; }
        public int BooksOnOffer { get; set; }
        public int BooksWished { get; set; }
        public int BooksReceived { get; set; }
        public int BooksSent { get; set; }
    }
}
