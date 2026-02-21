using System.ComponentModel.DataAnnotations.Schema;

namespace Booktique.Models.MainModels
{
    public class Notification
    {
        public int NotificationId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Order")]
        public int? OrderId { get; set; } 
        public Order? Order { get; set; }

        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public string? TargetUrl { get; set; }
    }
}
