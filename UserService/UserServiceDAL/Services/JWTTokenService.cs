//using UserServiceDAL.InterfaceServices;
//using System.Security.Cryptography;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using UserServiceDAL.Helpers;
//using Microsoft.Extensions.Options;
//using UserServiceDAL.Model.Tokens;
//using UserServiceDAL.Model.User;
//using System.Text;
//using UserServiceDAL.InterfaceRepository;


//namespace UserServiceDAL.Services
//{
//    public class JWTTokenService : IJWTTokenService
//    {
//        private readonly AppSettings _appSettings;
//        private readonly IRefreshTokenRepository _refreshTokenRepository;
//        private readonly string public_key;
//        public JWTTokenService( IOptions<AppSettings> appSettings, IRefreshTokenRepository refreshTokenRepository)
//        {
//            _appSettings = appSettings.Value;
//            _refreshTokenRepository = refreshTokenRepository;
//            //public_key = Convert.FromBase64String(_appSettings.JWTRSAPublicKey);
//        }

//        public async Task<TokenModel> GenerateJwtTokensAsync(User user)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();

//            SigningCredentials signingCredentials = null;

//            if (!string.IsNullOrEmpty(_appSettings.JWTSecret) && string.IsNullOrEmpty(_appSettings.JWTRSAPrivateKeyFilePath))
//            {
//                var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);
//                signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
//            }

//            if (!string.IsNullOrEmpty(_appSettings.JWTRSAPrivateKeyFilePath) && File.Exists(_appSettings.JWTRSAPrivateKeyFilePath))
//            {
//                //var privateKey = Convert.FromBase64String(_appSettings.JWTRSAPrivateKeyFilePath);
//                var privateKey = File.ReadAllText(_appSettings.JWTRSAPrivateKeyFilePath);

//                if (!string.IsNullOrEmpty(privateKey))
//                {
//                    RSA rsa = RSA.Create();
//                    rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

//                    signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
//                    {
//                        CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
//                    };
//                }
//            }
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[] {
//                new Claim("NickName", user.NickName.ToString()),
//                new Claim("Email", user.Email.ToString())
//            }),
//                Expires = DateTime.UtcNow.AddMinutes(_appSettings.TokenValidityInMinutes),
//                SigningCredentials = signingCredentials
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            var refreshToken = await CreateRefreshToken(user.NickName);
//            return new TokenModel { Token = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
//        }

//        public async Task<TokenModel> GetRefreshTokenAsync(TokenModel tokenModel)
//        {
//            JwtSecurityToken jwtToken = CheckToken(tokenModel.Token, false);
//            var userModel = ExtractUserFromToken(jwtToken);
//            var checkResult = await CheckRefreshTokenAsync(userModel.NickName, tokenModel.RefreshToken);
//            var oldRefreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(tokenModel.RefreshToken);

//            if (checkResult == CheckRefreshTokenResult.Ok)
//            {
//                oldRefreshToken.IsUsed = true;
//                await _refreshTokenRepository.SetRefreshTokenAsync(oldRefreshToken);
//                return await GenerateTokenAsync(userModel.NickName);
//            }
//            throw new AppException($"Refresh token is invalid {checkResult}"); //?? условие инвалид рефреш токена

//        }

//        public Task RevokeRefreshTokensAsync(string token)
//        {
//            JwtSecurityToken jwtToken = CheckToken(token, true);
//            var userModel = ExtractUserFromToken(jwtToken);

//            InvalidateUserRefreshTokens(userModel.Email);

//            return Task.CompletedTask;
//        }
//        public async Task<TokenModel?> GenerateTokenAsync(string nickName)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            SigningCredentials signingCredentials = null;

//            var privateKey = File.ReadAllText(_appSettings.JWTRSAPrivateKeyFilePath);

//            if (!string.IsNullOrEmpty(privateKey))
//            {
//                RSA rsa = RSA.Create();
//                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

//                signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
//                {
//                    CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
//                };
//            }


//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[] {
//                new Claim(JwtRegisteredClaimNames.Name, nickName),
//            }),
//                Expires = DateTime.UtcNow.AddMinutes(_appSettings.TokenValidityInMinutes),
//                SigningCredentials = signingCredentials

//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            var refreshToken = await CreateRefreshToken(nickName);

//            return new TokenModel { Token = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
//        }

//        public User ValidateJwtToken(string token)
//        {
//            JwtSecurityToken jwtToken = CheckToken(token, true);

//            return ExtractUserFromToken(jwtToken);
//        }

//        private void InvalidateUserRefreshTokens(string userEmail)
//        {
//            //// Invalidate all user refreshTokens
//            //foreach (var item in refreshTokens)
//            //{
//            //    if (item.Value.Email == userEmail && !item.Value.IsUsed)
//            //        item.Value.IsUsed = true;
//            //}
//            throw new NotImplementedException();
//        }
//        private User ExtractUserFromToken(JwtSecurityToken jwtToken)
//        {
//            var email = jwtToken.Claims.First(x => x.Type == "Email").Value;
//            var nickName = jwtToken.Claims.First(x => x.Type == "NickName").Value;

//            return new User
//            {
//                NickName = nickName,
//                Email = email
//            };
//        }
//        private async Task<string> CreateRefreshToken(string nickName)
//        {
//            var id = Guid.NewGuid().ToString();

//            var refreshToken = new RefreshToken { NickName = nickName, IsUsed = false, Expiration = DateTime.UtcNow.AddDays(_appSettings.RefreshTokenValidityInDays), refreshToken = id };
//            await _refreshTokenRepository.SetRefreshTokenAsync(refreshToken);
//            return await Task.FromResult(id);
//        }

//        //Task ITokenService.RevokeRefreshTokenAsync(string token)
//        //{
//        //}

//        public Task<User> ValidateTokenAsync(string token)
//        {
//            JwtSecurityToken jwtToken = CheckToken(token, true);
//            var userModel = ExtractUserFromToken(jwtToken);

//            return Task.FromResult(userModel);
//        }

//        private JwtSecurityToken CheckToken(string token, bool validateLifetime)
//        {
//            try
//            {
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var privateKey = Environment.GetEnvironmentVariable("JWTTokenprivate_key");
//               // var privateKey = File.ReadAllText(_appSettings.JWTRSAPrivateKeyFilePath);
//                RSA rsa = RSA.Create();
//                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
//                var rsaKey = new RsaSecurityKey(rsa);

//                tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = rsaKey,
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ValidateLifetime = validateLifetime,
//                    ClockSkew = TimeSpan.Zero
//                }, out SecurityToken validatedToken);
//                return (JwtSecurityToken)validatedToken;
//            }
//            catch (Exception ex)
//            {
//                throw new AppException($"Token is invalid {ex.Message}");
//            }
//        }
//        private async Task<CheckRefreshTokenResult> CheckRefreshTokenAsync(string nickName, string refreshTokenId)
//        {
//            var refreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(refreshTokenId);

//            if (refreshToken == null) return CheckRefreshTokenResult.NotFound;
//            if (refreshToken.NickName != nickName) return CheckRefreshTokenResult.Incorrect;
//            if (refreshToken.IsUsed) return CheckRefreshTokenResult.IsUsed;
//            if (refreshToken.Expiration < DateTime.UtcNow) return CheckRefreshTokenResult.Expired;
//            return CheckRefreshTokenResult.Ok;
            
//        }

//        enum CheckRefreshTokenResult
//        {
//            Ok,
//            NotFound,
//            Expired,
//            IsUsed,
//            Incorrect
//        }

//    }
//}
