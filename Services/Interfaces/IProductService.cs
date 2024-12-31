using Entities.DTO_s;
using Entities.Models;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(); // Tüm ürünleri getirme
        Task<Product> GetProductByIdAsync(int id); // Ürün id'ye göre getirme
        Task AddProductAsync(ProductDto productDto); // Ürün ekleme
        Task UpdateProductAsync(int id, ProductDto productDto); // Ürün güncelleme
        Task DeleteProductAsync(int id); // Ürün silme
    }
}
