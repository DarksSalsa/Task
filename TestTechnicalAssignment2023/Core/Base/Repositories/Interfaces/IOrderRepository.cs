using Base.Data;
using Base.Entities;

namespace Base.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(
            int? customerId,
            byte orderStatus,
            DateTime orderDate,
            DateTime requiredDate,
            DateTime? shippedDate,
            int storeId,
            int staffId);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<PaginatedItem<Order>> GetOrdersByPageAsync(int pageIndex, int pageSize);
        Task<bool> RemoveOrderAsync(int id);
    }
}
