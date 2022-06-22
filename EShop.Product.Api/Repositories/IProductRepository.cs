using System;
using System.Threading.Tasks;
using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;

namespace EShop.Product.Api.Repositories
{
    public interface IProductRepository
    {
        Task<ProductCreated> GetProduct(string ProductId);
        Task<ProductCreated> AddProduct(CreateProduct product);

    }
}
