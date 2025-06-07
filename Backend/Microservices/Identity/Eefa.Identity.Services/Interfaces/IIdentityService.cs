using System.Collections.Generic;
using System.Security.Claims;
using Eefa.Identity.Services.Identity;
using Library.Interfaces;

namespace Eefa.Identity.Services.Interfaces
{
    public interface IIdentityService : IService
    {
        IdentityModel GenerateToken(IdentityModel identityModel, IList<Claim> claims, int? expireTimeMinutes = null);
    }
}