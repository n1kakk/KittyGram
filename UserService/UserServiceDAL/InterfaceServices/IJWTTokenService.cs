using UserServiceDAL.Model.Tokens;
using UserServiceDAL.Model.User;

namespace UserServiceDAL.InterfaceServices
{
    public interface IJWTTokenService
    {
        public Task<TokenModel?> GetRefreshTokenAsync(TokenModel tokenModel);

        public Task RevokeRefreshTokensAsync(string token);

        public Task<TokenModel> GenerateJwtTokensAsync(User user);

        public User ValidateJwtToken(string token);
    }
}
