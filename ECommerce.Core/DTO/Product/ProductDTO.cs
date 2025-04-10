namespace ECommerce.Core.DTO.Product
{
    public record ProductDTO
    {
        public string Name { get; init; }
        public string? Description { get; init; }
        public decimal NewPrice { get; init; }
        public decimal OldPrice { get; init; }
        public List<PhotoDTO>? Photos { get; init; }
        public string? CategoryName { get; init; }
    }
    public record PhotoDTO
    {
        public string ImageName { get; init; }
        public int ProductId { get; init; }
    }
}
