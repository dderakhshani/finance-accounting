using System.Threading.Tasks;
using Library.Common;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Identity.WebApi.Controllers
{
      //[Route("{culture}/api/[controller]/[action]")]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]/[action]")]
    public class IdentityBaseController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetJsonValidators()
        {
            var json = await System.IO.File.ReadAllTextAsync("CreateAccountHeadCommand.json");
            return Ok(ServiceResult.Success(json));
        }
    }
}