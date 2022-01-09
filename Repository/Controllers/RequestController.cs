using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Newtonsoft.Json;
using Repository.Ifaces;
using Repository.Models;

namespace Repository.Controllers
{
    [Route("api")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IProductRepository _repository;
        public RequestController(IProductRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet("get_item")]
        public async Task<IActionResult> Get(string name)
        {
            var product = await _repository.GetProductByName(name);
            if (product == null)
                return NotFound();
            return new JsonResult(product);
        }
        
        [HttpGet("get_all")]
        public async Task<IActionResult> Get()
        {
            var res = await _repository.GetAllAsync();
            return new JsonResult(res.ToList());
        }

        [HttpPost("add_item")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            return await _repository.Create(product);
        }
    }
}