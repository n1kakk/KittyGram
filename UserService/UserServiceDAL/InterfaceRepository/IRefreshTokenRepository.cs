using UserServiceDAL.Model.Tokens;

namespace UserServiceDAL.InterfaceRepository
{
    public interface IRefreshTokenRepository
    {
        public Task<RefreshToken> GetRefreshTokenAsync(string refreshTokenId);
        public Task<RefreshToken> SetRefreshTokenAsync(RefreshToken refreshToken);
        public Task<RefreshToken> DeleteRefreshTokenAsync(string refreshTokenId);
    }
}
