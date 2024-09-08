using Dapper;
using System.Data;
using UserServiceDAL.InterfaceRepository;
using UserServiceDAL.Model.Email;

namespace UserServiceDAL.Repositories
{
    public class EmailVerificationRepository : IEmailVerificationRepository
    {
        private readonly IDbConnection _db;
        public EmailVerificationRepository(IDbConnection db)
        {
            _db = db;
        }
        public async Task<EmailVerificationModel?> GetEmailVerificationByEmailAsync(string email)
        {

            string query = "SELECT * FROM \"EmailVerification\" WHERE email = @email";
            return await _db.QuerySingleOrDefaultAsync<EmailVerificationModel>(query, new { email });

        }

        public async Task<EmailVerificationModel?> GetEmailVerificationByTokenAsync(string verificationToken)
        {
            string query = "SELECT * FROM \"EmailVerification\" WHERE verificationToken = @verificationToken";
            return await _db.QuerySingleOrDefaultAsync<EmailVerificationModel>(query, new { verificationToken });
        }

        public async Task<EmailVerificationModel?> GetEmailVerificationByUserIdAsync(int userId)
        {
            string query = "SELECT * FROM \"EmailVerification\" WHERE userId = @userId";
            return await _db.QuerySingleOrDefaultAsync<EmailVerificationModel>(query, new { userId });
        }

        public async Task InvalidateTokensByTypeAsync(int userId, int tokenType)
        {
            string query = "UPDATE \"EmailVerification\"SET tokenType = 3 WHERE userId = @userId AND tokenType = @tokenType";
            await _db.ExecuteAsync(query, new { userId, tokenType });
        }

        public async Task SetVerificationTokenAsync(EmailVerificationModel emailVerificationModel)
        {
            string sqlQuery = "INSERT INTO \"EmailVerification\" (email, userId, verificationToken, tokenCreationDate, tokenType)" +
                                "VALUES (@Email,@UserId, @VerificationToken, @TokenCreationDate, @TokenType)";
            await _db.ExecuteAsync(sqlQuery, emailVerificationModel);
        }


        public async Task<EmailVerificationModel?> UpdateTokenIsUsedAsync(string verificationToken, bool isUsed)
        {
            string query = "UPDATE \"EmailVerification\" SET isUsed = @isUsed WHERE verificationToken = @verificationToken RETURNING *";
            return await _db.QueryFirstOrDefaultAsync<EmailVerificationModel>(query, new { isUsed, verificationToken });
        }

        public async Task<EmailVerificationModel?> UpdateVerificationTokenAsync(string email, string verificationToken, DateTime tokenCreationDate)
        {
            string sqlQuery = "UPDATE \"EmailVerification\" SET verificationToken = @verificationToken, tokenCreationDate = @tokenCreationDate, isUsed = false WHERE email = @email RETURNING *";
            return await _db.QueryFirstOrDefaultAsync<EmailVerificationModel>(sqlQuery, new { verificationToken, tokenCreationDate, email });
        }

    }
}
