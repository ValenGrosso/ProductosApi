using ProductosApi.Data;
using ProductosApi.Entities;
using ProductosApi.Repositories.Interfaces;

namespace ProductosApi.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductosApiContext _context;

        public ProductRepository(ProductosApiContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product? GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existing = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public List<Product> SearchProductsByName(string name)
        {
            return _context.Products
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }
    }
}