using System.ComponentModel.DataAnnotations;

namespace UserServiceDAL.Model.ChangeResetPassword
{
    public class ForgotPasswordForm
    {
        //[Required]
        //[EmailAddress(ErrorMessage = "Not valid e-mail address")]
        //[Display(Name = "Email")]
        //public string Email { get; set; }
        [Required(ErrorMessage = "NickName is required.")]
        [Display(Name = "NickName")]
        public string NickName { get; set; }
    }
}
