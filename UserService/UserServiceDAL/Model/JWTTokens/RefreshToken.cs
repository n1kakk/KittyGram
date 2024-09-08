namespace UserServiceDAL.Model.Tokens
{
    public class RefreshToken
    {
        public string NickName { get; set; }
        public bool IsUsed { get; set; }
        public DateTime Expiration { get; set; }
        public string refreshToken { get; set; }
    }
}
