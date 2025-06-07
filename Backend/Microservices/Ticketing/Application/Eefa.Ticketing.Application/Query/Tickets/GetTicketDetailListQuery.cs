using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Ticketing.Application.ACL;
using Eefa.Ticketing.Application.Contract.Interfaces.Tickets;
using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Domain.Core.Enums;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Query.Tickets
{
    public class GetTicketDetailListQuery : IRequest<ServiceResult<PagedList<GetTicketDetailListDto>>>
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
    }
    public class GetTicketDetailListQueryHandler : IRequestHandler<GetTicketDetailListQuery, ServiceResult<PagedList<GetTicketDetailListDto>>>
    {
        private readonly Infrastructure.Patterns.IUnitOfWork _unitOfWork;
        private readonly ITicketService _ticketService;
        private readonly IIdentity _identity;
        private readonly IAdmin _admin;

        public GetTicketDetailListQueryHandler(Infrastructure.Patterns.IUnitOfWork unitOfWork, ITicketService ticketService, IAdmin admin, IIdentity identity)
        {
            _unitOfWork = unitOfWork;
            _ticketService = ticketService;
            _admin = admin;
            _identity = identity;
        }
        public async Task<ServiceResult<PagedList<GetTicketDetailListDto>>> Handle(GetTicketDetailListQuery request, CancellationToken cancellationToken)
        {
            List<TicketDetail> unreadedList = await _unitOfWork.TicketDetailRepository.GetUnreadListAsync(request.TicketId, cancellationToken);
            foreach (var item in unreadedList)
            {
                item.ReadTicket(request.UserId);
                if (item.ReadDate != null)
                {
                    _unitOfWork.TicketDetailRepository.Edit(item, cancellationToken);
                    await _ticketService.ChangeTicketStatusAsync(null, item.TicketId, TicketStatus.Ongoing, cancellationToken);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            List<GetTicketDetailListDto> result = await _unitOfWork.TicketDetailRepository.GetList(request.TicketId, cancellationToken);

            //var loginResult = await _identity.LoginAsync();
            //var allRole = await _admin.GetAllRoleAsync(loginResult.objResult);
            var allRole = await _unitOfWork.BaseInfoRepository.GetAllRoles();
            var rolsId = result.Select(x => x.RoleId).ToList();

            result = (from ticketdetail in result
                      join role in allRole on ticketdetail.RoleId equals role.Id
                      select new GetTicketDetailListDto()
                      {
                          Id = ticketdetail.Id,
                          RoleId = role.Id,
                          RoleName = role.Title,
                          AttachmentIds = ticketdetail.AttachmentIds,
                          TicketId = ticketdetail.TicketId,
                          CreatDate = ticketdetail.CreatDate,
                          DetailCreatorUserId = ticketdetail.DetailCreatorUserId,
                          Description = ticketdetail.Description,
                          ReadDate = ticketdetail.ReadDate,
                          ReaderUserId = ticketdetail.ReaderUserId,
                          TicketTitle = ticketdetail.TicketTitle,
                          TicketCreatorUserId = ticketdetail.TicketCreatorUserId,
                          DetailCreatorUserRoleId = ticketdetail.DetailCreatorUserRoleId,
                          DetailCreatorUserFullName = ticketdetail.DetailCreatorUserFullName,
                          DetailCreatorUserRoleName = ticketdetail.DetailCreatorUserRoleName,
                          ReaderUserFullName = ticketdetail.ReaderUserFullName,
                          TicketCreatorUserFullName = ticketdetail.TicketCreatorUserFullName,
                          HistoryCount = ticketdetail.HistoryCount,
                          TicketStatus = ticketdetail.TicketStatus,
                      }).ToList();

            var userSelected = result.Select(x => x.DetailCreatorUserId).ToList();

            var userresult = await _unitOfWork.BaseInfoRepository.GetUsersByIds(userSelected);


            result = (from ticketdetail in result
                      join user in userresult on new
                      {
                          UserId = ticketdetail.DetailCreatorUserId,
                          RoleId = ticketdetail.DetailCreatorUserRoleId
                      } equals new
                      {
                          user.UserId,
                          user.RoleId
                      }
                      select new GetTicketDetailListDto()
                      {
                          Id = ticketdetail.Id,
                          RoleId = ticketdetail.RoleId,
                          RoleName = ticketdetail.RoleName,
                          AttachmentIds = ticketdetail.AttachmentIds,
                          TicketId = ticketdetail.TicketId,
                          CreatDate = ticketdetail.CreatDate,
                          DetailCreatorUserId = user.UserId,
                          Description = ticketdetail.Description,
                          ReadDate = ticketdetail.ReadDate,
                          ReaderUserId = ticketdetail.ReaderUserId,
                          TicketTitle = ticketdetail.TicketTitle,
                          TicketCreatorUserId = ticketdetail.TicketCreatorUserId,
                          DetailCreatorUserFullName = user.Fullname,
                          DetailCreatorUserRoleId = user.RoleId,
                          DetailCreatorUserRoleName = user.RoleTitle,
                          TicketCreatorUserFullName = ticketdetail.TicketCreatorUserFullName,
                          HistoryCount = ticketdetail.HistoryCount,
                          TicketStatus = ticketdetail.TicketStatus,
                      }).ToList();


            return ServiceResult<PagedList<GetTicketDetailListDto>>.Success(new PagedList<GetTicketDetailListDto>()
            {
                Data = result,
                TotalCount = result.Count
            });
        }
    }
    record Person(string FirstName, string LastName);
    record Pet(string Name, Person Owner);
    record Employee(string FirstName, string LastName, int EmployeeID);
    record Cat(string Name, Person Owner) : Pet(Name, Owner);
    record Dog(string Name, Person Owner) : Pet(Name, Owner);
}
