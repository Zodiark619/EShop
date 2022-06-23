using System;
using System.Threading.Tasks;
using EShop.Infrastructure.Command.Product;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IBusControl _bus;
        public ProductController(IBusControl bus)
        {
            _bus = bus;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.CompletedTask;
            return Accepted("Get producct method calll");
        }



        [HttpPost]
        public async Task<IActionResult> Add([FromForm]CreateProduct product)
        {
            var uri = new Uri("rabbitmq://localhost/create_product");
            var endPoint=await _bus.GetSendEndpoint(uri);

            await endPoint.Send(product);
            return Accepted("Product created~");
        }
    }
}
