using Library.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.WorkflowAdmin.WebApi.Controllers
{
    [Route("api/admin/[controller]/[action]")]
    public class WorkflowAdminBaseController : BaseController
    {

        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");

    }
}