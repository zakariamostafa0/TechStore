using ECommerce.Api.Helper;
using ECommerce.Core.DTO.Product;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController(
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string sort, int? categoryId)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository
                    .GetAllAsync(sort, categoryId);

                if (products == null || !products.Any())
                    return Ok(new ResponseAPI(200, "No products found."));
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository
                    .GetByIdAsync(id, p => p.Category, p => p.Photos);
                if (product == null)
                    return NotFound(new ResponseAPI(404, $"Product with ID {id} not found."));
                var result = _mapper.Map<ProductDTO>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpPost()]
        public async Task<IActionResult> AddProduct(AddProductDTO productDto)
        {
            try
            {
                await _unitOfWork.ProductRepository.AddAsync(productDto);
                return Ok(new ResponseAPI(200, "Item has been added!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpPut()]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO productDto)
        {
            try
            {
                await _unitOfWork.ProductRepository.UpdateAsync(productDto);
                return Ok(new ResponseAPI(200, "Item has been updated!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository
                    .GetByIdAsync(id, p => p.Category, p => p.Photos);
                if (product == null)
                    return NotFound(new ResponseAPI(404, $"Product with ID {id} not found."));

                await _unitOfWork.ProductRepository.DeleteAsync(product);
                return Ok(new ResponseAPI(200, "Item has been deleted!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
    }
}
