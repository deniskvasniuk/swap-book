namespace swap_book.Models
{
	public class Message
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string? SenderId { get; set; }
		public string RecipientId { get; set; }
		public string Content { get; set; }
		public DateTime? SentDate { get; set; }
        public bool IsRead { get; set; }
    }
}
