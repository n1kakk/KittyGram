using Microsoft.AspNetCore.Mvc;
using NLog;
using UserServiceDAL.Helpers;
using UserServiceDAL.InterfaceServices;
using static UserServiceDAL.Services.EmailService;
using static UserServiceDAL.Services.UserService;


namespace UserServiceAPI.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private ILogger<EmailController> _logger;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public EmailController(IEmailService emailService, IUserService userService, ILogger<EmailController> logger) 
        { 
            _emailService = emailService;
            _userService = userService;
            _logger = logger;
        }


        [HttpGet("/EmailVerification")]
        public async Task<IActionResult> VerifyEmail(int userId, string token)
        {
            //var verificationResult = await _emailService.VerifyTokenAsync(token);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                _logger.LogError("Failed to verify email: {userId}", userId);
                throw new AppException("Failed to verify email");
            }

            var verificationStatus = await _emailService.VerifyTokenAsync(token);


            if (verificationStatus == VerificationStatus.Expired) return BadRequest("Token is expired");

            if (verificationStatus == VerificationStatus.TokenIsUsed) return BadRequest("You are already registered");

            if (verificationStatus != VerificationStatus.Verified) return BadRequest($"'{verificationStatus}'");

            var updateEmailIsVerifiedStatus = await _userService.UpdateEmailIsVerifiedStatus(token, userId, true, StatusType.VerifiedEmail);
            if (!updateEmailIsVerifiedStatus)
            {
                _logger.LogError("Failed to verify email, UpdateEmailIsVerifiedStatus: {userId}", userId);
                throw new AppException("Failed to verify Email");
            }
            return Ok("Email is verified");
        }

    }
}
