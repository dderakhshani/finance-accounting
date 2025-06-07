using Eefa.Ticketing.Domain.Core.Dtos.BaseInfo;
using Eefa.Ticketing.Domain.Core.Entities.BaseInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.Services.BasicInfos
{
    public interface IBasicInfoService
    {
        Task<List<Role>> GetAllRoles();
        Task<List<GetUserDataDto>> GetUsersByRoleIdAsync(int roleId);
    }
}
