using System;
using System.ComponentModel.DataAnnotations;


namespace cSharpTest.Models
{
    public class User : BaseEntity
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }

    }

        public class RegisterViewModel : BaseEntity
    {

        [Required]
        [RegularExpression("^([A-Za-z]*)$", ErrorMessage = "Names may not contain numbers or special characters.")]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^([A-Za-z]*)$", ErrorMessage = "Names may not contain numbers or special characters.")]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+])[A-Za-z\\d!@#$%^&*()_+]{8,20}", ErrorMessage = "Password must contain at least 1 number, 1 letter, and 1 special character.")]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        public string PasswordConfirmation { get; set; }
    }


    public class LoginViewModel : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }


    //This is a WRAPPER!
    public class LoginRegViewModel : BaseEntity
    {
        public LoginViewModel loginVM { get; set; }
        public RegisterViewModel registerVM { get; set; }
    }
}
