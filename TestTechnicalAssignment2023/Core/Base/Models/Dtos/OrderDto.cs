using Base.Entities;

namespace Base.Models.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public byte OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public CustomerDto? Customer { get; set; } 

        public ICollection<OrderItemDto> OrderItems { get; } = new List<OrderItemDto>();

        public StaffDto Staff { get; set; } = null!;

        public StoreDto Store { get; set; } = null!;
    }
}
