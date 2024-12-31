using Microsoft.AspNetCore.Http;

namespace Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public byte[]? Photo { get; set; } // Fotoğrafı byte dizisi olarak sakla

    }
}
