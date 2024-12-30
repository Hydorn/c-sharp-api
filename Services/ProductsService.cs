using Microsoft.EntityFrameworkCore;

namespace EntityFW.Services
{
    public class ProductsService
    {
        private readonly EntityFWContext _context;
        public ProductsService(EntityFWContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product?> GetProductById(Guid id)
        {

            return await this._context.Products.FindAsync(id);
        }

        public void EditProduct(Guid id, Product product)
        {
            this._context.Entry(product).State = EntityState.Modified;
            _context.SaveChangesAsync();
        }

        public async Task<bool> CreateProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //_logger.LogError(ex, "An error occurred while creating the product.");
                return false;
            }
        }

        public async Task<Product?> DeleteProduct(Guid id)
        {
            var product = await this._context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            else
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return product;
            }
        }
        public bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
