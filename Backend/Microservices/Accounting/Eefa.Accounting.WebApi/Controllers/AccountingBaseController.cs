using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Eefa.Accounting.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Route("api/accounting/[controller]/[action]")]
    public class AccountingBaseController : ControllerBase
    {
        public IMediator _mediator { get; set; }

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");
    }
}
