using System.ComponentModel.DataAnnotations;

namespace Booktique.Models.ViewModels
{
    public class LoginDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Provide User Name")]
        public string? UserName { get; set; }

        
        
        public string? UserEmail { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Provide Password")]
        public string? Password { get; set; }
    }
}
