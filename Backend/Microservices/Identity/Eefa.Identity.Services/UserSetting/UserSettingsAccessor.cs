using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Identity.Data.Databases.Entities;
using Eefa.Identity.Services.Identity;
using Eefa.Identity.Services.Interfaces;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Identity.Services.UserSetting
{
    public class UserSettingsAccessor : IUserSettingsAccessor
    {
        private readonly IRepository _repository;

        public UserSettingsAccessor(IRepository repository)
        {
            _repository = repository;
        }

        public List<Claim> AddRoleClaims(Role role)
        {
            return new List<Claim>()
            {
                new ("roleId", role.Id.ToString()),
                new ("userRoleName", role.Title),
                new ("levelCode", role.LevelCode)
            };
        }

        public async Task<Role> CurrentRole(int userId, CancellationToken cancellationToken)
        {
            var role = await _repository.Find<Data.Databases.Entities.UserSetting>(x =>
                    x.ConditionExpression(x => x.UserId == userId && x.Keyword == "LastRoleId"))
                .FirstOrDefaultAsync(cancellationToken);
            if (role == null)
            {
                var userRole = await _repository.Find<UserRole>(x =>
                        x.ConditionExpression(x => x.UserId == userId))
                    .FirstOrDefaultAsync(cancellationToken);
                _repository.Insert(new Data.Databases.Entities.UserSetting()
                {
                    Keyword = "LastRoleId",
                    Value = userRole.RoleId.ToString(),
                    UserId = userId,
                });
                await _repository.SaveChangesAsync(cancellationToken);
                return await _repository.Find<Role>(x =>
                        x.ObjectId(userRole.RoleId))
                    .FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                return await _repository.Find<Role>(x =>
                        x.ObjectId(int.Parse(role.Value)))
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }

        public async Task<Role> ChangeCurrentRole(int userId, int newRoleId, CancellationToken cancellationToken)
        {
            var currentRole = await _repository.Find<Data.Databases.Entities.UserSetting>(x =>
                x.ConditionExpression(x => x.UserId == userId && x.Keyword == "LastRoleId")).FirstOrDefaultAsync(cancellationToken);

            if (currentRole == null)
            {
                _repository.Insert(new Data.Databases.Entities.UserSetting()
                {
                    Keyword = "LastRoleId",
                    Value = newRoleId.ToString(),
                    UserId = userId,
                });
            }
            else
            {
                currentRole.Value = newRoleId.ToString();
                _repository.Update(currentRole);
            }
            await _repository.SaveChangesAsync(cancellationToken);

            return await _repository.Find<Role>(x =>
                    x.ObjectId(newRoleId))
                .FirstAsync(cancellationToken);
        }



        public List<Claim> AddCompanyClaims(CompanyInformation companyInformation)
        {
            return new List<Claim>()
            {
                new ("companyId", companyInformation.Id.ToString())
            };
        }

        public async Task<CompanyInformation> CurrentCompany(int userId, CancellationToken cancellationToken)
        {
            var companySetting = await _repository.Find<Data.Databases.Entities.UserSetting>(x =>
                    x.ConditionExpression(t => t.UserId == userId && t.Keyword == "LastCompanyId"))
                .FirstOrDefaultAsync(cancellationToken);


            if (companySetting == null)
            {
                var userYear = await _repository.Find<UserYear>(x =>
                        x.ConditionExpression(x => x.UserId == userId)).Include(x=>x.Year)
                    .FirstOrDefaultAsync(cancellationToken);

                

                _repository.Insert(new Data.Databases.Entities.UserSetting()
                {
                    Keyword = "LastCompanyId",
                    Value = userYear.Year.CompanyId.ToString(),
                    UserId = userId,
                });
                await _repository.SaveChangesAsync(cancellationToken);
                return await _repository.Find<CompanyInformation>(x =>
                        x.ObjectId(userYear.Year.CompanyId))
                    .FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                return await _repository.Find<CompanyInformation>(x =>
                        x.ObjectId(int.Parse(companySetting.Value)))
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }

        public async Task<CompanyInformation> ChangeCurrentCompany(int userId, int newCompanyId, CancellationToken cancellationToken)
        {
            var companySetting = await _repository.Find<Data.Databases.Entities.UserSetting>(x =>
                x.ConditionExpression(x => x.UserId == userId && x.Keyword == "LastCompanyId")).FirstOrDefaultAsync(cancellationToken);

            if (companySetting == null)
            {
                _repository.Insert(new Data.Databases.Entities.UserSetting()
                {
                    Keyword = "LastCompanyId",
                    Value = newCompanyId.ToString(),
                    UserId = userId,
                });
            }
            else
            {
                companySetting.Value = newCompanyId.ToString();
                _repository.Update(companySetting);
            }
            await _repository.SaveChangesAsync(cancellationToken);

            return await _repository.Find<CompanyInformation>(x =>
                    x.ObjectId(newCompanyId))
                .FirstAsync(cancellationToken);
        }






        public List<Claim> AddYearClaims(Year companyInformation)
        {
            return new List<Claim>()
            {
                new ("yearId", companyInformation.Id.ToString())
            };
        }

        public async Task<Year> CurrentYear(int userId, CancellationToken cancellationToken)
        {
            var yearSetting = await _repository.Find<Data.Databases.Entities.UserSetting>(x =>
                    x.ConditionExpression(x => x.UserId == userId && x.Keyword == "LastYearId"))
                .FirstOrDefaultAsync(cancellationToken);
            if (yearSetting == null)
            {
                var userYear = await _repository.Find<UserYear>(x =>
                        x.ConditionExpression(x => x.UserId == userId))
                    .FirstOrDefaultAsync(cancellationToken);
                _repository.Insert(new Data.Databases.Entities.UserSetting()
                {
                    Keyword = "LastYearId",
                    Value = userYear.YearId.ToString(),
                    UserId = userId,
                });
                await _repository.SaveChangesAsync(cancellationToken);
                return await _repository.Find<Year>(x =>
                        x.ObjectId(userYear.YearId))
                    .FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                return await _repository.Find<Year>(x =>
                        x.ObjectId(int.Parse(yearSetting.Value)))
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }

        public async Task<Year> ChangeCurrentYear(int userId, int newYearId, CancellationToken cancellationToken)
        {
            var currentRole = await _repository.Find<Data.Databases.Entities.UserSetting>(x =>
                x.ConditionExpression(x => x.UserId == userId && x.Keyword == "LastYearId")).FirstOrDefaultAsync(cancellationToken);

            if (currentRole == null)
            {
                _repository.Insert(new Data.Databases.Entities.UserSetting()
                {
                    Keyword = "LastYearId",
                    Value = newYearId.ToString(),
                    UserId = userId,
                });
            }
            else
            {
                currentRole.Value = newYearId.ToString();
                _repository.Update(currentRole);
            }
            await _repository.SaveChangesAsync(cancellationToken);

            return await _repository.Find<Year>(x =>
                    x.ObjectId(newYearId))
                .FirstAsync(cancellationToken);
        }





        public List<Claim> AddLanguageClaims(Language language)
        {
            return new List<Claim>()
            {
                new ("languageId", language.Id.ToString()),
                new ("cultureTwoIsoName", language.SeoCode??"fa")
            };
        }

        public async Task<Language> CurrentLanguage(int userId, CancellationToken cancellationToken)
        {
            var languageSetting = await _repository.Find<Data.Databases.Entities.UserSetting>(x =>
                    x.ConditionExpression(x => x.UserId == userId && x.Keyword == "LastLanguageId"))
                .FirstOrDefaultAsync(cancellationToken);
            if (languageSetting == null)
            {
                var language = await _repository.GetAll<Language>()
                    .FirstOrDefaultAsync(cancellationToken);
                _repository.Insert(new Data.Databases.Entities.UserSetting()
                {
                    Keyword = "LastYearId",
                    Value = language.Id.ToString(),
                    UserId = userId,
                });
                await _repository.SaveChangesAsync(cancellationToken);
                return language;
            }
            else
            {
                return await _repository.Find<Language>(x =>
                        x.ObjectId(int.Parse(languageSetting.Value)))
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }

        public async Task<Language> ChangeCurrentLanguage(int userId, int newLanguageId, CancellationToken cancellationToken)
        {
            var currentLanguage = await _repository.Find<Data.Databases.Entities.UserSetting>(x =>
                x.ConditionExpression(x => x.UserId == userId && x.Keyword == "LastLanguageId")).FirstOrDefaultAsync(cancellationToken);

            if (currentLanguage == null)
            {
                _repository.Insert(new Data.Databases.Entities.UserSetting()
                {
                    Keyword = "LastLanguageId",
                    Value = newLanguageId.ToString(),
                    UserId = userId,
                });
            }
            else
            {
                currentLanguage.Value = newLanguageId.ToString();
                _repository.Update(currentLanguage);
            }
            await _repository.SaveChangesAsync(cancellationToken);

            return await _repository.Find<Language>(x =>
                    x.ObjectId(newLanguageId))
                .FirstAsync(cancellationToken);
        }


        public async Task<List<Claim>> ChangeUserSetting(List<Claim> claims,IdentityModel identityModel, Data.Databases.Entities.User user, CancellationToken cancellationToken)
        {
            if (identityModel.RoleId is not 0)
            {
                var currentRole = await ChangeCurrentRole(user.Id, identityModel.RoleId, cancellationToken);
                claims.AddRange(AddRoleClaims(currentRole));
            }
            else
            {
                var currentRole = await CurrentRole(user.Id, cancellationToken);
                claims.AddRange(AddRoleClaims(currentRole));
            }

            if (identityModel.CompanyId is not 0)
            {
                var currentCompany = await ChangeCurrentCompany(user.Id, identityModel.CompanyId, cancellationToken);
                claims.AddRange(AddCompanyClaims(currentCompany));
            }
            else
            {
                var currentCompany = await CurrentCompany(user.Id, cancellationToken);
                claims.AddRange(AddCompanyClaims(currentCompany));
            }

            if (identityModel.YearId is not 0)
            {
                var currentYear = await ChangeCurrentYear(user.Id, identityModel.YearId, cancellationToken);
                claims.AddRange(AddYearClaims(currentYear));
            }
            else
            {
                var currentYear = await CurrentYear(user.Id, cancellationToken);
                claims.AddRange(AddYearClaims(currentYear));

            }

            if (identityModel.LanguageId is not 0)
            {
                var currentLanguage = await ChangeCurrentLanguage(user.Id, identityModel.LanguageId, cancellationToken);
                claims.AddRange(AddLanguageClaims(currentLanguage));

            }
            else
            {
                var currentLanguage = await CurrentLanguage(user.Id, cancellationToken);
                claims.AddRange(AddLanguageClaims(currentLanguage));
            }

            return claims;
        }


        public async Task ChangeUserSetting(IdentityModel identityModel, CancellationToken cancellationToken)
        {
            if (identityModel.RoleId is not 0)
            {
                var currentRole = await ChangeCurrentRole(identityModel.Id, identityModel.RoleId, cancellationToken);
            }
            else
            {
                var currentRole = await CurrentRole(identityModel.Id, cancellationToken);
            }

            if (identityModel.CompanyId is not 0)
            {
                var currentCompany = await ChangeCurrentCompany(identityModel.Id, identityModel.CompanyId, cancellationToken);
            }
            else
            {
                var currentCompany = await CurrentCompany(identityModel.Id, cancellationToken);
            }

            if (identityModel.YearId is not 0)
            {
                var currentYear = await ChangeCurrentYear(identityModel.Id, identityModel.YearId, cancellationToken);
            }
            else
            {
                var currentYear = await CurrentYear(identityModel.Id, cancellationToken);

            }

            if (identityModel.LanguageId is not 0)
            {
                var currentLanguage = await ChangeCurrentLanguage(identityModel.Id, identityModel.LanguageId, cancellationToken);

            }
            else
            {
                var currentLanguage = await CurrentLanguage(identityModel.Id, cancellationToken);
            }
        }
    }
}