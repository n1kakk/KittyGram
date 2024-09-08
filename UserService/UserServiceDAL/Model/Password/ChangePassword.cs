using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserServiceDAL.Model.ChangeResetPassword
{
    public class ChangePassword
    {
        [Required]
        public string NickName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        [JsonIgnore]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characaters long!")]
        [Display(Name = "New Password")]
        [JsonIgnore]
        public string NewPassword { get; set; }
    }
}
