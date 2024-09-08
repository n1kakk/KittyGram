using UserServiceDAL.Model.Email;

namespace UserServiceDAL.InterfaceServices
{
    public interface IEmailSenderService
    {
        public Task<bool> SendVerificationEmailAsync(SendEmailVerification email);
    }
}
