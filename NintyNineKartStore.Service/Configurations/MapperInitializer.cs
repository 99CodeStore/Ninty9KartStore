using AutoMapper;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Service.Models;
using System.Collections.Generic;

namespace NintyNineKartStore.Service.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        { 
            CreateMap<PagedRequest, PagedRequestInput>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();

            CreateMap<CoverType, CoverTypeDto>().ReverseMap();
            CreateMap<CoverType, CreateCoverTypeDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();

            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Company, CreateCompanyDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyDto>().ReverseMap();

        }
    }
}
