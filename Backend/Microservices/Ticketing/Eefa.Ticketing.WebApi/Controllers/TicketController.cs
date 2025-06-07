using Eefa.Common;
using Eefa.Common.Web;
using Eefa.Ticketing.Application.Commands.Tickets;
using Eefa.Ticketing.Application.Query.Tickets;
using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.WebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [AllowAnonymous]
    public class TicketController : ApiControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;

        public TicketController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpPost]
        public async Task<ServiceResult<int>> AddTicket(AddTicketCommand model, CancellationToken cancellationToken)
        {
            return await Mediator.Send(model, cancellationToken);
        }
        [HttpPost]
        public async Task<ServiceResult<int>> CloseTicket(CloseTicketCommand model, CancellationToken cancellationToken)
        {
            int userId = GetUserId();
            model.UserId = userId;
            return await Mediator.Send(model, cancellationToken);
        }
        [HttpPost]
        public async Task<ServiceResult<int>> ReadTicket(ReadTicketCommand model, CancellationToken cancellationToken)
        {
            int userId = GetUserId();
            model.UserId = userId;
            return await Mediator.Send(model, cancellationToken);
        }
        [HttpPost]
        public async Task<ServiceResult<int>> ReplyTicket(ReplyTicketCommand model, CancellationToken cancellationToken)
        {
            return await Mediator.Send(model, cancellationToken);
        }
        [HttpPost]
        public async Task<ServiceResult<int>> ForwardTicket(ForwardTicketCommand model, CancellationToken cancellationToken)
        {
            int userId = GetUserId();
            model.UserId = userId;
            return await Mediator.Send(model, cancellationToken);
        }
        [HttpPost]
        public async Task<ServiceResult<int>> AddPrivetMessage(AddPrivetMessageCommand model, CancellationToken cancellationToken)
        {
            return await Mediator.Send(model, cancellationToken);
        }




        [HttpGet, Route("{TicketId}")]
        public async Task<ServiceResult<Domain.Core.Entities.Tickets.Ticket>> GetTicketById([FromRoute] int TicketId, CancellationToken cancellationToken)
        {
            var model = new GetTicketByIdQuery();
            model.TicketId = TicketId;
            return await Mediator.Send(model, cancellationToken);
        }
        [HttpPost]
        public async Task<ServiceResult<Common.Data.PagedList<GetTicketListDto>>> GetTicketList([FromBody] GetTicketListQuery model, CancellationToken cancellationToken)
        {
            int userId = GetUserId();
            int roleId = GetRoleId();
            model.UserId = userId;
            model.RoleId = roleId;
            return await Mediator.Send(model, cancellationToken);
        }
        [HttpGet, Route("{TicketId}")]
        public async Task<ServiceResult<Common.Data.PagedList<GetTicketDetailListDto>>> GetTicketDetailList([FromRoute] int TicketId, CancellationToken cancellationToken)
        {
            var model = new GetTicketDetailListQuery();
            int userId = GetUserId();
            model.UserId = userId;
            model.TicketId = TicketId;
            return await Mediator.Send(model, cancellationToken);
        }
        [HttpGet]
        public async Task<ServiceResult<Common.Data.PagedList<GetPrivetMessageListDto>>> GetPrivetMessageList([FromQuery] int ticketDitailId, CancellationToken cancellationToken)
        {
            GetPrivetMessageListQuery model = new()
            {
                UserId = GetUserId(),
                RoleId = GetRoleId(),
                TicketDitailId = ticketDitailId
            };
            return await Mediator.Send(model, cancellationToken);
        }
        [HttpGet, Route("{ticketDitailId}")]
        public async Task<ServiceResult<List<GetTicketDetailHistoryListDto>>> GetTicketDetailHistoryList([FromRoute] int ticketDitailId, CancellationToken cancellationToken)
        {
            GetTicketDetailHistoryListQuery model = new()
            {
                TicketDitailId = ticketDitailId
            };
            return await Mediator.Send(model, cancellationToken);
        }
        private int GetUserId()
        {

            int userId = int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "id")?.Value);
            return userId;

            //var branch = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "branchId")?.Value;
            //var username = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "username").Value;
        }
        private int GetRoleId()
        {
            var rolId = int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "roleId")?.Value);
            return rolId;
        }
    }
}
