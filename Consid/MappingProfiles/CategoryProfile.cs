using System;
using AutoMapper;
using Consid.Models;
using Consid.ViewModels;

namespace Consid.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {

            CreateMap<Category, CategoryViewModel>()
                .ReverseMap();
        }      
    }
}
