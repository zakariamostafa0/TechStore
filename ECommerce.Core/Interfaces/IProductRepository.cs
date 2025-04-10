using ECommerce.Core.DTO.Product;
using ECommerce.Core.Entities.Product;
using ECommerce.Core.Sharing;

namespace ECommerce.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParam productParam);
        Task<bool> AddAsync(AddProductDTO productDTO);
        Task<bool> UpdateAsync(UpdateProductDTO productDTO);
        Task DeleteAsync(Product product);
    }
}
