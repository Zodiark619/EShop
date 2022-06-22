using System;
using System.Linq;
using System.Threading.Tasks;
using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using MongoDB.Driver;

namespace EShop.Product.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private  IMongoDatabase _database;
        private  IMongoCollection<CreateProduct> _collection;

        public ProductRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<CreateProduct>("product");
        }
        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            await _collection.InsertOneAsync(product);
            return new ProductCreated
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CreatedAt = DateTime.UtcNow,

            };
        }

        public async Task<ProductCreated> GetProduct(string ProductId)
        {
            var product= _collection.AsQueryable().Where(x => x.ProductId==ProductId).FirstOrDefault();
            if (product == null)
            {
                throw new Exception("Prororordcut not found.");
            }
            await Task.CompletedTask;
            return new ProductCreated { 
                ProductId=product.ProductId,
                ProductName=product.ProductName,
                CreatedAt=DateTime.UtcNow};
        }
    }
}
