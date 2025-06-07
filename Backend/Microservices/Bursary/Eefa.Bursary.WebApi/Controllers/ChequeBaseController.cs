using Eefa.Common.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class ChequeBaseController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");
    }
}
