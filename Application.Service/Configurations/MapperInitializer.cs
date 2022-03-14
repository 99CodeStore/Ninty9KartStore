using AutoMapper;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Service.Models;
using System.Collections.Generic;

namespace NsdcTraingPartnerHub.Service.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<PagedRequest, PagedRequestInput>().ReverseMap();

            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CreateCourseDto>().ReverseMap();

            CreateMap<SponsoringBody, SponsoringBodyDto>().ReverseMap();
            CreateMap<SponsoringBody, CreateSponsoringBodyDto>().ReverseMap();

            CreateMap<TrainingPartner, TrainingPartnerDto>().ReverseMap();
            CreateMap<TrainingPartner, CreateTrainingPartnerDto>().ReverseMap();

            CreateMap<CenterAuthorityMember, CenterAuthorityMemberDto>().ReverseMap();
            CreateMap<CenterAuthorityMember, CreateCenterAuthorityMemberDto>().ReverseMap();
            CreateMap<CenterAuthorityMember, UpdateCenterAuthorityMemberDto>().ReverseMap();

            //CreateMap<ShoppingCartDto, ShoppingCart>().ForMember(dest =>
            //dest.Product,
            //opt => opt.MapFrom(src => src.ProductDto)).ReverseMap();

            //CreateMap<ShoppingCart, CreateShoppingCartDto>().ReverseMap();
            //CreateMap<ShoppingCart, UpdateShoppingCartDto>().ReverseMap();

            //CreateMap<OrderHeader, CreateOrderHeaderDto> ().ReverseMap();
            //CreateMap<OrderHeader, OrderHeaderDto> ().ReverseMap();

            //CreateMap<OrderDetail, OrderDetailDto> ().ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserDto> ().ReverseMap();

            CreateMap<TrainingCenter, CreateTrainingCenterDto>().ReverseMap();
            CreateMap<TrainingCenter, TrainingCenterDto>().ReverseMap();
        }
    }
}
