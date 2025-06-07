using System.Threading.Tasks;
using System.Threading;

public interface IUnitOfWork
{
    IRepository<AccountHead> AccountHeads { get; }
    IRepository<AccountReference> AccountReferences { get; }
    IRepository<AccountReferencesGroup> AccountReferencesGroups { get; }
    IRepository<AutoVoucherFormula> AutoVoucherFormulas { get; }
    IRepository<CodeRowDescription> CodeRowDescriptions { get; }
    IRepository<CodeVoucherExtendType> CodeVoucherExtendTypes { get; }
    IRepository<CodeVoucherGroup> CodeVoucherGroups { get; }
    IRepository<MoadianInvoiceHeader> MoadianInvoiceHeaders { get; }
    IRepository<MoadianInvoiceDetail> MoadianInvoiceDetails { get; }
    IRepository<VouchersHead> VouchersHeads { get; }
    IRepository<Year> Years { get; }
    IRepository<VouchersDetail> VouchersDetails { get; }
    IRepository<BaseValue> BaseValues { get; }
    IRepository<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; }
    IRepository<VoucherAttachment> VoucherAttachments { get; }
    IRepository<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; }
    IRepository<VerificationCode> VerificationCodes { get; }
    IRepository<Person> Persons { get; }


    int ExecuteSqlCommand(string sql, params object[] parameters);
    Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

    // Context Actions
    void ResetContextState();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task RollBackAsync(CancellationToken cancellationToken = default);


}