using System.Threading.Tasks;
using EShop.Infrastructure.Command.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.CompletedTask;
            return Accepted("Get producct method calll");
        }



        [HttpPost]
        public async Task<IActionResult> Add([FromForm]CreateProduct product)
        {
            await Task.CompletedTask;
            return Accepted("Product created~");
        }
    }
}
