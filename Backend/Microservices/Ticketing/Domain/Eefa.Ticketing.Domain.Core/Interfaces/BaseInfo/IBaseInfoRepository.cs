using Eefa.Ticketing.Domain.Core.Dtos.BaseInfo;
using Eefa.Ticketing.Domain.Core.Entities.BaseInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Domain.Core.Interfaces.BaseInfo
{
    public interface IBaseInfoRepository
    {
        Task<List<Role>> GetAllRoles();
        Task<List<GetUserDataDto>> GetUsersByRoleIdAsync(int id);
        Task<Role> GetRoleById(int Id);
        Task<List<GetUsersByIdsObjResultDto>> GetUsersByIds(List<int> UserIds);
        Task<List<int>> GetTreeRole(int RoleId);
    }
}
