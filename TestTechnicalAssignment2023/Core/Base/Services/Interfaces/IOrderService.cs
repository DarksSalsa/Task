using Base.Data;
using Base.Models;
using Base.Models.Responses;

namespace Base.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(
            int customerId,
            short orderStatus,
            DateOnly orderDate,
            DateOnly requiredDate,
            DateOnly shippedDate,
            int storeId,
            int staffId);
        Task<OrderModel?> GetOrderByIdAsync(int id);
        Task<PaginatedItemResponse<OrderModel>> GetOrdersByPageAsync(int pageIndex, int pageSize);
        Task<bool> RemoveOrderAsync(int id);
    }
}
