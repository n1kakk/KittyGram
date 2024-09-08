namespace UserServiceDAL.Model.DTOs
{
    public class UserEmailDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string VerificationToken { get; set; }
    }
}
