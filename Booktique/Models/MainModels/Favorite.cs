using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Booktique.Models.MainModels
{
    public class Favorite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FavoriteId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User? User { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        public Book? Book { get; set; }

        [ForeignKey("Folder")]
        public int? FolderId { get; set; }
        public Folder? Folder { get; set; }

    }
}
