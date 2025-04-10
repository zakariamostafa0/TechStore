namespace ECommerce.Core.DTO.Product
{
    public record UpdateProductDTO : AddProductDTO
    {
        public int Id { get; set; }
    }
}
