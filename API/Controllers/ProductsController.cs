using Core.Entities;
using Infrastucture.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly StroreContext _context;
        public ProductsController( StroreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> getProducts()
        {
            return await _context.products.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> getProduct(int id)
        {
            var product = await _context.products.FindAsync(id);

            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExist(id)) return BadRequest(" Can't fount this product");

            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.products.FindAsync(id);

            if(product == null) return NotFound();

            _context.products.Remove(product);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        public Boolean ProductExist(int id)
        {
            return _context.products.Any(x => x.Id == id);
        }
    }
}
