using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

public interface IUnitOfWork
{
    IRepository<AccountReference> AccountReferences { get; }
    IRepository<AccountReferencesGroup> AccountReferencesGroups { get; }
    IRepository<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; }
    IRepository<BaseValue> BaseValues { get; }
    IRepository<Attachment> Attachments { get; }
    IRepository<BaseValueType> BaseValueTyps { get; }
    IRepository<Branch> Branchs { get; }
    IRepository<CompanyInformation> CompanyInformations { get; }
    IRepository<CorrectionRequest> CorrectionRequests { get; }
    IRepository<CountryDivision> CountryDivisions { get; }
    IRepository<DataBaseMetadata> DataBaseMetadatas { get; }
    IRepository<Employee> Employees { get; }
    IRepository<Help> Helps { get; }
    IRepository<Language> Languages { get; }
    IRepository<MenuItem> MenuItems { get; }
    IRepository<Permission> Permissions { get; }
    IRepository<Person> Persons { get; }
    IRepository<PersonAddress> PersonsAddress { get; }
    IRepository<PersonBankAccount> PersonBankAccounts { get; }
    IRepository<PersonFingerprint> PersonFingerprints { get; }
    IRepository<PersonPhone> PersonPhones { get; }
    IRepository<Customer> Customers { get; }
    IRepository<Position> Positions { get; }
    IRepository<Role> Roles { get; }
    IRepository<RolePermission> RolePermissions { get; }
    IRepository<Unit> Units { get; }
    IRepository<UnitPosition> UnitPositions { get; }
    IRepository<User> Users { get; }
    IRepository<UserRole> UserRoles { get;}
    IRepository<UserYear> UsersYears { get; }
    IRepository<UserCompany> UsersCompanys { get; }
    IRepository<Year> Years { get; }


    int ExecuteSqlCommand(string sql, params object[] parameters);
    Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);
    Task<List<TResult>> ExecuteSqlCommandAsync<TResult>(string query, object[] parameters,
     CancellationToken cancellationToken) where TResult : class;
    // Context Actions
    void ResetContextState();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task RollBackAsync(CancellationToken cancellationToken = default);
}