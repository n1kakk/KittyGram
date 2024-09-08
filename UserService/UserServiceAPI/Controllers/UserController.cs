using Microsoft.AspNetCore.Mvc;
using Nest;
using NLog;
using UserServiceDAL.Helpers;
using UserServiceDAL.InterfaceServices;
using UserServiceDAL.Model;
using UserServiceDAL.Model.ChangeResetPassword;
using UserServiceDAL.Model.DTOs;
using UserServiceDAL.Model.User;
using static UserServiceDAL.Services.EmailService;




namespace UserServiceAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private ILogger<UserController> _logger;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IElasticSearchService _elasticsearchService;

        public UserController(IUserService userService, IEmailService emailService,
            ILogger<UserController> logger, IElasticSearchService elasticSearchService)
        {
            _userService = userService;
            _emailService = emailService;
            _logger = logger;
            _elasticsearchService = elasticSearchService;

        }



        [HttpGet("GetNickNameUser/{nickName}")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        public async Task<IActionResult> GetNickNameUsers(string nickName)
        {
            var userDTO = await _userService.GetUserByNickNameAsync(nickName);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           return Ok(userDTO);
        }


        //[HttpGet("SearchNickName/{nickName}")]
        //[ProducesResponseType(200, Type = typeof(UserDTO))]
        //public async Task<IActionResult> SearchNickNames(string nickName)
        //{

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    //var searchResponse = _elasticClient.Search<Person>(s => s
        //    //    .From(0)
        //    //    .Size(10)
        //    //    .Index("person")
        //    //    .Query(q => q
        //    //         .Match(m => m
        //    //            .Field(f => f.FirstName)
        //    //            .Query("First1")
        //    //         )
        //    //    )
        //    //);


        //    //var searchResponse = _elasticClient.Search<Person>(s => s
        //    //    .Index("person")
        //    //    .Query(q => q
        //    //          .MatchPhrasePrefix(c => c
        //    //                .Field(p => p.FirstName)
        //    //                .Query(nickName)
        //    //          )
        //    //    )
        //    //);

        //    //var people = searchResponse.Documents;


        //    //return Ok(people);

        //    //var result = _userService.SearchByNickName(nickName);
        //    return Ok(result);
        //}


        [HttpGet("SearchNickName/{nickName}")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        public async Task<IActionResult> SearchNickNames(string nickName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _elasticsearchService.SearchNickNameAsync(nickName);
            if (result.Any())
            {
                return BadRequest($"NickName {nickName} is already taken");
            }
            //_logger.LogInformation("Signup failed: {nickName}", nickName);
            return Ok($"Nickname {nickName} is available");
        }


        [HttpPost("SignUp")]
        public async Task<IActionResult> Signup(SignupRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userEmailDTO = await _userService.Signup(model);
            if (userEmailDTO == null)
            {
                _logger.LogError("Signup failed: {NickName}", model.NickName);
                throw new AppException("Signup failed");
            }
            var emailSent = await _emailService.SendEmailVerificationAsync(userEmailDTO.VerificationToken);
            if (!emailSent)
            {
                _logger.LogError("Failed to send email verification: {NickName}", model.NickName);
                throw new AppException("Failed to send email verification");
            }


            return Ok(new { Message = "Verification Email was sent" });

        }


        [HttpPost("UpdateNickName/{nickName}")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        public async Task<IActionResult> UpdateNickName(string nickName, string newNickname)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(newNickname)) return BadRequest("No new nickname was provided.");

            var UserUpdateDTO = await _userService.UpdateNickNameAsync(nickName, newNickname);
            if (UserUpdateDTO == null)
            {
                _logger.LogError("Failed to update NickName: {nickName}", nickName);
                throw new AppException("Failed to update NickName");
            }

            return Ok(new { Message = "NickName updated", User = UserUpdateDTO });
        }


        [HttpPost("UpdateProfileDescription/{nickName}")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        public async Task<IActionResult> UpdateProfileDescription(string nickName, string? description)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(description)) return BadRequest("No description was provided.");

            var UserUpdateDTO = await _userService.UpdateProfileDescriptionAsync(nickName, description);
            if (UserUpdateDTO == null)
            {
                _logger.LogError("Failed to update Profile description: {nickName}", nickName);
                throw new AppException("Failed to update Profile description");
            }

            return Ok(new { Message = "ProfileDescription updated", User = UserUpdateDTO });
        }


        [HttpPost("UpdateFirstNameLastName/{nickName}")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        public async Task<IActionResult> UpdateFirstNameLastName(string nickName, string? newFirstname, string? newLastname)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (string.IsNullOrEmpty(newFirstname) && string.IsNullOrEmpty(newLastname)) return BadRequest("No new FirstName or LastName was provided.");

            var UserUpdateDTO = await _userService.UpdateFirstNameLastNameAsync(nickName, newFirstname, newLastname);

            if (UserUpdateDTO == null)
            {
                _logger.LogError("Failed to update LastName FirstName: {nickName}", nickName);
                throw new AppException("Failed to update LastName FirstName");
            }

            return Ok(new { Message = "FirstName LastName updated", User = UserUpdateDTO });
        }


        [HttpPost("LogIn")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var tokenModel = await _tokenService.GenerateTokenAsync(existingUser.NickName);
            //return Ok((tokenModel));
            var userDTO = await _userService.LoginAsync(model);
            if (userDTO == null)
            {
                _logger.LogError("Login failed: {NickName}", model.NickName);
                throw new AppException("Login failed");
            }
            return Ok(new { Message = "You are logged in", User = userDTO });
        }


        [HttpPost("/ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var changing = await _userService.ChangePasswordAsync(changePasswordModel);
            if (!changing)
            {
                _logger.LogError("Failed to update password: {nickName}", changePasswordModel.NickName);
                throw new AppException("Failed to update password");
            }

            return Ok("Password was changed");
        }


        [HttpGet("/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordForm forgotPasswordForm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var forgotErrorMessage = "Forgot password failed";

            var userDTO = await _userService.GetUserByNickNameAsync(forgotPasswordForm.NickName);
            if (userDTO == null)
            {
                _logger.LogError("Login failed: {NickName}", forgotPasswordForm.NickName);
                throw new AppException(forgotErrorMessage);
            }

            var setToken = await _emailService.SetPasswordVerificationTokenAsync(userDTO.Email, userDTO.UserId);
            if (setToken == null) throw new AppException(forgotErrorMessage);

            var emailSent = await _emailService.SendEmailResetPasswordAsync(setToken.VerificationToken);

            if (!emailSent)
            {
                _logger.LogError("Failed to send email verification: {NickName}", forgotPasswordForm.NickName);
                throw new AppException("Failed to send email verification");
            }

            return Ok(new { Message = "Verification Email was sent" });

        }


        [HttpGet("/ResetPassword")]
        public async Task<IActionResult> ResetPassword(int userId, string token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                _logger.LogError("Failed to reset password, GetUserByIdAsync: {userId}", userId);
                throw new AppException("Failed to reset password");
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var verificationStatus = await _emailService.VerifyTokenAsync(token);
            if (verificationStatus == VerificationStatus.Expired) return BadRequest("Token is expired");

            if (verificationStatus == VerificationStatus.TokenIsUsed) return BadRequest("You password was already changed");
            if (verificationStatus != VerificationStatus.Verified) return BadRequest($"'{verificationStatus}'");


            var update = await _emailService.UpdateTokenIsUsedAsync(token, true);
            if (update == null)
            {
                _logger.LogError("Failed to reset password, UpdateTokenIsUsedAsync: {userId}", userId);
                throw new AppException("Failed to reset password");
            }

            return RedirectToAction("ResetPasswordAction", new ResetPassword { UserId = userId, Token = token });
        }


        [HttpPost("/ResetPasswordAction")]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверка соответствия нового пароля и его подтверждения
            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "The new password and confirmation password do not match.");
                return BadRequest(ModelState);
            }

            var reseting = await _userService.ResetPasswordAsync(model.UserId, model.NewPassword);
            if (!reseting)
            {
                _logger.LogError("Failed to reset password, ResetPasswordAsync: {userId}", model.UserId);
                throw new AppException("Failed to reset password");
            }

            return Ok("Password was reseted");
        }


        [HttpGet("/SendEmailVerification")]
        public async Task<IActionResult> SendEmailVerification(string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var userEmailDTO = await _userService.SendEmailVerificationAgainAsync(email);
            if (userEmailDTO == null)
                return BadRequest("You are registered");

            var setToken = await _emailService.SetEmailVerificationTokenAsync(userEmailDTO.Email, userEmailDTO.UserId);
            if (setToken == null)
            {
                _logger.LogError("Failed to send email verification again, SetEmailVerificationTokenAsync: {UserId}", userEmailDTO.UserId);
                throw new AppException("Failed to send email verification again");
            }

            var emailSent = await _emailService.SendEmailVerificationAsync(setToken.VerificationToken);
            if (!emailSent)
            {
                _logger.LogError("Failed to send email verification, SendEmailVerificationAsync: {UserId}", userEmailDTO.UserId);
                throw new AppException("Failed to send email verification");
            }
            return Ok(new { Message = "Verification Email was sent" });

        }



    }
}
