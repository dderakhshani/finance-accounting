using Eefa.Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
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

    public IRepository<AccountHead> AccountHeads => serviceProvider.GetService<IRepository<AccountHead>>();
    public IRepository<AccountReference> AccountReferences => serviceProvider.GetService<IRepository<AccountReference>>();
    public IRepository<AccountReferencesGroup> AccountReferencesGroups => serviceProvider.GetService<IRepository<AccountReferencesGroup>>();
    public IRepository<AutoVoucherFormula> AutoVoucherFormulas => serviceProvider.GetService<IRepository<AutoVoucherFormula>>();
    public IRepository<CodeRowDescription> CodeRowDescriptions => serviceProvider.GetService<IRepository<CodeRowDescription>>();
    public IRepository<CodeVoucherExtendType> CodeVoucherExtendTypes => serviceProvider.GetService<IRepository<CodeVoucherExtendType>>();
    public IRepository<CodeVoucherGroup> CodeVoucherGroups => serviceProvider.GetService<IRepository<CodeVoucherGroup>>();
    public IRepository<MoadianInvoiceHeader> MoadianInvoiceHeaders => serviceProvider.GetService<IRepository<MoadianInvoiceHeader>>();
    public IRepository<VouchersHead> VouchersHeads => serviceProvider.GetService<IRepository<VouchersHead>>();
    public IRepository<Year> Years => serviceProvider.GetService<IRepository<Year>>();
    public IRepository<VouchersDetail> VouchersDetails => serviceProvider.GetService<IRepository<VouchersDetail>>();
    public IRepository<BaseValue> BaseValues => serviceProvider.GetService<IRepository<BaseValue>>();
    public IRepository<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups => serviceProvider.GetService<IRepository<AccountReferencesRelReferencesGroup>>();
    public IRepository<VoucherAttachment> VoucherAttachments => serviceProvider.GetService<IRepository<VoucherAttachment>>();
    public IRepository<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups => serviceProvider.GetService<IRepository<AccountHeadRelReferenceGroup>>();
    public IRepository<MoadianInvoiceDetail> MoadianInvoiceDetails => serviceProvider.GetService<IRepository<MoadianInvoiceDetail>>();
    public IRepository<VerificationCode> VerificationCodes => serviceProvider.GetService<IRepository<VerificationCode>>();
    public IRepository<Person> Persons => serviceProvider.GetService<IRepository<Person>>();
    public IRepository<AccountHeadCloseCode> AccountHeadCloseCodes => serviceProvider.GetService<IRepository<AccountHeadCloseCode>>();





    public int ExecuteSqlCommand(string sql, params object[] parameters) => this.context.Database.ExecuteSqlRaw(sql, parameters);
    public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters) => await this.context.Database.ExecuteSqlRawAsync(sql, parameters);

    public void ResetContextState() => this.context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await this.context.SaveChangesAsync(cancellationToken);
    public async Task RollBackAsync(CancellationToken cancellationToken = default) => await this.context.Database.RollbackTransactionAsync(cancellationToken);

    //public int ExecuteSqlCommand(string sql, params object[] parameters)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
    //{
    //    throw new NotImplementedException();
    //}

    //public void ResetContextState()
    //{
    //    throw new NotImplementedException();
    //}

    //public Task RollBackAsync(CancellationToken cancellationToken = default)
    //{
    //    throw new NotImplementedException();
    //}

    //public async Task<int> SaveChancesAsync(CancellationToken cancellationToken = default)
    //{
    //    return await this.context.SaveChangesAsync(cancellationToken);
    //}

    //public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    throw new NotImplementedException();
    //}
}
