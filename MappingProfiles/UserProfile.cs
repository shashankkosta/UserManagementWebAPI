using AutoMapper;
using UserManagement.Models;

namespace UserManagement.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Source -> Target
            CreateMap<User, UserGet>();
            CreateMap<UserPost, User>();
            CreateMap<User, UserPost>();
        }
    }
}