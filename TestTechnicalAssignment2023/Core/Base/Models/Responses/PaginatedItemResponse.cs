using System.ComponentModel.DataAnnotations;

namespace Base.Models.Responses
{
    public class PaginatedItemResponse<T>
    {
        [Required]
        public int PageIndex { get; set; }
        [Required]
        public int PageSize { get; set; }
        public long Count { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}
