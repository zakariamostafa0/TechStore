namespace ECommerce.Core.DTO
{
    public record UpdateProductDTO : AddProductDTO
    {
        public int Id { get; set; }
    }
}
