using Base.Data;
using Base.Models.Dtos;
using Base.Models.Responses;

namespace Base.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int?> CreateOrderAsync(
            int? customerId,
            byte orderStatus,
            DateTime orderDate,
            DateTime requiredDate,
            DateTime? shippedDate,
            int storeId,
            int staffId);
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<PaginatedItemResponse<OrderDto>> GetOrdersByPageAsync(int pageIndex, int pageSize);
        Task<bool> RemoveOrderAsync(int id);
    }
}
