using System;
using AutoMapper;
using Consid.Models;
using Consid.ViewModels;
using Microsoft.CodeAnalysis;

namespace Consid.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.ManagerFirstName, opt => opt.MapFrom(src => src.Manager.FirstName))
                .ForMember(dest => dest.ManagerLastName, opt => opt.MapFrom(src => src.Manager.LastName))
                .ReverseMap();
        }
    }
}
