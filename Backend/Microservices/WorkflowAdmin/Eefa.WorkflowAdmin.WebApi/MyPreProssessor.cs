using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Library.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Eefa.WorkflowAdmin.WebApi
{
    public class MyPreProssessor : IPrePublishProcessor
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IConfigurationAccessor _configurationAccessor;
        public MyPreProssessor(ICurrentUserAccessor currentUserAccessor, IHttpContextAccessor httpContextAccessor, IConfigurationAccessor configurationAccessor)
        {
            _currentUserAccessor = currentUserAccessor;
            _httpContextAccessor = httpContextAccessor;
            _configurationAccessor = configurationAccessor;
        }

        public void Process<T>(ConsumeContext<T> context) where T : class
        {
            if (context == null) return;
            try
            {
                var token = context.Headers.Where(x => x.Key == "Authorization").Select(x => x.Value.ToString()).FirstOrDefault();
                if (string.IsNullOrEmpty(token)) return;



                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationAccessor.GetJwtConfiguration().Secret)),
                    ValidAudience = _configurationAccessor.GetJwtConfiguration().Audience,
                    ValidIssuer = _configurationAccessor.GetJwtConfiguration().Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                token = token.Substring(7, token.Length-7);

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                _httpContextAccessor.HttpContext = new DefaultHttpContext();
                _httpContextAccessor.HttpContext.User =
                    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim("id", principal.Claims.First(x=>x.Type == "id").Value.ToString()),
                        new Claim("yearId", principal.Claims.First(x=>x.Type == "yearId").Value.ToString()),
                        new Claim("companyId", principal.Claims.First(x=>x.Type == "companyId").Value.ToString()),
                        new Claim("roleId", principal.Claims.First(x=>x.Type == "roleId").Value.ToString()),
                        new Claim("levelCode", principal.Claims.First(x=>x.Type == "levelCode").Value.ToString()),
                        new Claim("languageId",principal.Claims.First(x=>x.Type == "languageId").Value.ToString()),
                    }));
                _currentUserAccessor.SetHttpContext(new HttpContextAccessor() { });

                var a = _currentUserAccessor.GetId();
            }
            catch
            {

            }
        }

    }
}