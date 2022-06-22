using System;
using System.Threading.Tasks;
using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using EShop.Product.Api.Repositories;

namespace EShop.Product.Api.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            
            return await _repository.AddProduct(product);
        }

        public async Task<ProductCreated> GetProduct(string ProductId)
        {
            return await _repository.GetProduct(ProductId);
        }
    }
}
