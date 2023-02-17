using Base.Data;
using Base.Entities;
using Base.Repositories.Interfaces;
using Infrastructure.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Base.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BikeStoresContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(IDbContextWrapper<BikeStoresContext> context, ILogger<OrderRepository> logger)
        {
            _context = context.DbContext;
            _logger = logger;
        }

        public async Task<int> CreateOrderAsync(
            int customerId,
            byte orderStatus,
            DateTime orderDate,
            DateTime requiredDate,
            DateTime shippedDate,
            int storeId,
            int staffId)
        {
            var report = await _context.AddAsync(
                new Order()
                {
                    CustomerId = customerId,
                    OrderStatus = orderStatus,
                    OrderDate = orderDate,
                    RequiredDate = requiredDate,
                    ShippedDate = shippedDate,
                    StoreId = storeId,
                    StaffId = staffId
                });
            await _context.SaveChangesAsync();
            return report.Entity.OrderId;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            var item = await _context.Orders
                .Include(i => i.Customer)
                .Include(i => i.OrderItems)
                .Include(i => i.OrderItems).ThenInclude(i => i.Product)
                .Include(i => i.Store)
                .Include(i => i.Staff).Where(w => w.OrderId == id).FirstOrDefaultAsync();
            return item;
        }

        public async Task<PaginatedItem<Order>> GetOrdersByPageAsync(int pageIndex, int pageSize)
        {
            var count = await _context.Orders.LongCountAsync();
            var content = await _context.Orders
                .Include(i => i.Customer)
                .Include(i => i.OrderItems)
                .Include(i => i.OrderItems).ThenInclude(i => i.Product)
                .Include(i => i.Store)
                .Include(i => i.Staff)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedItem<Order>() { Content = content, Count = count };
        }

        public async Task<bool> RemoveOrderAsync(int id)
        {
            var item = await GetOrderByIdAsync(id);
            if (item == null)
            {
                return false;
            }

            var result = _context.Orders.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
