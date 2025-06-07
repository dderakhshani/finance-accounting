using Eefa.Identity.Data.Databases.Entities;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Language = Eefa.Identity.Data.Databases.Entities.Language;

namespace Eefa.Identity.Data.Databases.SqlServer.Context
{
    public interface IIdentityUnitOfWork : IUnitOfWork
    {
        public DbSet<BaseValue> BaseValues { get; set; }
        public DbSet<BaseValueType> BaseValueTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<CompanyInformation> CompanyInformations { get; set; }
        public DbSet<CountryDivision> CountryDivisions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HelpAttachment> HelpAttachments { get; set; }
        public DbSet<HelpData> HelpDatas { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<RequiredPermission> RequiredPermissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitPosition> UnitPositions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<UserYear> UserYears { get; set; }
        public DbSet<ValidationMessage> ValidationMessages { get; set; }
        public DbSet<Year> Years { get; set; }
    }
}