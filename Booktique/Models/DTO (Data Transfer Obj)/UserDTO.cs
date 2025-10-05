using System.ComponentModel.DataAnnotations;

namespace Booktique.Models.DTO__Data_Transfer_Obj_
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Please enter your username")]
        [StringLength(100, MinimumLength = 3)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(100, MinimumLength = 3)]
        public string? UserPassword { get; set; }

    }
}
