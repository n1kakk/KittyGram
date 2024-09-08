namespace UserServiceDAL.Model.DTOs
{
    public class UserUpdateDTO
    {
        public string NickName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileDescription { get; set; }
    }
}
