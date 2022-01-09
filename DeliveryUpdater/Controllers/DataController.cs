using System.Threading.Tasks;
using DeliveryUpdater.Data;
using DeliveryUpdater.Ifaces;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryUpdater.Controllers
{
    [Route("api")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDeliveryUpdaterLogic _deliveryUpdaterLogic;
        public DataController(IDeliveryUpdaterLogic deliveryUpdaterLogic)
        {
            _deliveryUpdaterLogic = deliveryUpdaterLogic;
        }
        
        [HttpPost("update_database")]
        public async Task<ActionResult> UpdateDatabase()
        {
            await _deliveryUpdaterLogic.Update(51254);
            return Ok();
        }
    }
}