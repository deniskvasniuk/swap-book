namespace swap_book.Models
{
    public class Exchange
    {
        // ID покупки
        public int ExchangeId { get; set; }
        // ім’я і прізвище покупця
        public string Person { get; set; }
        // адреса покупця
        public string Address { get; set; }
        // ID книги
        public int BookId { get; set; }
        // дата покупки
        public DateTime Date { get; set; }
    }
}

