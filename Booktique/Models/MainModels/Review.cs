using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Booktique.Models.MainModels
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // stele 1–5

        [MaxLength(1000)]
        public string? Comment { get; set; } // text opțional

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? GuestName { get; set; } // optional, pentru review fără user

        // relații
        [Required]
        public int BookId { get; set; }
        
        [JsonIgnore]
        public Book Book { get; set; } = default!;

        [Required]
        public int? UserId { get; set; }
        
        [JsonIgnore]
        public User User { get; set; } = default!;
    }
}
