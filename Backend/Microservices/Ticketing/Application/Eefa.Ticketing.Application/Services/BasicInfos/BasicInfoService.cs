using Eefa.Ticketing.Application.ACL;
using Eefa.Ticketing.Domain.Core.Dtos.BaseInfo;
using Eefa.Ticketing.Domain.Core.Entities.BaseInfo;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Services.BasicInfos
{
    public class BasicInfoService : IBasicInfoService
    {
        private readonly IConfiguration _configuration;
        private readonly IIdentity _identity;
        private readonly IAdmin _admin;
        private readonly Eefa.Ticketing.Infrastructure.Patterns.IUnitOfWork _unitOfWork;
        public BasicInfoService(IConfiguration configuration, IIdentity identity, IAdmin admin, Eefa.Ticketing.Infrastructure.Patterns.IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _identity = identity;
            _admin = admin;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            //var loginResult = await _identity.LoginAsync();
            //var result = await _admin.GetAllRoleAsync(loginResult.objResult);
            var result = await _unitOfWork.BaseInfoRepository.GetAllRoles();
            return result;
        }
        public async Task<List<GetUserDataDto>> GetUsersByRoleIdAsync(int roleId)
        {
            //var loginResult = await _identity.LoginAsync();
            //var result = await _admin.GetUsersByRoleIdAsync(roleId, loginResult.objResult);
            var result = await _unitOfWork.BaseInfoRepository.GetUsersByRoleIdAsync(roleId);
            return result;
        }
    }

}
