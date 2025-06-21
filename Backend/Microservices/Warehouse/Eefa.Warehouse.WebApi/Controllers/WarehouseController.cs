using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Warehouse.WebApi.Controllers
{
    public class WarehouseController : WarehouseBaseControlle
    {

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(true);

        }

    }
}
