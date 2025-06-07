using Eefa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure.BackgroundTasks
{
    public class BackgroundUserAccessor : ICurrentUserAccessor
    {
        public int GetId() => 1; // مقدار پیش‌فرض یا مقدار دلخواه
        public List<string> GetPermissions() => new List<string>(); // بدون دسترسی
        public string GetRefreshToken() => throw new NotSupportedException("No token in background service.");
        public int GetRoleId() => 0;
        public int GetBranchId() => 0;
        public string GetRoleLevelCode() => "default";
        public int GetCompanyId() => 0;
        public int GetYearId() => DateTime.Now.Year;
        public int GetLanguageId() => 1; // زبان پیش‌فرض
        public string GetCultureTwoIsoName() => "en";
        public string GetIp() => "127.0.0.1";
        public string GetUsername() => "BackgroundService";
        public Task<string> GetAccessToken() => Task.FromResult(string.Empty);
        public bool IsLoggedIn() => false;
        public bool IsExpiredToken() => true;
    }

}
