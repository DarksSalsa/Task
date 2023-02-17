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
        }
    }
}
