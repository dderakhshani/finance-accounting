using Eefa.Common;
using Eefa.Ticketing.Application.ACL;
using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Query.Tickets
{
    public class GetTicketDetailHistoryListQuery : IRequest<ServiceResult<List<GetTicketDetailHistoryListDto>>>
    {
        public int TicketDitailId { get; set; }
    }
    public class GetTicketDetailHistoryListQueryHandler : IRequestHandler<GetTicketDetailHistoryListQuery, ServiceResult<List<GetTicketDetailHistoryListDto>>>
    {
        private readonly Infrastructure.Patterns.IUnitOfWork _unitOfWork;
        private readonly IAdmin _admin;
        private readonly IIdentity _identity;
        public GetTicketDetailHistoryListQueryHandler(Infrastructure.Patterns.IUnitOfWork unitOfWork, IAdmin admin, IIdentity identity)
        {
            _unitOfWork = unitOfWork;
            _admin = admin;
            _identity = identity;
        }

        public async Task<ServiceResult<List<GetTicketDetailHistoryListDto>>> Handle(GetTicketDetailHistoryListQuery request, CancellationToken cancellationToken)
        {
            var detailHistories = _unitOfWork.DetailHistoryRepository.GetHistoriesIncludeMessage(request.TicketDitailId, cancellationToken);

            //var loginResult = await _identity.LoginAsync();
            //var allRole = await _admin.GetAllRoleAsync(loginResult.objResult);
            var allRole = await _unitOfWork.BaseInfoRepository.GetAllRoles();

            List<GetTicketDetailHistoryListDto> result = (from h in detailHistories
                                                          join p in allRole on h.PrimaryRoleId equals p.Id
                                                          join s in allRole on h.SecondaryRoleId equals s.Id

                                                          select new GetTicketDetailHistoryListDto()
                                                          {
                                                              HistoryId = h.HistoryId,
                                                              CreatDate = h.CreatDate,
                                                              PrimaryRoleId = h.PrimaryRoleId,
                                                              SecondaryRoleId = h.SecondaryRoleId,
                                                              TicketDetailId = h.TicketDetailId,
                                                              PrimaryRoleName = p.Title,
                                                              SecondaryRoleName = s.Title,
                                                              Message = h.Message
                                                          }).Distinct().ToList();

            return ServiceResult<List<GetTicketDetailHistoryListDto>>.Success(result);
        }
    }
}
