using System;
using AutoMapper;
using Consid.Models;
using Consid.ViewModels;

namespace Consid.MappingProfiles
{
    public class LibraryItemProfile : Profile
    {
        public LibraryItemProfile()
        {
            CreateMap<LibraryItem, LibraryItemViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.IsCheckedOut, opt => opt.MapFrom(src => src.Borrower != null))
                .ReverseMap()
                .ForPath(s => s.Category.CategoryName, opt => opt.Ignore())
                .ForPath(s => s.BorrowDate, opt => opt.MapFrom(src => src.IsCheckedOut ? src.BorrowDate : null))
                .ForPath(s => s.Borrower, opt => opt.MapFrom(src => src.IsCheckedOut ? src.Borrower : null));

            CreateMap<LibraryItem, LibraryItemViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.IsCheckedOut, opt => opt.MapFrom(src => src.Borrower != null))
                .ReverseMap()
                .ForPath(s => s.Category.CategoryName, opt => opt.Ignore())
                .ForPath(s => s.BorrowDate, opt => opt.MapFrom(src => src.IsCheckedOut ? src.BorrowDate : null))
                .ForPath(s => s.Borrower, opt => opt.MapFrom(src => src.IsCheckedOut ? src.Borrower : null));
        }
    }
}
