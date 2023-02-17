using Base.Entities;

namespace Base.Models.Dtos
{
    public class StockDto
    {
        public int StoreId { get; set; }

        public int ProductId { get; set; }

        public int? Quantity { get; set; }
    }
}
