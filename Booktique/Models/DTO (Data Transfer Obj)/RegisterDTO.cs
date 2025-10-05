using System.ComponentModel.DataAnnotations;

namespace Booktique.Models.DTO__Data_Transfer_Obj_
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Please enter your username")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Password must contain minimum 3 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [StringLength(100, MinimumLength = 3)]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password must contain minimum 5 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your password again")]
        public string ConfirmPassword { get; set; }
    }
}
