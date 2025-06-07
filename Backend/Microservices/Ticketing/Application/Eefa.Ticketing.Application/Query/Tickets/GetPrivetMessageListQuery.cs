using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Ticketing.Application.ACL;
using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Query.Tickets
{
    public class GetPrivetMessageListQuery : PaginatedQueryModel, IRequest<ServiceResult<PagedList<GetPrivetMessageListDto>>>
    {
        public int TicketDitailId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
    public class GetPrivetMessageListQueryHandler : IRequestHandler<GetPrivetMessageListQuery, ServiceResult<PagedList<GetPrivetMessageListDto>>>
    {
        private readonly Infrastructure.Patterns.IUnitOfWork _unitOfWork;
        private readonly IAdmin _admin;

        public GetPrivetMessageListQueryHandler(Infrastructure.Patterns.IUnitOfWork unitOfWork, IAdmin admin)
        {
            _unitOfWork = unitOfWork;
            _admin = admin;
        }
        public async Task<ServiceResult<PagedList<GetPrivetMessageListDto>>> Handle(GetPrivetMessageListQuery request, CancellationToken cancellationToken)
        {
            var userAndRoled = await _unitOfWork.TicketDetailRepository.GetCreatorUserAndRoleId(request.TicketDitailId, cancellationToken);
            var userAndRolep = await _unitOfWork.PrivetMessageRepository.GetCreatorUserAndRoleId(request.TicketDitailId, cancellationToken);


            List<GetPrivetMessageListDto> result = new List<GetPrivetMessageListDto>();
            if (request.RoleId == userAndRoled.RoleId)
            {
                result = await _unitOfWork.PrivetMessageRepository.GetPrivetMessageList(request.TicketDitailId, cancellationToken);
            }

            return ServiceResult<PagedList<GetPrivetMessageListDto>>.Success(new PagedList<GetPrivetMessageListDto>()
            {
                Data = result,
                TotalCount = request.PageIndex <= 1
                   ? result.Count
                   : 0
            });

        }
    }
}
