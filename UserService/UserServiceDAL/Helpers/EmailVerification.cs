namespace UserServiceDAL.Security
{
    public static class EmailVerification
    {
        public static bool VerifyToken(string token, string dbToken)
        {
            return (token == dbToken);
        }
        public static bool VerifyTokenCreationDate(DateTime tokenCreationDate)
        {
            DateTime expirationTime = tokenCreationDate.AddHours(12);
            //DateTime expirationTime = tokenCreationDate.AddMinutes(1);
            DateTime currentTime = DateTime.Now;
            return currentTime <= expirationTime;
        }
        public static string GenerateVerificationLink(int userId, string token, string verificationReason)
        {
            string verificationLink = $"https://localhost:7069/{verificationReason}?userId={userId}&token={token}";

            return verificationLink;
        }
        public static string GenerateVerificationToken()
        {
            string token = Guid.NewGuid().ToString();
            return token;
        }
    }
}
