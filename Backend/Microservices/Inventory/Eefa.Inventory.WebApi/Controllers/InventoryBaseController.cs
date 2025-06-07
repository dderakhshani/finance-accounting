
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eefa.Common.Web;

namespace Eefa.Inventory.WebApi.Controllers
{
    [Route("api/inventory/[controller]/[action]")]
    public class InventoryBaseController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");
    }
}