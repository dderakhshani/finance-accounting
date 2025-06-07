using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface ICurrentUserAccessor
    {
        public void SetHttpContext(IHttpContextAccessor httpContext);

        public int GetId();

        List<string>? GetPermissions();
        public string GetRefreshToken();
        public int GetRoleId();
        public string GetRoleLevelCode();
        public int GetCompanyId();
        public int GetYearId();
        public int GetLanguageId();
        public string GetIp();
        public string GetUsername();
        public bool IsExpiredToken();
        public int GetBranchId();
        public bool IsLoggedIn();
        public string GetCultureTwoIsoName();
        public string GetFullName();
        public Task<string> GetAccessToken();
    }
}