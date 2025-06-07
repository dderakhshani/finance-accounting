using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Identity.Services.Identity;
using Eefa.Identity.Services.User.Models;
using Library.Interfaces;

namespace Eefa.Identity.Services.Interfaces
{
    public interface IUserService : IService
    {
        Task<Data.Databases.Entities.User> GetUserByUserPass(IdentityModel identityModel, CancellationToken cancellationToken = new CancellationToken());
        Task<Data.Databases.Entities.User> GetUserById(int id, CancellationToken cancellationToken = new CancellationToken());

        Task<List<Claim>> GetClaims(IdentityModel identityModel, Data.Databases.Entities.User user,
            CancellationToken cancellationToken = new CancellationToken());

        Task<Dictionary<string, List<Claim>>> GetClaims2(IdentityModel identityModel, Data.Databases.Entities.User user,
            CancellationToken cancellationToken);

        Task<UserProfileModel> GetProfile(int id);

    }
}