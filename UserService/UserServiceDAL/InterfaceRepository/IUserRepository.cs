using UserServiceDAL.Model.User;

namespace UserServiceDAL.InterfaceRepository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByNickNameAsync(string nickname);

        Task<int> GetUserStatusByIdAsync(int userId);

        Task<int> SetUserAsync(User user);


        Task<User?> UpdateUserStatusAsync(int userId, int status);
        Task<User?> UpdateFirstNameAsync(string nickname, string newFirstname);
        Task<User?> UpdateLastNameAsync(string nickname, string newLastname);
        Task<User?> UpdateProfileDescriptionAsync(string nickname, string newProfileDescription);
        Task<User?> UpdateNickNameAsync(string nickname, string newNickname);
        Task<User?> UpdatePasswordAsync(string email, string hashedPassword, string salt);
    }
}
