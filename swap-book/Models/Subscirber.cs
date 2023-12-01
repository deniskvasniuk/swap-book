using System;
using System.ComponentModel.DataAnnotations;

namespace swap_book.Models
{
    public class Subscriber
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string SubscriberEmail { get; set; }

        public DateTime? SubscribitionTime { get; set; }

        public bool? Confirmed { get; set; }

        public string? ConfirmationToken { get; set; }
    }
}