using UserServiceDAL.Model.DTOs;
using UserServiceDAL.Model.Email;
using static UserServiceDAL.Services.EmailService;

namespace UserServiceDAL.InterfaceServices
{
    public interface IEmailService
    {
        Task<EmailVerificationModel?> SetVerificationTokenAsync(string email, int userId, TokenType tokenType);
        Task<UserEmailDTO?> SetEmailVerificationTokenAsync(string email, int userId);
        Task<UserEmailDTO?> SetPasswordVerificationTokenAsync(string email,int userId);

        Task<VerificationStatus> VerifyTokenAsync(string token);

        Task<EmailVerificationModel?> UpdateTokenIsUsedAsync(string token, bool isUsed);


        Task<bool> SendEmailResetPasswordAsync(string verificationToken);
        Task<bool> SendEmailVerificationAsync(string verificationToken);
    }
}
