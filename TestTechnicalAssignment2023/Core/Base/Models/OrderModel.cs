using Base.Entities;

namespace Base.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }

        public short OrderStatus { get; set; }

        public DateOnly OrderDate { get; set; }

        public DateOnly RequiredDate { get; set; }

        public DateOnly? ShippedDate { get; set; }

        public virtual Customer? Customer { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

        public virtual Staff Staff { get; set; } = null!;

        public virtual Store Store { get; set; } = null!;
    }
}
