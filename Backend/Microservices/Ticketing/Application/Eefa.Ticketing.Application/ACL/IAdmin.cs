using Eefa.Ticketing.Application.Contract.Dtos.BasicInfos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.ACL
{
    public interface IAdmin
    {
        Task<RoleAllResult> GetAllRoleAsync(string token);
        Task<GetUsersByRoleIdResult> GetUsersByRoleIdAsync(int roleId, string token);
        Task<GetRoleTree> GetRoleTreeAsync(string token, int roleId);
        Task<GetUsersByIdsResult> GetUsersByIdsAsync(List<int> userIds, string token);
        Task<GetRoleById> GetRoleById(int id, string token);
    }
}
