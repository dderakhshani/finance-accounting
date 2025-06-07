using Library.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileTransfer.WebApi.Controllers
{
    [Route("api/fileTransfer/[controller]/[action]")]
    public class FileTransferBaseController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");
    }
}