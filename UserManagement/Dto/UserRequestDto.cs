using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Dto
{
    public class UserRequestDto
    {
        [Required(ErrorMessage ="First Name Should Not Be Empty")]
        [RegularExpression(@"^[a-zA-Z]{1,20}$")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Should Not Be Empty")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage ="EMAIL ID IS REQUIRED")]
        public string Email { get; set; }

        [PasswordPropertyText]
        [Required (ErrorMessage ="Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d!@#$%^&*]{8,16}$") ]
        public string Password { get; set; }

    }
}
