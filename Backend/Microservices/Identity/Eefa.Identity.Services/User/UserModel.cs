using System.Collections.Generic;
using System.Security.Claims;
using Eefa.Identity.Services.Identity;

namespace Eefa.Identity.Services.User
{
    public class UserModel
    {
        public IdentityModel IdentityModel { get; set; }
        public List<Claim> Claims { get; set; }
    }
}