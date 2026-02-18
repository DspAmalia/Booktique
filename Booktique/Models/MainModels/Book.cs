using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Booktique.Models.MainModels
{
    [Table("book")]
    public class Book: IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("BookId")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Please enter the title")]
        [StringLength(1000, MinimumLength = 1)]
        [Column("BookTitle")]
        public string? BookTitle { get; set; }

        [Required(ErrorMessage = "Please enter the author")]
        [StringLength(100, MinimumLength = 3)]
        [Column("BookAuthor")]
        public string? BookAuthor { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [StringLength(100000, MinimumLength = 3)]
        [Column("BookDescription")]
        public string? BookDescription { get; set; }

        [Range(1900, int.MaxValue, ErrorMessage = "Please enter a year after 1900")]
        [Column("BookYear")]
        public int BookYear { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BookYear > DateTime.Now.Year)
            {
                yield return new ValidationResult(
                    $"Year cannot be greater than {DateTime.Now.Year}",
                    new[] { nameof(BookYear) });
            }
        }

        [Required(ErrorMessage = "Please enter the number of pages")]
        [Column("BookNumberPag")]
        public int? BookNumberPag { get; set; }

        [Required(ErrorMessage = "Please enter the publishing house")]
        [StringLength(1000, MinimumLength = 1)]
        [Column("BookPublishingHouse")]
        public string? BookPublishingHouse { get; set; }

        [Required(ErrorMessage = "Please enter the category")]
        [StringLength(1000, MinimumLength = 1)]
        [Column("BookCategory")]
        public string? BookCategory { get; set; }

        [Required(ErrorMessage = "Please enter the language")]
        [StringLength(1000, MinimumLength = 1)]
        [Column("BookLanguage")]
        public string? BookLanguage { get; set; }

        [Required(ErrorMessage = "Please enter the stock")]
        [Range(0, 300000)]
        [Column("BookStock")]
        public int? BookStock { get; set; }

        [DataType(DataType.Currency)]
        [Column("BookPrice",TypeName = "decimal(18,2)")]
        [Range(1, 300000, ErrorMessage = "Please enter a price between 1 and $300.000")]
        [Required(ErrorMessage = "Please enter a price")]
        public decimal? BookPrice { get; set; }

        [DataType(DataType.Url)]
        [Column("BookCoverPath")]
        public string? BookCoverPath { get; set; }

        [Required]
        [Range(0, 5)]
        [Column("BookRating")]
        public int? BookRating { get; set; }
        public string Condition { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.Now;

        public int? SellerId { get; set; }
        public User Seller { get; set; }

        [JsonIgnore]
        public List<Review> Reviews { get; set; } = new();

        [JsonIgnore]
        public List<Favorite> Favorites { get; set; } = new();
    }
}
