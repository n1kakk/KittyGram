﻿namespace UserServiceDAL.Helpers
{
    public class AppSettings
    {
        public string JWTSecret { get; set; }
        public string JWTRSAPrivateKey { get; set; }
        public string JWTRSAPublicKey { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
    }
}
