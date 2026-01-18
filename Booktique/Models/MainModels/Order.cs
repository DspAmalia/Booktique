using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Booktique.Models.MainModels
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
