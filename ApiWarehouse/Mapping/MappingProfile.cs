using AutoMapper;
using WareHouse.DTO.CategoryDto;
using WareHouse.DTO.ProductDto;
using WareHouse.DTO.StorageDto;
using WareHouse.Models;

namespace WareHouse.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, GetProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            CreateMap<Storage, StorageDto>().ReverseMap();
            CreateMap<Storage, GetStorageDto>().ReverseMap();
            CreateMap<Storage, UpdateStorageDto>().ReverseMap();

        }
    }
}
