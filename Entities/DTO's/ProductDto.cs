using Microsoft.AspNetCore.Http;

namespace Entities.DTO_s
{
    public class ProductDto
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public IFormFile Photo { get; set; } // Fotoğraf dosyasını alacak
    }
}
