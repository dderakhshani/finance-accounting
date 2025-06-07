using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Ticketing.Application.ACL;
using Eefa.Ticketing.Domain.Core.Dtos.BaseInfo;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Query.BasicInfos
{
    public class GetUsersByRoleIdQuery : IRequest<ServiceResult<PagedList<GetUserDataDto>>>
    {
        public int RoleId { get; set; }
    }
    public class GetUsersByRoleIdHandler : IRequestHandler<GetUsersByRoleIdQuery, ServiceResult<PagedList<GetUserDataDto>>>
    {
        private readonly IIdentity _identity;
        private readonly IAdmin _admin;
        private readonly Eefa.Ticketing.Infrastructure.Patterns.IUnitOfWork _unitOfWork;
        public GetUsersByRoleIdHandler(IIdentity identity, IAdmin admin, Eefa.Ticketing.Infrastructure.Patterns.IUnitOfWork unitOfWork)
        {
            _identity = identity;
            _admin = admin;
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<PagedList<GetUserDataDto>>> Handle(GetUsersByRoleIdQuery request, CancellationToken cancellationToken)
        {
            //var loginResult = await _identity.LoginAsync();
            //var result = await _admin.GetUsersByRoleIdAsync(request.RoleId, loginResult.objResult);

            var result = await _unitOfWork.BaseInfoRepository.GetUsersByRoleIdAsync(request.RoleId);
            return ServiceResult<PagedList<GetUserDataDto>>.Success(new PagedList<GetUserDataDto>()
            {
                Data = result,
                TotalCount = result.Count
            });
        }
    }
}
