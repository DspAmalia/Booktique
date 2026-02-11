using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booktique.Models.MainModels
{
    public class AIMessage
    {
        [Key]
        public int AIMessageId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("user")]
        public virtual User User { get; set; } 

        [Required]
        public string Sender { get; set; } // "user" sau "ai"

        [Required]
        public string Text { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now; 
    }
}