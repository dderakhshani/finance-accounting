using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;
    private readonly IServiceProvider serviceProvider;
    public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        this.context = context;
        this.serviceProvider = serviceProvider;
    }

    public IRepository<AccountReference> AccountReferences => serviceProvider.GetService<IRepository<AccountReference>>();
    public IRepository<BaseValue> BaseValues => serviceProvider.GetService<IRepository<BaseValue>>();
    public IRepository<Attachment> Attachments => serviceProvider.GetService<IRepository<Attachment>>();
    public IRepository<BaseValueType> BaseValueTyps => serviceProvider.GetService<IRepository<BaseValueType>>();
    public IRepository<Branch> Branchs => serviceProvider.GetService<IRepository<Branch>>();
    public IRepository<CompanyInformation> CompanyInformations => serviceProvider.GetService<IRepository<CompanyInformation>>();
    public IRepository<CorrectionRequest> CorrectionRequests => serviceProvider.GetService<IRepository<CorrectionRequest>>();
    public IRepository<CountryDivision> CountryDivisions => serviceProvider.GetService<IRepository<CountryDivision>>();
    public IRepository<DataBaseMetadata> DataBaseMetadatas => serviceProvider.GetService<IRepository<DataBaseMetadata>>();
    public IRepository<Employee> Employees => serviceProvider.GetService<IRepository<Employee>>();
    public IRepository<Help> Helps => serviceProvider.GetService<IRepository<Help>>();
    public IRepository<Language> Languages => serviceProvider.GetService<IRepository<Language>>();
    public IRepository<MenuItem> MenuItems => serviceProvider.GetService<IRepository<MenuItem>>();
    public IRepository<AccountReferencesGroup> AccountReferencesGroups => serviceProvider.GetService<IRepository<AccountReferencesGroup>>();
    public IRepository<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups => serviceProvider.GetService<IRepository<AccountReferencesRelReferencesGroup>>();
    public IRepository<Permission> Permissions => serviceProvider.GetService<IRepository<Permission>>();
    public IRepository<Person> Persons => serviceProvider.GetService<IRepository<Person>>();
    public IRepository<PersonAddress> PersonsAddress => serviceProvider.GetService<IRepository<PersonAddress>>();
    public IRepository<PersonBankAccount> PersonBankAccounts => serviceProvider.GetService<IRepository<PersonBankAccount>>();
    public IRepository<PersonFingerprint> PersonFingerprints => serviceProvider.GetService<IRepository<PersonFingerprint>>();
    public IRepository<PersonPhone> PersonPhones => serviceProvider.GetService<IRepository<PersonPhone>>();
    public IRepository<Customer> Customers => serviceProvider.GetService<IRepository<Customer>>();
    public IRepository<Position> Positions => serviceProvider.GetService<IRepository<Position>>();
    public IRepository<Role> Roles => serviceProvider.GetService<IRepository<Role>>();
    public IRepository<RolePermission> RolePermissions => serviceProvider.GetService<IRepository<RolePermission>>();
    public IRepository<Unit> Units => serviceProvider.GetService<IRepository<Unit>>();
    public IRepository<UnitPosition> UnitPositions => serviceProvider.GetService<IRepository<UnitPosition>>();
    public IRepository<User> Users => serviceProvider.GetService<IRepository<User>>();
    public IRepository<UserRole> UserRoles => serviceProvider.GetService<IRepository<UserRole>>();
    public IRepository<UserYear> UsersYears => serviceProvider.GetService<IRepository<UserYear>>();
    public IRepository<UserCompany> UsersCompanys => serviceProvider.GetService<IRepository<UserCompany>>();
    public IRepository<Year> Years => serviceProvider.GetService<IRepository<Year>>();


    public int ExecuteSqlCommand(string sql, params object[] parameters) => this.context.Database.ExecuteSqlRaw(sql, parameters);
    public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters) => await this.context.Database.ExecuteSqlRawAsync(sql, parameters);
    public Task<List<TResult>> ExecuteSqlCommandAsync<TResult>(string query, object[] parameters, CancellationToken cancellationToken) where TResult : class
    {
        throw new NotImplementedException();
    }
    public void ResetContextState() => this.context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await this.context.SaveChangesAsync(cancellationToken);
    public async Task RollBackAsync(CancellationToken cancellationToken = default) => await this.context.Database.RollbackTransactionAsync(cancellationToken);
}