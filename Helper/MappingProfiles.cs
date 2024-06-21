using AutoMapper;
using Restaurant_Management.Dto;
using Restaurant_Management.Models;

namespace Restaurant_Management.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Restaurant, RestaurantDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
