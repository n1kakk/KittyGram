using UserServiceDAL.Model.ChangeResetPassword;
using UserServiceDAL.Model.DTOs;
using UserServiceDAL.Model.User;
using static UserServiceDAL.Services.UserService;

namespace UserServiceDAL.InterfaceServices
{
    public interface IUserService
    {
        public Task<UserDTO?> GetUserByIdAsync(int id);
        public Task<UserDTO?> GetUserByNickNameAsync(string nickname);
        public Task<UserDTO?> GetUserByEmailAsync(string email);
        public Task<StatusType?> GetUserStatusByIdAsync(int userId);


        public Task<UserDTO?> LoginAsync(LoginRequest model);
        public Task<UserEmailDTO> Signup(SignupRequest model);


        Task<bool> ChangePasswordAsync(ChangePassword changePasswordModel);
        Task<bool> ResetPasswordAsync(int userId, string newPassword);

        Task<UserEmailDTO?> SendEmailVerificationAgainAsync(string email);


        Task<string?> UpdateUserStatusAsync(int userId, StatusType status);
        Task<UserUpdateDTO?> UpdateFirstNameLastNameAsync(string nickname, string? newFirstname, string? newLastname);
        Task<UserUpdateDTO?> UpdateProfileDescriptionAsync(string nickname, string? newProfileDescription);
        Task<UserUpdateDTO?> UpdateNickNameAsync(string nickname, string? newNickname);
        //Task<UserDTO?> UpdateBirthday(string nickname, string? newBirthday);
        Task<bool> UpdateEmailIsVerifiedStatus(string token, int userId, bool tokenIsUsed, StatusType status);

    }
}
