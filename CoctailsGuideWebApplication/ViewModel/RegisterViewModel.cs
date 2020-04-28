using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace CoctailsGuideWebApplication.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Year of birth")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} must have a minimum of {2} and a maximum of {1} characters.", MinimumLength = 3)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Your password and confirmation password do not match")]
        [Display(Name = "Password confirm")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

    }
}
