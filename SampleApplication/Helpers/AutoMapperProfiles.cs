using AutoMapper;
using SampleApplication.DTOs;
using SampleApplication.Entities;
using SampleApplication.Extensions;
using System.Linq;

namespace SampleApplication.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(destination => destination.PhotoUrl, options => options.MapFrom(src => 
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(destination => destination.Age, options => options.MapFrom(src => 
                    src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoDto>();

            CreateMap<MemberUpdateDto, AppUser>();
        }
    }
}