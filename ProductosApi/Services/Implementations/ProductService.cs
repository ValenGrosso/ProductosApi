using ProductosApi.Entities;
using ProductosApi.Models.DTOs.Requests;
using ProductosApi.Models.DTOs.Responses;
using ProductosApi.Repositories.Interfaces;
using ProductosApi.Services.Interfaces;

namespace ProductosApi.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public List<ProductForReadDto> GetAllProducts()
        {
            return _repository.GetAllProducts()
                .Select(p => new ProductForReadDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                })
                .ToList();
        }
        public ProductForReadDto? GetProductById(int id)
        {
            var product = _repository.GetProductById(id);
            if (product == null) return null;

            return new ProductForReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }
        public ProductForReadDto CreateProduct(ProductForCreateDto dto)
        {
            var nameExists = _repository.GetAllProducts()
                .Any(p => p.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

            if (nameExists)
            {
                throw new InvalidOperationException("Ya existe un producto con ese nombre.");
            }

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price
            };

            _repository.AddProduct(product);

            return new ProductForReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }
        public void UpdateProduct(int id, ProductForUpdateDto dto)
        {
            var product = new Product
            {
                Id = id,
                Name = dto.Name,
                Price = dto.Price
            };

            _repository.UpdateProduct(product);
        }
        public void DeleteProduct(int id)
        {
            var product = _repository.GetProductById(id);
            if (product != null)
            {
                _repository.DeleteProduct(product);
            }
        }
        public List<ProductForReadDto> SearchProductsByName(string name)
        {
            return _repository.SearchProductsByName(name)
                .Select(p => new ProductForReadDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                })
                .ToList();
        }
        public ProductStatsDto GetStats()
        {
            var products = _repository.GetAllProducts();

            if (!products.Any())
            {
                return new ProductStatsDto
                {
                    Total = 0,
                    AveragePrice = 0,
                    MostExpensiveName = string.Empty
                };
            }

            return new ProductStatsDto
            {
                Total = products.Count(),
                AveragePrice = products.Average(p => p.Price),
                MostExpensiveName = products.OrderByDescending(p => p.Price).First().Name
            };
        }
    }
}
