using Base.Models.Dtos;

namespace Base.Models.Requests
{
    public class CreateOrderRequest
    {
        public int? CustomerId { get; set; }

        public byte OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }
        public int StoreId { get; set; }

        public int StaffId { get; set; }
    }
}
