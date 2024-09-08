using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace UserServiceDAL.Model.User
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "NickName is required.")]
        [Display(Name = "NickName")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [JsonIgnore]
        public string Password { get; set; }
    }
}
