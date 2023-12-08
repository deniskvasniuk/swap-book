namespace swap_book.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public  Book Book { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
