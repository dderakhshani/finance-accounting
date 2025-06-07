using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Eefa.Admin.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Route("api/admin/[controller]/[action]")]
    public class AdminBaseController : ControllerBase
    {

        public IMediator _mediator { get; set; }

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");

    }
}