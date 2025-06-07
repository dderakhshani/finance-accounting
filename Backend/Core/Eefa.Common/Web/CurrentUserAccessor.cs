using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Eefa.Common.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Eefa.Common.Web
{

    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        const string PERMISSION = "permission";
        const string REFRESH_TOKEN = "refreshToken";
        const string ID = "id";
        const string ROLE_ID = "roleId";
        const string BRANCH_ID = "branchId";
        const string LEVEL_CODE = "levelCode";
        const string COMPANY_ID = "companyId";
        const string YEAR_ID = "yearId";
        const string LANGUAGE_ID = "languageId";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetId()
        {
            if (_httpContextAccessor?.HttpContext?.User?.Claims == null)
                return 0;// for background services

            return int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ID)?.Value ?? "1");
        }

        public List<string> GetPermissions()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims.Where(x => x.Type == PERMISSION).Select(x => x.Value).ToList();
        }

        public string GetRefreshToken()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == REFRESH_TOKEN)?.Value ?? throw new InvalidToken();
        }

        public int GetRoleId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ROLE_ID)?.Value ?? "1");
        }

        public int GetBranchId()
        {
            var branchId = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == BRANCH_ID)?.Value;
            if (!string.IsNullOrEmpty(branchId))
                return int.Parse(branchId);
            else
                return 0;
        }
        public string GetRoleLevelCode()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == LEVEL_CODE)?.Value ?? throw new InvalidToken();
        }

        public int GetCompanyId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == COMPANY_ID)?.Value ?? throw new InvalidToken());
        }

        public int GetYearId()
        {
            if (_httpContextAccessor?.HttpContext?.User?.Claims == null)
                return 0; // for background services
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == YEAR_ID)?.Value ?? throw new InvalidToken());
        }

        public int GetLanguageId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == LANGUAGE_ID)?.Value ?? throw new InvalidToken());
        }

        public string GetCultureTwoIsoName()
        {
            if (_httpContextAccessor.HttpContext != null &&
                _httpContextAccessor.HttpContext.Request.Path.Value != null &&
                _httpContextAccessor.HttpContext.Request.Path.HasValue &&
                _httpContextAccessor.HttpContext.Request.Path.Value.Length >= 4 &&
                _httpContextAccessor.HttpContext.Request.Path.Value[0] == '/' &&
                _httpContextAccessor.HttpContext.Request.Path.Value[3] == '/')
            {
                var cultureCode = _httpContextAccessor.HttpContext.Request.Path.Value.Substring(1, 2);

                return CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .FirstOrDefault(culture => string.Equals(
                        culture.Name,
                        cultureCode,
                        StringComparison.CurrentCultureIgnoreCase))?
                    .TwoLetterISOLanguageName ?? "en";
            }
            else
            {
                return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "cultureTwoIsoName")?.Value ?? "en";
            }
        }

        public string GetIp()
        {
            return _httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? throw new Exception("invalid token");
        }

        public string GetUsername()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "username")?.Value ?? throw new Exception("invalid token");
        }
        public async Task<string> GetAccessToken()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            return accessToken;
        }
        public bool IsLoggedIn()
        {
            return _httpContextAccessor?.HttpContext?.User != null;
        }

        public bool IsExpiredToken()
        {
            var b = (DateTimeOffset
                .FromUnixTimeSeconds(long.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?
                    .FirstOrDefault(x => x.Type == "exp")?.Value)));

            var c = DateTimeOffset.Now;

            var d = b - c < TimeSpan.FromSeconds(10);

            return d;
        }
    }

 
}