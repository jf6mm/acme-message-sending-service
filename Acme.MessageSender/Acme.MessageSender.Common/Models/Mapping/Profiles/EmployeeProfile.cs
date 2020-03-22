using Acme.MessageSender.Common.Helpers;
using Acme.MessageSender.Common.Models.Dto;
using AutoMapper;

namespace Acme.MessageSender.Common.Models.Mapping.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeDto, Employee>()
               .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateTimeConverter.FromIsoDate(src.DateOfBirth)))
               .ForMember(dest => dest.EmploymentEndDate, opt => opt.MapFrom(src => DateTimeConverter.FromIsoDate(src.EmploymentEndDate)))
               .ForMember(dest => dest.EmploymentStartDate, opt => opt.MapFrom(src => DateTimeConverter.FromIsoDate(src.EmploymentStartDate)))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
