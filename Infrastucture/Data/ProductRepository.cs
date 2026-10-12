using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Data
{
    public class ProductRepository(StroreContext context) : IProductRepository
    {
        public void AddProduct(Product product)
        {   
            context.products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            context.products.Remove(product);
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.products.Select(x=>x.Brand)
                .Distinct()
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand,string? type, string? sort)
        {
            var query = context.products.AsQueryable();

            if(!string.IsNullOrWhiteSpace(brand))
                query = query.Where(x => x.Brand == brand);

            if(!string.IsNullOrWhiteSpace(type))
                query = query.Where(x=>x.Type == type);

            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x=> x.Name)
            };

            return query.ToList();
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await context.products.Select(x=>x.Type)
                .Distinct()
                .ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return context.products.Any( x => x.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
        }
    }
}
