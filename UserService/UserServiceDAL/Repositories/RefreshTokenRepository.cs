﻿using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using UserServiceDAL.Helpers;
using UserServiceDAL.InterfaceRepository;
using UserServiceDAL.Model.Tokens;

namespace UserServiceDAL.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly StackExchange.Redis.IDatabase _database;
        private readonly string keyPrefix = "refreshToken:";
        private readonly string keyPrefixAccessToken = "accessToken: ";
        private readonly AppSettings _appSettings;
        public RefreshTokenRepository(ConnectionMultiplexer redis, IOptions<AppSettings> appSettings)
        {
            _redis = redis;
            _database = redis.GetDatabase();
            _appSettings = appSettings.Value;
        }

        public async Task<RefreshToken> DeleteRefreshTokenAsync(string refreshTokenId)
        {
            var key = keyPrefix + refreshTokenId;

            bool refreshTokenExists = await _database.KeyExistsAsync(key);
            if (!refreshTokenExists) return null;
            var data = await _database.StringGetAsync(key);

            var refreshToken = JsonSerializer.Deserialize<RefreshToken>(data);
            await _database.KeyDeleteAsync(key);

            return refreshToken;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshTokenId)
        {
            var key = keyPrefix + refreshTokenId;
            bool refreshTokenExists = await _database.KeyExistsAsync(key);
            if (!refreshTokenExists) return null;

            var data = await _database.StringGetAsync(key);

            var refreshToken = JsonSerializer.Deserialize<RefreshToken>(data);

            return refreshToken;
        }

        public async Task<RefreshToken> SetRefreshTokenAsync(RefreshToken refreshToken)
        {
            var key = keyPrefix + refreshToken.refreshToken;
            bool refreshTokenExists = await _database.KeyExistsAsync(key);
            if (!refreshTokenExists) await _database.StringSetAsync(key, JsonSerializer.Serialize(refreshToken), TimeSpan.FromDays(_appSettings.RefreshTokenValidityInDays));

            return refreshToken;
        }
    }
}
