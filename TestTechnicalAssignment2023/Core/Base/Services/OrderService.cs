using AutoMapper;
using Base.Entities;
using Base.Models.Dtos;
using Base.Models.Responses;
using Base.Repositories.Interfaces;
using Base.Services.Interfaces;
using Infrastructure.Core.Services;
using Infrastructure.Core.Services.Interfaces;

namespace Base.Services
{
    public class OrderService : TransactionService<BikeStoresContext> ,IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IDbContextWrapper<BikeStoresContext> context,
            ILogger<OrderService> logger,
            IOrderRepository orderRepository,
            IMapper mapper)
            : base(context, logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(int customerId, byte orderStatus, DateTime orderDate, DateTime requiredDate, DateTime shippedDate, int storeId, int staffId)
        {
            return await ExecuteSafeAsync(async () => await _orderRepository.CreateOrderAsync(customerId, orderStatus, orderDate, requiredDate, shippedDate, storeId, staffId));
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _orderRepository.GetOrderByIdAsync(id);
                return _mapper.Map<OrderDto>(result);
            });
        }

        public async Task<PaginatedItemResponse<OrderDto>> GetOrdersByPageAsync(int pageIndex, int pageSize)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _orderRepository.GetOrdersByPageAsync(pageIndex, pageSize);
                return new PaginatedItemResponse<OrderDto>()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Count = result.Count,
                    Data = result.Content.Select(s => _mapper.Map<OrderDto>(s))
                };
            });
        }

        public async Task<bool> RemoveOrderAsync(int id)
        {
            return await ExecuteSafeAsync(async () => await _orderRepository.RemoveOrderAsync(id));
        }
    }
}
