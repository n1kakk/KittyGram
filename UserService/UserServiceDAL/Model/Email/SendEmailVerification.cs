
namespace UserServiceDAL.Model.Email
{
    public class SendEmailVerification
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; } = true;
        public string VerificationToken { get; set; }
    }
}
