using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Identity.Data.Databases.Entities;
using Eefa.Identity.Services.Identity;
using Eefa.Identity.Services.Interfaces;
using Eefa.Identity.Services.User.Models;
using Library.Interfaces;
using Library.Utility;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Identity.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;
        private readonly IUserSettingsAccessor _userSettingsAccessor;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UserService(IRepository repository, IUserSettingsAccessor userSettingsAccessor, ICurrentUserAccessor currentUserAccessor)
        {
            _repository = repository;
            _userSettingsAccessor = userSettingsAccessor;
            this._currentUserAccessor = currentUserAccessor;
        }

        public async Task<Data.Databases.Entities.User> GetUserByUserPass(IdentityModel identityModel, CancellationToken cancellationToken)
        {
            var user = await _repository.Find<Data.Databases.Entities.User>(cfg =>
                    cfg.ConditionExpression(x => x.Username == identityModel.Username.ToLower()))
                .Include(x => x.UserRoleUsers).ThenInclude(x => x.Role)
                .Include(x => x.Person).ThenInclude(x => x.PersonAddresses)
                .FirstOrDefaultAsync(cancellationToken);

            if (user != null && user.IsBlocked)
            {
                throw new Exception("UsernameHasBeenBlocked");
            }
            if (user != null && Encryption.Symmetric.VerifyHash(user.Password, identityModel.Password))
            {
                //if (user.PasswordExpiryDate > DateTime.Now)
                //{
                    user.FailedCount = 0;
                    user.LastOnlineTime = DateTime.Now;
                    await _repository.SaveChangesAsync(cancellationToken);
                //}
                //else
                //{
                //    throw new Exception("PassworHasExpired");
                //}
            }
            else
            {
                if (user != null)
                {
                    user.FailedCount++;

                    if (user.FailedCount > 3)
                    {
                        user.IsBlocked = true;
                        user.BlockedReasonBaseId = 1;
                        _repository.Update<Data.Databases.Entities.User>(user);
                    }
                }
            }
            await _repository.SaveChangesAsync(cancellationToken);

            if (user == null || user is { FailedCount: > 0 })
            {
                throw new Exception("UsernameOrPasswordIncorrect");
            }

            return user;
        }

        public async Task<Data.Databases.Entities.User> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await _repository.Find<Data.Databases.Entities.User>(cfg =>
                    cfg.ObjectId(id))
                .Include(x => x.Person)
                .ThenInclude(x => x.PersonAddresses)
                .Include(x => x.UserRoleUsers)
                .Include(x => x.UserYearUsers)
                .Include(x => x.UserSettingUsers)
                .FirstOrDefaultAsync(cancellationToken);
            user.LastOnlineTime = DateTime.Now;
            await _repository.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<List<Claim>> GetClaims(IdentityModel identityModel, Data.Databases.Entities.User user, CancellationToken cancellationToken)
        {
            var permisions = await (from userrole in _repository.GetQuery<UserRole>()
                                    join role in _repository.GetQuery<Role>()
                                        on userrole.RoleId equals role.Id
                                    join rolepermission in _repository.GetQuery<RolePermission>()
                                        on role.Id equals rolepermission.RoleId
                                    join permission in _repository.GetQuery<Permission>()
                                        on rolepermission.PermissionId equals permission.Id
                                    where userrole.UserId == identityModel.Id
                                    select new { permission.Id }).Distinct().ToListAsync(cancellationToken);


            var userpermisions = await (
                from userper in _repository.GetQuery<UserPermission>()
                join permission in _repository.GetQuery<Permission>()
                                        on userper.PermissionId equals permission.Id
                where userper.UserId == identityModel.Id
                select new { permission.Id}
            ).Distinct().ToListAsync(cancellationToken);
            var branch = await (
                    from e in _repository.GetQuery<Employee>()
                    join up in _repository.GetQuery<UnitPosition>()
                        on e.UnitPositionId equals up.Id
                    join u in _repository.GetQuery<Unit>()
                    on up.UnitId equals u.Id
                    join b in _repository.GetQuery<Branch>()
                        on u.BranchId equals b.Id
                    where e.PersonId == user.PersonId
                    select new { b }).FirstOrDefaultAsync(cancellationToken);

            var gender =
                (await _repository.Find<BaseValue>(cfg => cfg.ObjectId(user.Person.GenderBaseId))
                    .FirstOrDefaultAsync(cancellationToken)).Title;

            var claims = new List<Claim>
            {
                new("id", user.Id.ToString()),
                new("username", user.Username),
                new("fullName", user.Person.FirstName + " " + user.Person.LastName),
                new("gender", gender),
                new("userAvatarReletiveAddress", user?.Person?.PhotoURL ?? ""),
                new("branchId", branch?.b?.Id.ToString() ?? ""),
                new("accountReferenceId", user?.Person?.AccountReferenceId?.ToString() ?? ""),
            };
            //var per = (from s in permisions.Select(x => x.Id) select new Claim("permission", "role_" + s.ToString())).ToList();
            //per.AddRange((from s in userpermisions.Select(x => x.Id) select new Claim("permission", "user_" + s.ToString())).ToList());
            //claims.AddRange(per);
            claims = await _userSettingsAccessor.ChangeUserSetting(claims, identityModel, user, cancellationToken);
            return claims;
        }



        public async Task<Dictionary<string, List<Claim>>> GetClaims2(IdentityModel identityModel, Data.Databases.Entities.User user, CancellationToken cancellationToken)
        {
            var temp = await (
                from userrole in _repository.GetQuery<UserRole>()
                join role in _repository.GetQuery<Role>()
                    on userrole.RoleId equals role.Id
                join rolepermission in _repository.GetQuery<RolePermission>()
                    on role.Id equals rolepermission.RoleId
                join permission in _repository.GetQuery<Permission>()
                    on rolepermission.PermissionId equals permission.Id
                where userrole.UserId == identityModel.Id
                select new { permission.UniqueName, permission.LevelCode, permission.IsDataRowLimiter, permission.SubSystem, userrole }
            ).ToListAsync(cancellationToken);

            var branch = await (
                    from e in _repository.GetQuery<Employee>()
                    join up in _repository.GetQuery<UnitPosition>()
                        on e.UnitPositionId equals up.Id
                    join u in _repository.GetQuery<Unit>()
                    on up.UnitId equals u.Id
                    join b in _repository.GetQuery<Branch>()
                        on u.BranchId equals b.Id
                    where e.PersonId == user.PersonId
                    select new { b }).FirstOrDefaultAsync(cancellationToken);

            var gender =
                (await _repository.Find<BaseValue>(cfg => cfg.ObjectId(user.Person.GenderBaseId))
                    .FirstOrDefaultAsync(cancellationToken)).Title;
            var claimsdDictionary = new Dictionary<string, List<Claim>>();


            var mainClaims = new List<Claim>
            {
                new("id", user.Id.ToString()),
                new("username", user.Username),
                new("fullName", user.Person.FirstName + " " + user.Person.LastName),
                new("gender", gender),
                new("userAvatarReletiveAddress", user?.Person?.PhotoURL ?? ""),
                new("branchId", branch?.b?.Id.ToString() ?? ""),
                new("accountReferenceId", user?.Person?.AccountReferenceId?.ToString() ?? ""),
            };

            mainClaims.AddRange(from s in temp.Select(x => x.SubSystem).Distinct() select new Claim("allowedSystem", s));

            claimsdDictionary.Add("main", mainClaims);

            var settingClaims = await _userSettingsAccessor.ChangeUserSetting(new List<Claim>(), identityModel, user, cancellationToken);

            foreach (var subSystem in temp.Select(x => x.SubSystem).Distinct())
            {
                if (!claimsdDictionary.ContainsKey(subSystem))
                {
                    var claims = new List<Claim>();
                    claims.AddRange(mainClaims);
                    claims.AddRange(settingClaims);
                    claims.AddRange(from p in temp where p.SubSystem == subSystem select new Claim(ClaimTypes.Role, p.UniqueName));

                    claims.AddRange(
                        (from p in temp
                         where p.SubSystem == subSystem
                               && p.IsDataRowLimiter
                               && p.userrole.RoleId == int.Parse(settingClaims.First(x => x.Type == "roleId").Value)
                         select p.LevelCode)
                        .Distinct().Select(accessPermission => new Claim("AccessPermission", accessPermission)));
                    claimsdDictionary.Add(subSystem, claims);
                }
            }
            return claimsdDictionary;
        }


        public async Task<UserProfileModel> GetProfile(int id)
        {
            var userProfile = await _repository.GetQuery<Data.Databases.Entities.User>().Where(x => x.Id == id).Select(user => new UserProfileModel
            {
                Id = user.Id,
                FullName = (user.Person.FirstName + " " + user.Person.LastName).Trim(),
                Username = user.Username,
                Permissions = new List<UserPermissionModel>(),
                Roles = user.UserRoleUsers.Select(x => new UserRoleModel
                {
                    Id= x.Id,
                    RoleId = x.RoleId,
                    Title = x.Role.Title
                }).ToList(),
                Years = user.UserYearUsers.Select(x => new UserYearModel
                {
                    Id = x.YearId,
                    CompanyId = x.Year.CompanyId,
                    FirstDate = x.Year.FirstDate,
                    LastDate = x.Year.LastDate,
                    IsCurrentYear = x.Year.IsCurrentYear,
                    IsEditable = x.Year.IsEditable,
                    YearName = x.Year.YearName,
                    LastEditableDate = x.Year.LastEditableDate
                }).ToList(),
                Companies = user.UserCompanyUsers.Select(x => new UserCompanyModel
                {
                    Id = x.CompanyInformationsId,
                    Title = x.CompanyInformations.Title
                }).ToList(),
         
            }).FirstOrDefaultAsync();
            userProfile.Permissions = await (
                 from userrole in _repository.GetQuery<Data.Databases.Entities.UserRole>()
                 join rolepermission in _repository.GetQuery<Data.Databases.Entities.RolePermission>()
                     on userrole.RoleId equals rolepermission.RoleId
                 join permission in _repository.GetQuery<Data.Databases.Entities.Permission>()
                     on rolepermission.PermissionId equals permission.Id
                 where userrole.UserId == _currentUserAccessor.GetId()
                 select new UserPermissionModel
                 {
                     Id = permission.Id,
                     Title = permission.Title,
                     LevelCode = permission.LevelCode,
                     ParentId = permission.ParentId,
                     UniqueName = permission.UniqueName
                 }).Distinct().ToListAsync();

            userProfile.Menus = await (
                    from userrole in _repository.GetQuery<Data.Databases.Entities.UserRole>()
                    join rolepermission in _repository.GetQuery<Data.Databases.Entities.RolePermission>()
                        on userrole.RoleId equals rolepermission.RoleId
                    join permission in _repository.GetQuery<Data.Databases.Entities.Permission>()
                        on rolepermission.PermissionId equals permission.Id
                    join m in _repository.GetQuery<Data.Databases.Entities.MenuItem>()
                        on permission.Id equals m.PermissionId
                    where  m.IsActive && userrole.UserId == _currentUserAccessor.GetId()
                    orderby m.OrderIndex ascending
                    select new UserMenuModel { 
                        Id = m.Id,
                        FormUrl = m.FormUrl,
                        ImageUrl = m.ImageUrl,
                        ParentId = m.ParentId,
                        PermissionId = m.PermissionId,
                        Title = m.Title
                    }).Distinct().ToListAsync();
            return userProfile;
        }
    }
}