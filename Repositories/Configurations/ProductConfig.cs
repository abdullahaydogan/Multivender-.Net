using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
             new Product { Id = 1, Name = "Tv", Price = 12, Stock = 123, Category = "Electronic", Photo = new byte[] { /* Fotoğraf byte dizisi */ } },
             new Product { Id = 2, Name = "Phone", Price = 123, Stock = 1233, Category = "Electronic", Photo = new byte[] { /* Fotoğraf byte dizisi */ } },
             new Product { Id = 3, Name = "Laptop", Price = 999, Stock = 50, Category = "Electronic", Photo = new byte[] { /* Fotoğraf byte dizisi */ } }

         );
        }
    }
}
