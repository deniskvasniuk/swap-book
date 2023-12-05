
namespace swap_book.Models
{
    public class Exchange
    {
        public int ExchangeId { get; set; } 

        public int BookId { get; set; }

        public string UserId { get; set; }

        public int ExchangedBookId { get; set; }

        public ExchangeStatus Status { get; set; } = ExchangeStatus.Pending;

        public Book Book { get; set; }

        public ApplicationUser User { get; set; }

        public Book ExchangedBook { get; set; }

        public DateTime CreatedAt { get; set; }

        public enum ExchangeStatus
        {
            Pending,
            Accepted,
            Rejected
        }
    }

}

