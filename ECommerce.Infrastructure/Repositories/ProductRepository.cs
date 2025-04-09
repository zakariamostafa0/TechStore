using AutoMapper;
using ECommerce.Core.DTO;
using ECommerce.Core.Entities.Product;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        public ProductRepository(AppDbContext context,
            IMapper mapper,
            IImageManagementService imageManagementService) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO is null)
                return false;

            var product = _mapper.Map<Product>(productDTO);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var images = await _imageManagementService.UploadImagesAsync(productDTO.Photo, productDTO.Name);

            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var productPhoto = new Photo
                    {
                        ImageName = image,
                        ProductId = product.Id
                    };
                    await _context.Photos.AddAsync(productPhoto);
                }
                await _context.SaveChangesAsync();
            }
            else return false;
            return true;
        }


        public async Task<bool> UpdateAsync(UpdateProductDTO productDTO)
        {
            if (productDTO is null)
                return false;

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == productDTO.Id);

            if (product is null)
                return false;
            _mapper.Map(productDTO, product);

            var findImages = await _context.Photos
                .Where(p => p.ProductId == product.Id)
                .ToListAsync();

            if (findImages != null && findImages.Count > 0)
            {
                foreach (var image in findImages)
                {
                    await _imageManagementService.DeleteImageAsync(image.ImageName);
                }
                _context.Photos.RemoveRange(findImages);
            }
            var imagesPath = await _imageManagementService.UploadImagesAsync(productDTO.Photo, productDTO.Name);
            if (imagesPath != null && imagesPath.Count > 0)
            {
                var photos = imagesPath.Select(image => new Photo
                {
                    ImageName = image,
                    ProductId = product.Id
                }).ToList();
                await _context.Photos.AddRangeAsync(photos);
                await _context.SaveChangesAsync();
            }
            else
                return false;

            return true;
        }

        public async Task DeleteAsync(Product product)
        {
            var photo = await _context.Photos
                .Where(p => p.ProductId == product.Id)
                .ToListAsync();

            if (photo != null && photo.Count > 0)
            {
                foreach (var image in photo)
                {
                    await _imageManagementService.DeleteImageAsync(image.ImageName);
                }
                _context.Products.RemoveRange(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
