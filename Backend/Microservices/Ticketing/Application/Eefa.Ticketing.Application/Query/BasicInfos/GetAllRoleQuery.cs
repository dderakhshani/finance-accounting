using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Ticketing.Application.ACL;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Ticketing.Domain.Core.Entities.BaseInfo;

namespace Eefa.Ticketing.Application.Query.BasicInfos
{
    public class GetAllRoleQuery : IRequest<ServiceResult<PagedList<Role>>>
    {
    }
    public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, ServiceResult<PagedList<Role>>>
    {
        private readonly IIdentity _identity;
        private readonly IAdmin _admin;
        private readonly Eefa.Ticketing.Infrastructure.Patterns.IUnitOfWork _unitOfWork;
        public GetAllRoleQueryHandler(IIdentity identity, IAdmin admin, Eefa.Ticketing.Infrastructure.Patterns.IUnitOfWork unitOfWork)
        {
            _identity = identity;
            _admin = admin;
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<PagedList<Role>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            //    var loginResult = await _identity.LoginAsync();
            //    var result = await _admin.GetAllRoleAsync(loginResult.objResult);
            var result = await _unitOfWork.BaseInfoRepository.GetAllRoles();
            return ServiceResult<PagedList<Role>>.Success(new PagedList<Role>()
            {
                Data = result,
                TotalCount = result.Count()
            });
        }
    }
}
