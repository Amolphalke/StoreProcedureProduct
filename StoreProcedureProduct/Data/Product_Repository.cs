using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreProcedureProduct.Models;

namespace StoreProcedureProduct.Data
{
    public class Product_Repository
    {
        private readonly ApplicationDbContext _context;
        public Product_Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.FromSqlRaw("EXEC spGetProducts").ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var parameter = new SqlParameter("@ProductId", id);
            var products = await _context.Products.FromSqlRaw("EXEC spGetProductById @ProductId", parameter).ToListAsync();
            return products.FirstOrDefault();
        }
        public async Task AddProductAsync(Product product)
        {
            var parameters = new[]
            {
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Price", product.Price)
            };
            await _context.Database.ExecuteSqlRawAsync("EXEC spAddProduct @Name, @Price", parameters);
        }
        public async Task UpdateProductAsync(Product product)
        {
            var parameters = new[]
            {
                new SqlParameter("@ProductId", product.ProductId),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Price", product.Price)
            };
            await _context.Database.ExecuteSqlRawAsync("EXEC spUpdateProduct @ProductId, @Name, @Price", parameters);
        }

        public async Task DeleteProductAsync(int id)
        {
            var parameter = new SqlParameter("@ProductId", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC spDeleteProduct @ProductId", parameter);
        }
    }
}
