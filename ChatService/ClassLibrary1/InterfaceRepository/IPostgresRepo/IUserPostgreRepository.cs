using ClassLibrary1.Models.PostgreModels.User;


namespace ClassLibrary1.InterfaceRepository.IPostgresRepo;

public interface IUserPostgreRepository
{
    Task<bool> SetUserAsync(User user);

    Task<bool> UpdateUserActivity(User user);

    Task<User?> GetUserByNickNameAsync(string nickName);
    Task<List<User?>> GetUsersByActivityStatusAsync(int activityStatus);
}
