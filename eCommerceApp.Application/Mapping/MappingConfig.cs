using AutoMapper;
using eCommerceApp.Application.Dtos.Category;
using eCommerceApp.Application.Dtos.Identity;
using eCommerceApp.Application.Dtos.Product;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Entities.Identity;

namespace eCommerceApp.Application.Mapping;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<CreateProduct, Product>();
        CreateMap<UpdateProduct, Product>();
        CreateMap<Product, ProductResponse>();

        CreateMap<CreateCategory, Category>();
        CreateMap<UpdateCategory, Category>();
        CreateMap<Category, CategoryResponse>();

        CreateMap<CreateUser, AppUser>();
        CreateMap<LoginUser, AppUser>();
    }
}
