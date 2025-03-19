using AutoMapper;
using LandingAPI.DTO;
using LandingAPI.Models;

namespace LandingAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<News, NewsDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<Event, EventDTO>();
            CreateMap<Files, FilesDTO>();
        }
    }
}
