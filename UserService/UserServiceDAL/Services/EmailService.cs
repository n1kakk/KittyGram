using UserServiceDAL.InterfaceServices;
using UserServiceDAL.InterfaceRepository;
using UserServiceDAL.Security;
using UserServiceDAL.Model.Email;
using AutoMapper;
using UserServiceDAL.Model.DTOs;
using UserServiceDAL.Helpers;
namespace UserServiceDAL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IEmailSenderService _emailVerificationSender;
        private readonly IMapper _mapper;
        public EmailService(IEmailVerificationRepository emailVerificationRepository, IEmailSenderService emailVerificationSender, IMapper mapper) 
        {
            _emailVerificationRepository = emailVerificationRepository;
            _emailVerificationSender = emailVerificationSender;
            _mapper = mapper;
        }

        public async Task<bool> SendEmailVerificationAsync(string verificationToken)
        {

            var sendEmailVerification = await PrepareEmailVerificationAsync(verificationToken, "Email Verification", "Verify Email", "EmailVerification");
            return await SendEmailAsync(sendEmailVerification);
        }

        public async Task<bool> SendEmailResetPasswordAsync(string verificationToken)
        {

            var sendEmailVerification = await PrepareEmailVerificationAsync(verificationToken, "Password Reset", "Reset Password", "ResetPassword");
            return await SendEmailAsync(sendEmailVerification);
        }


        private async Task<SendEmailVerification?> PrepareEmailVerificationAsync(string verificationToken, string subject, string linkName, string linkAddress)
        {
            var emailVerificationModel = await _emailVerificationRepository.GetEmailVerificationByTokenAsync(verificationToken);
            if (emailVerificationModel == null) return null;
            
            string tokenLink = EmailVerification.GenerateVerificationLink(emailVerificationModel.UserId, emailVerificationModel.VerificationToken, linkAddress);

            string bodyWithLink = $"Please proceed by clicking the link below:<br/>" +
                $"<a href='{tokenLink}'>{linkName}</a>";

            var sendEmailVerification = new SendEmailVerification
            {
                ToAddress = emailVerificationModel.Email,
                Subject = subject,
                Body = bodyWithLink,
                IsBodyHtml = true,
                VerificationToken = emailVerificationModel.VerificationToken
            };
            return sendEmailVerification;
        }
        private async Task<bool> SendEmailAsync(SendEmailVerification sendEmailVerification)
        { 
            if (sendEmailVerification == null) return false;

            var emailResult = await _emailVerificationSender.SendVerificationEmailAsync(sendEmailVerification);

            return emailResult; 
        }


        public async Task<VerificationStatus> VerifyTokenAsync(string token)
        {
            var dbEmailVerification = await _emailVerificationRepository.GetEmailVerificationByTokenAsync(token);

            if (dbEmailVerification == null || dbEmailVerification.TokenType == 3) return VerificationStatus.InvalidToken;

            if (dbEmailVerification.IsUsed) return VerificationStatus.TokenIsUsed;

            DateTime tokenCreationDate = dbEmailVerification.TokenCreationDate;

            if (!EmailVerification.VerifyTokenCreationDate(tokenCreationDate)) return VerificationStatus.Expired;
            return VerificationStatus.Verified;
        }



        public async Task<EmailVerificationModel?> UpdateTokenIsUsedAsync(string token, bool isUsed)
        {
            var emailVerification = await _emailVerificationRepository.GetEmailVerificationByTokenAsync(token);
            if (emailVerification == null) throw new AppException($"Invalid token");
            return await _emailVerificationRepository.UpdateTokenIsUsedAsync(token, isUsed);
        }


        public async Task<UserEmailDTO?> SetPasswordVerificationTokenAsync(string email, int userId)
        {
            await _emailVerificationRepository.InvalidateTokensByTypeAsync(userId, (int)TokenType.PasswordVerificationToken);
            var setToken = await SetVerificationTokenAsync(email, userId, TokenType.PasswordVerificationToken);
            var UserEmailDTO = _mapper.Map<UserEmailDTO>(setToken);
            return UserEmailDTO;
        }

        public async Task<UserEmailDTO?> SetEmailVerificationTokenAsync(string email,int userId)
        {
            await _emailVerificationRepository.InvalidateTokensByTypeAsync(userId, (int)TokenType.EmailVerificationToken);
            var setToken = await SetVerificationTokenAsync(email, userId, TokenType.EmailVerificationToken);
            var UserEmailDTO = _mapper.Map<UserEmailDTO>(setToken);
            return UserEmailDTO;
        }


        public async Task<EmailVerificationModel?> SetVerificationTokenAsync(string email, int userId, TokenType tokenType)
        {
            string verificationToken = EmailVerification.GenerateVerificationToken();
            var emailVerification = new EmailVerificationModel
            {
                UserId = userId,
                Email = email,
                VerificationToken = verificationToken,
                TokenCreationDate = DateTime.Now,
                TokenType = (int)tokenType
            };

            await _emailVerificationRepository.SetVerificationTokenAsync(emailVerification);
            return emailVerification;
        }

        public enum TokenType
        {
            PasswordVerificationToken = 1,
            EmailVerificationToken = 2,
            InvalidToken = 3
        }

        public enum VerificationStatus
        {
            NotFound,
            TokenIsUsed,
            InvalidToken,
            Expired,
            Verified
        }

    }
}
    