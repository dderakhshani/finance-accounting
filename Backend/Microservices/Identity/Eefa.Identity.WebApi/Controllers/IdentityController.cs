using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Identity.Services.Identity;
using Eefa.Identity.Services.Interfaces;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;

namespace Eefa.Identity.WebApi.Controllers
{
    public class IdentityController : IdentityBaseController
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IConfiguration _configuration;
        private readonly IUserSettingsAccessor _userSettingsAccessor;
        public IdentityController(IUserService userService, IIdentityService identityService, ICurrentUserAccessor currentUserAccessor, IConfiguration configuration, IUserSettingsAccessor userSettingsAccessor)
        {
            _userService = userService;
            _identityService = identityService;
            _currentUserAccessor = currentUserAccessor;
            _configuration = configuration;
            _userSettingsAccessor = userSettingsAccessor;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] IdentityModel model)
        {

            if (model.Username is not null)
            {
                model.Username = model.Username.ToLower();
            }


            model.Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var user = await _userService.GetUserByUserPass(model);
            model.Id = user.Id;
            var claims = await _userService.GetClaims(model, user);
            var token = _identityService.GenerateToken(model, claims);

            return Ok(ServiceResult.Success(token.AccessToken));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login2([FromBody] IdentityModel model)
        {
            if (model.Username is not null)
            {
                model.Username = model.Username.ToLower();
            }

            model.Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var user = await _userService.GetUserByUserPass(model);
            model.Id = user.Id;

            var tokens = (await _userService.GetClaims2(model, user, CancellationToken.None))
                .ToDictionary(claimsDic =>
                    claimsDic.Key, claimsDic =>
                    _identityService.GenerateToken(model, claimsDic.Value, 1440 * 90).AccessToken);

            return Ok(ServiceResult.Success(tokens));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromForm] string username, [FromForm] string password, [FromForm] int expireTimeMinutes)
        {
            var identity = new IdentityModel
            {
                Username = username?.ToLower(),
                Password = password
            };
            var user = await _userService.GetUserByUserPass(identity);
            identity.Id = user.Id;
            var claims = await _userService.GetClaims(identity, user);
            var token = _identityService.GenerateToken(identity, claims, expireTimeMinutes);

            return Ok(ServiceResult.Success(token.AccessToken));
        }

        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] IdentityModel model)
        {
            var user = await _userService.GetUserById(_currentUserAccessor.GetId());

            if (model.YearId != default && !user.UserYearUsers.Any(x => x.YearId == model.YearId)) throw new AuthenticationException();
            else
            {
                if (user.UserYearUsers.Count == 0) throw new AuthenticationException("User does not have access to the selected yearId");

                var userLastYearSetting = user.UserSettingUsers.FirstOrDefault(x => x.Keyword == "LastYearId");
                var userLastYearId = int.Parse(userLastYearSetting?.Value);
                if (userLastYearId != default && !user.UserYearUsers.Any(x => x.YearId == userLastYearId))
                {
                    userLastYearSetting.Value = user.UserYearUsers.FirstOrDefault()?.YearId.ToString();
                };

            }

            if (model.RoleId != default && !user.UserRoleUsers.Any(x => x.RoleId == model.RoleId)) throw new AuthenticationException("User does not have the selected roleId");
            else
            {
                if (user.UserRoleUsers.Count == 0) throw new AuthenticationException();

                var userLastRoleSetting = user.UserSettingUsers.FirstOrDefault(x => x.Keyword == "LastRoleId");
                var userLastRoleId = int.Parse(userLastRoleSetting?.Value);
                if (userLastRoleId != default && !user.UserRoleUsers.Any(x => x.RoleId == userLastRoleId))
                {
                    userLastRoleSetting.Value = user.UserRoleUsers.FirstOrDefault()?.RoleId.ToString();
                };
            }


            var identityModel = new IdentityModel()
            {
                RoleId = model.RoleId != default ? model.RoleId : int.Parse(user.UserSettingUsers.FirstOrDefault(x => x.Keyword == "LastRoleId")?.Value),
                CompanyId = model.CompanyId != default ? model.CompanyId : int.Parse(user.UserSettingUsers.FirstOrDefault(x => x.Keyword == "LastCompanyId")?.Value),
                YearId = model.YearId != default ? model.YearId : int.Parse(user.UserSettingUsers.FirstOrDefault(x => x.Keyword == "LastYearId")?.Value),
                LanguageId = model.LanguageId != default ? model.LanguageId : int.Parse(user.UserSettingUsers.FirstOrDefault(x => x.Keyword == "LastLanguageId")?.Value),
                Username = user.Username,
                Ip = _currentUserAccessor.GetIp(),
                Id = user.Id
            };



            await _userSettingsAccessor.ChangeUserSetting(identityModel, CancellationToken.None);

            var claims = await _userService.GetClaims(identityModel, user);

            var token = _identityService.GenerateToken(identityModel, claims);
            return Ok(ServiceResult.Success(token.AccessToken));
        }

        [HttpGet]
        public async Task<IActionResult> Profile() => Ok(ServiceResult.Success(await _userService.GetProfile(_currentUserAccessor.GetId())));

    }
}
