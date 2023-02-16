namespace Base.Data
{
    public class PaginatedItem<T>
    {
        public long Count { get; set; }
        public IEnumerable<T> Content { get; set; }
    }
}
