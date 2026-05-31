using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booktique.Models.MainModels
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        public int? SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual User? Sender { get; set; }

        public int? ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User? Receiver { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}