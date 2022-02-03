using AutoMapper;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Service.Models;

namespace NintyNineKartStore.Service.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();

            CreateMap<CoverType, CoverTypeDto>().ReverseMap();
            CreateMap<CoverType, CreateCoverTypeDto>().ReverseMap();
        }
    }
}
