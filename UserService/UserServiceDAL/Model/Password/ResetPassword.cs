using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserServiceDAL.Model.ChangeResetPassword
{
    public class ResetPassword
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
