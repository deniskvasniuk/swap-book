using Microsoft.AspNetCore.Identity;

namespace swap_book.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Name { get; set; }
        public string? ProfilePicture { get; set; }
        public Guid? PublicProfileLink { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Facebook { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? Description { get; set; }

        public List<Book> Books { get; set; }
        public List<Wishlist> Wishlists { get; set; }
    }
}
