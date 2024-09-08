using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserServiceDAL.Model.User
{
    public class SignupRequest
    {
        [Required]
        [Display(Name = "NickName")]
        public string NickName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Not valid e-mail address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characaters long!")]
        [Display(Name = "Password")]
        [JsonIgnore]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and its confirmation do not match")]
        [JsonIgnore]
        public string ConfirmPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ProfileDescription { get; set; }
        public string? ImgUrl { get; set; }
    }
}
