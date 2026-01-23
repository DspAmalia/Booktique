using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booktique.Models.MainModels
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UserId")]
        public int UserId { get; set; }

        [Column("UserName")]
        [MaxLength(100)]
        public string? UserName { get; set; }

        [Column("UserEmail")]
        [MaxLength(100)]
        [EmailAddress]
        public string? UserEmail { get; set; }

        [Column("Password")]
        [MaxLength(100)]
        public string? Password { get; set; }

        [Column("Role")]
        [MaxLength(20)]
        public string? Role { get; set; }

        public string? ProfilePicture { get; set; }

        public bool IsSubscribed { get; set; } = false;
    }
}
