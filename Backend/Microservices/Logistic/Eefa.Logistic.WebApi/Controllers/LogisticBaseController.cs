
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eefa.Common.Web;

namespace Eefa.Logistic.WebApi.Controllers
{
    [Route("api/logistics/[controller]/[action]")]
    public class LogisticBaseController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");
    }
}