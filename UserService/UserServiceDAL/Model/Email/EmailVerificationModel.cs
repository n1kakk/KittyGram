
namespace UserServiceDAL.Model.Email
{
    public class EmailVerificationModel
    {
        public string Email { get; set; }
        public int UserId { get; set; }
        public string VerificationToken { get; set; }
        public DateTime TokenCreationDate { get; set; }
        public bool IsUsed { get; set; }
        public int TokenType { get; set; }
    }
}
