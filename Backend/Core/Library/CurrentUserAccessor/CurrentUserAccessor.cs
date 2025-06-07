using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Library.Exceptions;
using Library.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Library.CurrentUserAccessor
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string GetToken()
        {
            return _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"] ?? throw new InvalidToken();
        }

        public void SetHttpContext(IHttpContextAccessor httpContext)
        {
            _httpContextAccessor = httpContext;
        }

        public List<string>? GetPermissions()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims.Where(x => x.Type == "permission").Select(x => x.Value).ToList();
        }

        public int GetId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "id")?.Value ?? throw new InvalidToken());
        }

        public string GetRefreshToken()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "refreshToken")?.Value ?? throw new InvalidToken();
        }

        public int GetRoleId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "roleId")?.Value ?? throw new InvalidToken());

        }

        public int GetBranchId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "branchId")?.Value ?? throw new InvalidToken());
        }


        public string GetRoleLevelCode()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "levelCode")?.Value ?? throw new InvalidToken();
        }

        public int GetCompanyId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "companyId")?.Value ?? throw new InvalidToken());
        }

        public int GetYearId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "yearId")?.Value ?? throw new InvalidToken());
        }

        public int GetLanguageId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "languageId")?.Value ?? throw new InvalidToken());
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

        public string GetFullName()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "fullName")?.Value ?? throw new Exception("invalid token");

        }

        public async Task<string> GetAccessToken()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            return accessToken;
        }
        public string GetIp()
        {
            return _httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? throw new Exception("invalid token");
        }

        public string GetUsername()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "username")?.Value ?? throw new Exception("invalid token");
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