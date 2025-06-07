using Eefa.Common;
using Eefa.Common.Web;
using Eefa.Ticketing.Application.Query.BasicInfos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Eefa.Ticketing.Domain.Core.Entities.BaseInfo;
using System.Threading.Tasks;
using Eefa.Ticketing.Domain.Core.Dtos.BaseInfo;

namespace Eefa.Ticketing.WebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [AllowAnonymous]
    public class BasicInfoController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ServiceResult<Common.Data.PagedList<Role>>> GetAllRole(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetAllRoleQuery(), cancellationToken);
        }
        [HttpGet]
        public async Task<ServiceResult<Common.Data.PagedList<GetUserDataDto>>> GetUsersByRoleId([FromQuery] GetUsersByRoleIdQuery model, CancellationToken cancellationToken)
        {
            return await Mediator.Send(model, cancellationToken);
        }
    }
}
