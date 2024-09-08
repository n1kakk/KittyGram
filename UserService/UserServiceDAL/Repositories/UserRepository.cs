using System.Data;
using UserServiceDAL.InterfaceRepository;
using Dapper;
using UserServiceDAL.Model.User;
using Microsoft.AspNetCore.DataProtection;


namespace UserServiceDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;
        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _db.QuerySingleOrDefaultAsync<User>("SELECT * FROM \"Users\" WHERE userId = @userId", new { userId });
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _db.QuerySingleOrDefaultAsync<User>("SELECT * FROM \"Users\" WHERE email = @email", new { email });
        }

        public async Task<User?> GetUserByNickNameAsync(string nickname)
        {
            return await _db.QuerySingleOrDefaultAsync<User>("SELECT * FROM \"Users\" WHERE nickname = @nickname", new { nickname });
        }


        public async Task<int> GetUserStatusByIdAsync(int userId)
        {
            return await _db.QuerySingleOrDefaultAsync<int>("SELECT \"status\" FROM \"Users\" WHERE userId = @userId", new { userId });
        }


        public async Task<int> SetUserAsync(User user)
        {
            string sqlQuery = "INSERT INTO \"Users\" (nickname, hashedPassword, salt, firstname, lastname, birthday, profileDescription, imgUrl, email, status)" +
                "VALUES (@NickName, @HashedPassword, @Salt, @FirstName, @LastName, @Birthday, @ProfileDescription, @ImgUrl, @Email, 1) RETURNING userId";
            int userId = await _db.ExecuteScalarAsync<int>(sqlQuery, user);
            return userId;
        }

        public async Task<User?> UpdateFirstNameAsync(string nickname, string newFirstname)
        {
            string sqlQuery = "UPDATE \"Users\" SET firstname = @newFirstname WHERE nickname = @nickname RETURNING *";

            return await _db.QueryFirstOrDefaultAsync<User>(sqlQuery, new { newFirstname, nickname });
        }
        public async Task<User?> UpdateLastNameAsync(string nickname, string newLastname)
        {
            string sqlQuery = "UPDATE \"Users\" SET lastname = @newLastname WHERE nickname = @nickname RETURNING *";
            return await _db.QueryFirstOrDefaultAsync<User>(sqlQuery, new { newLastname, nickname });
        }

        public async Task<User?> UpdateNickNameAsync(string nickname, string newNickname)
        {
            string sqlQuery = "UPDATE \"Users\" SET nickname = @newNickname WHERE nickname = @nickname RETURNING *";
            return await _db.QueryFirstOrDefaultAsync<User>(sqlQuery, new { newNickname, nickname });
        }

        public async Task<User?> UpdateProfileDescriptionAsync(string nickname, string? newProfileDescription)
        {
            string sqlQuery = "UPDATE \"Users\" SET profileDescription = @newProfileDescription WHERE nickname = @nickname RETURNING *";
            return await _db.QueryFirstOrDefaultAsync<User>(sqlQuery, new { newProfileDescription, nickname });
        }

        public async Task<User?> UpdateUserStatusAsync(int userId, int status)
        {
            string sqlQuery = "UPDATE \"Users\" SET status = @status WHERE userId = @userId RETURNING *";
            return await _db.QueryFirstOrDefaultAsync<User>(sqlQuery, new { status, userId });
        }

        public async Task<User?> UpdatePasswordAsync(string email, string hashedPassword, string salt)
        {
            string sqlQuery = "UPDATE \"Users\" SET hashedPassword = @hashedPassword, salt = @salt WHERE email = @email RETURNING *";
            return await _db.QueryFirstOrDefaultAsync<User>(sqlQuery, new { hashedPassword, salt, email }); ;
        }

    }
}
//type C:/Users/79832/source/repos/private_key.pem | gh secret set PRIVATE_KEY --stdin
//type public.pem | gh secret set PUBLIC_KEY
