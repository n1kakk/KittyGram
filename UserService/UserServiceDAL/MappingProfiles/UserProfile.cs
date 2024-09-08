using AutoMapper;
using UserServiceDAL.Model.DTOs;
using UserServiceDAL.Model.User;

namespace UserServiceDAL.MappingProfiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();


            CreateMap<User, UserEmailDTO>();
            CreateMap<UserEmailDTO, User>();

            CreateMap<User, UserUpdateDTO>();
            CreateMap<UserUpdateDTO, User>();
        }
    }
}
