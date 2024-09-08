
namespace UserServiceDAL.Model.User
{
    public class User
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public int Status { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ProfileDescription { get; set; }
        public string? ImgUrl { get; set; }


    }
}
