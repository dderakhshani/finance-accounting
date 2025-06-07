using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Identity.Data.Databases.Entities;
using Eefa.Identity.Services.Identity;
using Library.Interfaces;

namespace Eefa.Identity.Services.Interfaces
{
    public interface IUserSettingsAccessor : IService
    {
        Task<Role> CurrentRole(int userId, CancellationToken cancellationToken);
        Task<Role> ChangeCurrentRole(int userId, int newRoleId, CancellationToken cancellationToken);
        List<Claim> AddRoleClaims(Role role);

        Task<CompanyInformation> CurrentCompany(int userId, CancellationToken cancellationToken);
        Task<CompanyInformation> ChangeCurrentCompany(int userId, int newCompanyId, CancellationToken cancellationToken);
        List<Claim> AddCompanyClaims(CompanyInformation companyInformation);

        Task<Language> CurrentLanguage(int userId, CancellationToken cancellationToken);
        Task<Language> ChangeCurrentLanguage(int userId, int newLanguageId, CancellationToken cancellationToken);
        List<Claim> AddLanguageClaims(Language language);     
        
        Task<Year> CurrentYear(int userId, CancellationToken cancellationToken);
        Task<Year> ChangeCurrentYear(int userId, int newYearId, CancellationToken cancellationToken);
        List<Claim> AddYearClaims(Year language);

        Task<List<Claim>> ChangeUserSetting(List<Claim> claims,IdentityModel identityModel, Data.Databases.Entities.User user,
            CancellationToken cancellationToken);

        Task ChangeUserSetting(IdentityModel identityModel, CancellationToken cancellationToken);

    }
}