using AutoMapper;
using DistanceWebApi.Models;
using DistanceWebApi.ViewModels.Users;

namespace DistanceWebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<RegisterDto, User>();
        }
    }
}