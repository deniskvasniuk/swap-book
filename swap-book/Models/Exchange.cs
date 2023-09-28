namespace swap_book.Models
{
    public class Exchange
    {
        
        public int ExchangeId { get; set; }
        
        public string Person { get; set; }
      
        public string Address { get; set; }
       
        public int BookId { get; set; }
        
        public DateTime Date { get; set; }
    }
}

