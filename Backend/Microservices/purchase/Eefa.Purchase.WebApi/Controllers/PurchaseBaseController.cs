
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eefa.Common.Web;

namespace Eefa.Purchase.WebApi.Controllers
{
    [Route("api/Purchase/[controller]/[action]")]
    public class PurchaseBaseController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");
    }
}