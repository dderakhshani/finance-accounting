using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Ticketing.Application.ACL;
using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Query.Tickets
{
    public class GetTicketListQuery : GetTicketListInputDto, IRequest<ServiceResult<PagedList<GetTicketListDto>>>
    {
    }
    public class GetTicketListQueryHandler : IRequestHandler<GetTicketListQuery, ServiceResult<PagedList<GetTicketListDto>>>
    {
        private readonly Infrastructure.Patterns.IUnitOfWork _unitOfWork;
        private readonly IIdentity _identity;
        private readonly IAdmin _admin;

        public GetTicketListQueryHandler(Infrastructure.Patterns.IUnitOfWork unitOfWork, IIdentity identity, IAdmin admin)
        {
            _unitOfWork = unitOfWork;
            _identity = identity;
            _admin = admin;
        }
        public async Task<ServiceResult<PagedList<GetTicketListDto>>> Handle(GetTicketListQuery request, CancellationToken cancellationToken)
        {

            //var loginResult = await _identity.LoginAsync();
            //var result = await _admin.GetRoleTreeAsync(loginResult.objResult, request.RoleId);
            var result = await _unitOfWork.BaseInfoRepository.GetTreeRole(request.RoleId);
            List<int> roleIds = new List<int>();
            for (int i = 0; i < result.Count; i++)
            {
                roleIds.Add(result[i]);
            }
            List<GetTicketListDto> tickets = await _unitOfWork.TicketRepository.GetList(request, roleIds, cancellationToken);

            return ServiceResult<PagedList<GetTicketListDto>>.Success(new PagedList<GetTicketListDto>()
            {
                Data = tickets,
                TotalCount = request.PageIndex <= 1
                   ? tickets.Count
                   : 0
            });
        }
    }
}
