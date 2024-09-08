using AutoMapper;
using UserServiceDAL.Model.DTOs;
using UserServiceDAL.Model.Email;

namespace UserServiceDAL.MappingProfiles

{
    public class EmailVerificationProfile: Profile
    {
        public EmailVerificationProfile()
        { 
            CreateMap<EmailVerificationModel, UserEmailDTO>();
            CreateMap<UserEmailDTO, EmailVerificationModel>();
        }
    }
}
