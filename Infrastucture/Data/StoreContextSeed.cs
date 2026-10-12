using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastucture.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StroreContext context)
        {
            if (!context.products.Any())
            {
                var productData = await File.ReadAllTextAsync("../Infrastucture/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products == null) return;

                context.products.AddRange(products);

                await context.SaveChangesAsync();
            }
        }
    }
}
