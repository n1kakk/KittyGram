using UserServiceDAL.Model.Email;

namespace UserServiceDAL.InterfaceRepository
{
    public interface IEmailVerificationRepository
    {
        Task<EmailVerificationModel?> GetEmailVerificationByEmailAsync(string email);
        Task<EmailVerificationModel?> GetEmailVerificationByUserIdAsync(int userId);
        Task<EmailVerificationModel?> GetEmailVerificationByTokenAsync(string verificationToken);

        Task SetVerificationTokenAsync(EmailVerificationModel emailVerificationModel);

        Task<EmailVerificationModel?> UpdateVerificationTokenAsync(string email, string verificationToken, DateTime tokenCreationDate);
        Task<EmailVerificationModel?> UpdateTokenIsUsedAsync(string verificationToken, bool isUsed);

        Task InvalidateTokensByTypeAsync(int userId, int tokenType);
    }
}
