using UserServiceDAL.InterfaceRepository;
using UserServiceDAL.InterfaceServices;
using AutoMapper;
using UserServiceDAL.Model.DTOs;
using UserServiceDAL.Model.ChangeResetPassword;
using UserServiceDAL.Model.User;
using UserServiceDAL.Model.Email;
using UserServiceDAL.Helpers;
using NLog;
using UserServiceDAL.Model.ElasticSearch;

namespace UserServiceDAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IBaseRepository _baseRepository;
        private readonly IEmailService _emailService;
        private ILogger<UserService> _logger;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IElasticSearchService _elasticsearchService;
        public UserService(IUserRepository userRepository, IMapper mapper, 
            IBaseRepository baseRepository, IEmailService emailService, ILogger<UserService> logger, IElasticSearchService elasticsearchService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _baseRepository = baseRepository;
            _emailService = emailService;
            _logger = logger;
            _elasticsearchService = elasticsearchService;
        }
        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (IsNull(user)) throw new AppException($"User does not exist");
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<UserDTO?> LoginAsync(LoginRequest model)
        {
            var user = await _userRepository.GetUserByNickNameAsync(model.NickName);
            if (user == null || !PasswordHasher.VerifyPassword(model.Password, user.HashedPassword, user.Salt))
            {
                throw new KeyNotFoundException("User not found or wrong password");
            }
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<UserDTO?> GetUserByNickNameAsync(string nickname)
        {
            var user = await _userRepository.GetUserByNickNameAsync(nickname);
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<UserEmailDTO?> Signup(SignupRequest model)
        {
            var existingUserByEmail = await GetUserByEmailAsync(model.Email);
            var existingUserByNickName = await GetUserByNickNameAsync(model.NickName);
            if (existingUserByEmail != null)
            {
                //_logger.LogError("User with the email {Email} already exists", model.Email);
                throw new AppException($"User with the email '{model.Email}' already exists");
            }
            else if (existingUserByNickName != null)
            {
                //_logger.LogError("User with the nickname {NickName} already exists", model.NickName);
                throw new AppException($"User '{model.NickName}' already exists");
            }


            string salt = PasswordHasher.Salt();
            string hashedPassword = PasswordHasher.HashPassword(model.Password + salt);
            var emailVerificationModel = new EmailVerificationModel();

            var user = new User
            {
                Email = model.Email,
                NickName = model.NickName,
                HashedPassword = hashedPassword,
                Salt = salt,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthday = model.Birthday,
                ProfileDescription = model.ProfileDescription,
            };

            _logger.LogInformation("Beginning transaction for user registration");
            await _baseRepository.BeginTransactionAsync();
            int userId = await _userRepository.SetUserAsync(user);
            emailVerificationModel = await _emailService.SetVerificationTokenAsync(model.Email, userId, EmailService.TokenType.EmailVerificationToken);

            var userModelElastic = new UserModelElastic
            {
                Id = userId,
                NickName = model.NickName,
                Birthday = model.Birthday
            }; 
            var elasticResult = await _elasticsearchService.SetUserAsync(userModelElastic);

            if (userId == 0 || emailVerificationModel == null || !elasticResult)
            {
                _logger.LogError($"Failed to set user or email verification token. Rolling back transaction. {model.NickName}");
                await _baseRepository.RollbackTransactionAsync();
                return null;
            }

            await _baseRepository.CommitTransactionAsync();
            _logger.LogInformation("User registered successfully. Email: {Email}, Nickname: {NickName}", model.Email, model.NickName);
            var userEmailDTO = _mapper.Map<UserEmailDTO>(emailVerificationModel);
            return userEmailDTO;
        }

        //public async Task<UserDTO?> UpdateBirthday(string nickname, string? newBirthday)
        //{
        //    var userDTO = await _userRepository.GetNickNameUserAsync(nickname);

        //    if (userDTO == null) return null;
        //    if (IsNull(newBirthday)) return null;

        //    return await _userRepository.UpdateBirthdayAsync(nickname, newBirthday);
        //}

        public async Task<UserUpdateDTO?> UpdateFirstNameLastNameAsync(string nickname, string? newFirstname, string? newLastname)
        {
            var user = await GetUserByNicknameOrThrowExceptionIfNotFound(nickname);

            if (!IsNull(newFirstname)) user = await _userRepository.UpdateFirstNameAsync(nickname, newFirstname);
            if (!IsNull(newLastname)) user = await _userRepository.UpdateLastNameAsync(nickname,newLastname);
            if (IsNull(user)) return null;

            var UserUpdateDTO = _mapper.Map<UserUpdateDTO>(user);
            return UserUpdateDTO;
        }

        public async Task<UserUpdateDTO?> UpdateNickNameAsync(string nickname, string newNickname)
        {

            await GetUserByNicknameOrThrowExceptionIfNotFound(nickname);

            var existingUser = await _userRepository.GetUserByNickNameAsync(newNickname);
            if (!IsNull(existingUser))
            {
                _logger.LogError($"User with the username '{newNickname}' already exists");
                throw new AppException($"User with the username '{newNickname}' already exists");
            }

            _logger.LogInformation("Beginning transaction for NickName update");
            await _baseRepository.BeginTransactionAsync();

            var updatedUser = await _userRepository.UpdateNickNameAsync(nickname, newNickname);

            var userModelElastic = new UserModelElastic
            {
                Id = updatedUser.UserId,
                NickName = newNickname
            };

            var elasticResult = await _elasticsearchService.UpdateUserNickNameAsync(userModelElastic);

            if (IsNull(updatedUser) || !elasticResult)
            {
                _logger.LogError($"Failed to update User nickname. Rolling back transaction. {nickname}");
                await _baseRepository.RollbackTransactionAsync();
                return null;
            }
            await _baseRepository.CommitTransactionAsync();
            _logger.LogInformation($"Nickname was updated successfully. Old: {nickname} New: {newNickname}");

            var UserUpdateDTO = _mapper.Map<UserUpdateDTO>(updatedUser);
            return UserUpdateDTO;
        }

        public async Task<UserUpdateDTO?> UpdateProfileDescriptionAsync(string nickname, string newProfileDescription)
        {
            await GetUserByNicknameOrThrowExceptionIfNotFound(nickname);

            var updatedUser = await _userRepository.UpdateProfileDescriptionAsync(nickname, newProfileDescription);
            if (IsNull(updatedUser)) return null;

            var UserUpdateDTO = _mapper.Map<UserUpdateDTO>(updatedUser);
            return UserUpdateDTO;

        }

        public async Task<StatusType?> GetUserStatusByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (IsNull(user)) return null;

            var statusInt = await _userRepository.GetUserStatusByIdAsync(userId);
            if (Enum.IsDefined(typeof(StatusType), statusInt)) return (StatusType)statusInt;
            return null;

        }

        public async Task<string?> UpdateUserStatusAsync(int userId, StatusType status)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            await _userRepository.UpdateUserStatusAsync(userId, (int)status);
            return "Status was changed";
        }

        public async Task<bool> UpdateEmailIsVerifiedStatus(string token, int userId, bool tokenIsUsed, StatusType status)
        {
            _logger.LogInformation("Beginning transaction for UpdateEmailIsVerifiedStatus");
            await _baseRepository.BeginTransactionAsync();
            var updateUser = await UpdateUserStatusAsync(userId, status);
            var updateIsUsed = await _emailService.UpdateTokenIsUsedAsync(token, tokenIsUsed);
            if (string.IsNullOrEmpty(updateUser) || updateIsUsed == null)
            {
                _logger.LogError($"Failed to UpdateEmailIsVerifiedStatus. Rolling back transaction. userId: {userId}");
                await _baseRepository.RollbackTransactionAsync();
                return false;
            }
            _logger.LogInformation("UpdateEmailIsVerifiedStatus success");
            await _baseRepository.CommitTransactionAsync();
            return true;

        }

        public async Task<bool> ChangePasswordAsync(ChangePassword changePasswordModel)
        {

            var user = await GetUserByNicknameOrThrowExceptionIfNotFound(changePasswordModel.NickName);

            var userVerified = PasswordHasher.VerifyPassword(changePasswordModel.OldPassword, user.HashedPassword, user.Salt);

            if (!userVerified) throw new AppException("Wrong password");

            string salt = PasswordHasher.Salt();
            string hashedPassword = PasswordHasher.HashPassword(changePasswordModel.NewPassword + salt);

            var updateResult = await _userRepository.UpdatePasswordAsync(user.Email, hashedPassword, salt);
            if (updateResult == null) return false;
            return true;
        }


        public async Task<bool> ResetPasswordAsync(int userId,  string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (IsNull(user))
            {
                _logger.LogError($"User does not exist. userId: {userId}");
                throw new AppException("User does not exist");
            }

            string salt = PasswordHasher.Salt();
            string hashedPassword = PasswordHasher.HashPassword(newPassword + salt);

            var updateResult = await _userRepository.UpdatePasswordAsync(user.Email, hashedPassword, salt);
            if (updateResult == null) return false;
            return true;
        }

        public async Task<UserEmailDTO?> SendEmailVerificationAgainAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (IsNull(user))
            {
                _logger.LogError($"User does not exist. email: {email}");
                throw new AppException("User does not exist");
            }

            var userStatus = await GetUserStatusByIdAsync(user.UserId);
            if (userStatus == StatusType.VerifiedEmail) return null;
            var userEmailDTO = _mapper.Map<UserEmailDTO>(user);
            return userEmailDTO;

        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        private async Task<User> GetUserByNicknameOrThrowExceptionIfNotFound(string nickname)
        {
            var user = await _userRepository.GetUserByNickNameAsync(nickname);
            if (IsNull(user)) throw new AppException($"'{nickname}' does not exist");
            return user;
        }

        private bool IsNull<T>(T value)
        {
            return value == null;
        }
        public enum StatusType
        {
            UnverifiedEmail = 1,
            VerifiedEmail = 2
        }
    }
}
