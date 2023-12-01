using Microsoft.AspNetCore.Identity;

namespace swap_book.Models
{
    public class ApplicationUser:IdentityUser
    {
	    public string? Name { get; set; }
	    public string? ProfilePicture { get; set; }
        public Guid? PublicProfileLink { get; set; }
        public List<Book> Books { get; set; }
        public List<Wishlist> Wishlists { get; set; }
    }
}
