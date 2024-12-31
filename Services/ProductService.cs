using Entities.DTO_s;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services.Interfaces;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly RepositoryContext _repositoryContext;

        public ProductService(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _repositoryContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _repositoryContext.Products.FindAsync(id);
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            using (var memoryStream = new MemoryStream())
            {
                await productDto.Photo.CopyToAsync(memoryStream);
                var photoBytes = memoryStream.ToArray();

                var product = new Product
                {
                    Name = productDto.Name,
                    Stock = productDto.Stock,
                    Price = productDto.Price,
                    Category = productDto.Category,
                    Photo = photoBytes 
                };

                await _repositoryContext.Products.AddAsync(product);
                await _repositoryContext.SaveChangesAsync();
            }
        }

        public async Task UpdateProductAsync(int id, ProductDto productDto)
        {
            var product = await _repositoryContext.Products.FindAsync(id);
            if (product != null)
            {
                product.Name = productDto.Name;
                product.Stock = productDto.Stock;
                product.Price = productDto.Price;
                product.Category = productDto.Category;

                if (productDto.Photo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await productDto.Photo.CopyToAsync(memoryStream);
                        product.Photo = memoryStream.ToArray();
                    }
                }

                _repositoryContext.Products.Update(product);
                await _repositoryContext.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repositoryContext.Products.FindAsync(id);
            if (product != null)
            {
                _repositoryContext.Products.Remove(product);
                await _repositoryContext.SaveChangesAsync();
            }
        }
    }
}
