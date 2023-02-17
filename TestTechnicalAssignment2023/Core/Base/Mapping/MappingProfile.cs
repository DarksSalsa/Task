using AutoMapper;
using Base.Entities;
using Base.Models.Dtos;

namespace Base.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<Brand, BrandDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<OrderItem, OrderItemDto>().ForMember("ProductName", o => o.MapFrom(m => m.Product.ProductName));
            CreateMap<Product, ProductDto>();
            CreateMap<Staff, StaffDto>();
            CreateMap<Stock, StockDto>();
            CreateMap<Store, StoreDto>();
        }
    }
}
