using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Common
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
        public IMediator _mediator { get; set; }

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

      
    }
}