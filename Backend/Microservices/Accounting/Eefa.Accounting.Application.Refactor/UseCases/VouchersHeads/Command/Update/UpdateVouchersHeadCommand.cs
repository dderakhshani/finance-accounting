using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

// TODO check The Handler class
public class UpdateVouchersHeadCommand : IRequest<ServiceResult>, IMapFrom<VouchersHead>
{
    public string Key { get; set; }
    public int Id { get; set; }
    public int VoucherDailyId { get; set; }
    public int VoucherNo { get; set; } = default!;
    public DateTime VoucherDate { get; set; } = default!;
    public string VoucherDescription { get; set; } = default!;
    public int CodeVoucherGroupId { get; set; } = default!;
    public int VoucherStateId { get; set; } = default!;
    public int? AutoVoucherEnterGroup { get; set; }

    public ICollection<CreateVouchersDetailCommand> VouchersDetailsCreatedList { get; set; }
    public ICollection<UpdateVouchersDetailCommand> VouchersDetailsUpdatedList { get; set; }
    public ICollection<DeleteVouchersDetailCommand> VouchersDetailsDeletedList { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateVouchersHeadCommand, VouchersHead>()
            .IgnoreAllNonExisting();
    }
}

//public class UpdateVouchersHeadCommandHandler : IRequestHandler<UpdateVouchersHeadCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    private readonly IMediator _mediator;
//    private readonly IApplicationUser _applicationUser;

//    //private readonly AccountingUnitOfWork _context;
//    public UpdateVouchersHeadCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser, IMediator mediator/*, AccountingUnitOfWork context*/)
//    {
//        _mapper = mapper;
//        _applicationUser = applicationUser;
//        _mediator = mediator;
//        _unitOfWork= unitOfWork;
//        //_context = context;
//    }

//    public async Task<ServiceResult> Handle(UpdateVouchersHeadCommand request, CancellationToken cancellationToken)
//    {
//        VouchersHead voucher = await GetEntity(request.Id, cancellationToken);

//        bool voucherRequiresCorrectionRequestToUpdate = voucher.VoucherStateId != 1 && request.Key != "TestKey";


//        UpdateVoucher(request, voucher);

//        await CreateVoucherDetails(request, voucher);

//        await UpdateVoucherDetails(request, voucher);

//        DeleteVoucherDetails(request, voucher);

//        await ValidateVoucherTotalCreditAndTotalDebit(voucher);

//        _unitOfWork.VouchersHeads.Update(voucher);

//        if (voucherRequiresCorrectionRequestToUpdate)
//        {
//            var message = await GetChangesMessage();
//            await SubmitCorrectionRequest(request, message);
//        }
//        else
//        {
//            await _unitOfWork.SaveChangesAsync(cancellationToken);
//        }

//        return await _mediator.Send(new GetVouchersHeadQuery() { Id = request.Id }, cancellationToken);
//    }

//    private async Task<string> GetChangesMessage()
//    {
//        //var dbContext = _unitOfWork.UnitOfWork.DbContex();
//        //var entries = dbContext.ChangeTracker.Entries().Where(x => x.Entity is VouchersHead || x.Entity is VouchersDetail).ToList();

//        List<Message> messagesList = new List<Message>();

//        foreach (var change in entries)
//        {
//            var entityName = change.Entity.GetType().Name;
//            var entityKey = change.Entity is VouchersHead ? change.OriginalValues["VoucherNo"]?.ToString() : change.OriginalValues["RowIndex"]?.ToString();
//            var bannedProperties = new List<string> { "CreatedAt", "ModifiedAt", "CreatedById", "ModifiedById", "Id", "OwnerRoleId", "IsDeleted" };
//            var voucherDetailPropertiesToMention = new List<string> { "AccountHeadId", "AccountReferencesGroupId", "ReferenceId1", "VoucherRowDescription", "Credit", "Debit" };

//            var message = new Message
//            {
//                EntityKey = entityKey,
//                EntityName = entityName,
//                IsMain = change.Entity is VouchersHead
//            };
//            messagesList.Add(message);

//            var isEntityDeleted = bool.Parse(change.CurrentValues["IsDeleted"]?.ToString());
//            if (change.State == EntityState.Modified && !isEntityDeleted)
//            {
//                foreach (var prop in change.OriginalValues.Properties.Where(x => !bannedProperties.Any(y => y.Equals(x.Name, StringComparison.OrdinalIgnoreCase))).ToList())
//                {
//                    var originalValue = change.OriginalValues[prop.Name]?.ToString();
//                    var currentValue = change.CurrentValues[prop.Name]?.ToString();
//                    if (originalValue != currentValue &&
//                        (
//                        !(string.IsNullOrEmpty(originalValue) && currentValue == "0")
//                        ||
//                        (originalValue == "0" && string.IsNullOrEmpty(currentValue))
//                        )
//                        )
//                    {
//                        message.State = EntityState.Modified;
//                        message.Changes.Add(new ChangesModel
//                        {
//                            PropertyName = prop.Name,
//                            From = originalValue,
//                            To = currentValue
//                        });
//                    }
//                }
//            }

//            if (change.State == EntityState.Modified && isEntityDeleted)
//            {
//                message.State = EntityState.Deleted;
//                foreach (var prop in change.OriginalValues.Properties.Where(x => voucherDetailPropertiesToMention.Any(y => y.Equals(x.Name, StringComparison.OrdinalIgnoreCase))).ToList())
//                {
//                    var originalValue = change.OriginalValues[prop.Name]?.ToString();
//                    var currentValue = change.CurrentValues[prop.Name]?.ToString();
//                    if (currentValue != null)
//                    {
//                        message.State = EntityState.Deleted;
//                        message.Changes.Add(new ChangesModel
//                        {
//                            PropertyName = prop.Name,
//                            From = originalValue,
//                            To = currentValue
//                        });
//                    }
//                }
//            }
//            if (change.State == EntityState.Added && change.Entity is VouchersDetail)
//            {
//                message.State = EntityState.Added;
//                foreach (var prop in change.OriginalValues.Properties.Where(x => voucherDetailPropertiesToMention.Any(y => y.Equals(x.Name, StringComparison.OrdinalIgnoreCase))).ToList())
//                {
//                    var originalValue = change.OriginalValues[prop.Name]?.ToString();
//                    var currentValue = change.CurrentValues[prop.Name]?.ToString();
//                    if (!string.IsNullOrEmpty(currentValue))
//                    {
//                        message.State = EntityState.Added;
//                        message.Changes.Add(new ChangesModel
//                        {
//                            PropertyName = prop.Name,
//                            From = originalValue,
//                            To = currentValue
//                        });
//                    }
//                }
//            }
//        }
//        messagesList = messagesList.OrderBy(x => x.State).OrderByDescending(x => x.IsMain).ToList();
//        var translatedMessage = await this.GetTranslatedMessage(messagesList);

//        return translatedMessage;
//    }

//    private async Task<VouchersHead> GetEntity(int id, CancellationToken cancellationToken)
//    {
//        return await _unitOfWork.VouchersHeads.GetByIdAsync(id);
//    }
//    private async Task SubmitCorrectionRequest(UpdateVouchersHeadCommand request, string message)
//    {
//        await this._mediator.Send(new SubmitVoucherHeadCorrectionRequestCommand { Payload = request, Message = message });
//    }
//    private void UpdateVoucher(UpdateVouchersHeadCommand request, VouchersHead voucher)
//    {
//        voucher.CompanyId = _applicationUser.CompanyId;
//        voucher.YearId = _applicationUser.YearId;
//        voucher.VoucherDate = request.VoucherDate;
//        voucher.VoucherDescription = request.VoucherDescription;
//        voucher.CodeVoucherGroupId = request.CodeVoucherGroupId;
//        voucher.VoucherStateId = request.VoucherStateId;
//        voucher.AutoVoucherEnterGroup = request.AutoVoucherEnterGroup;

//        if (request.VoucherDailyId > 0) voucher.VoucherDailyId = request.VoucherDailyId;

//        _unitOfWork.VouchersHeads.Update(voucher);
//    }

//    private async Task CreateVoucherDetails(UpdateVouchersHeadCommand request, VouchersHead voucher)
//    {
//        foreach (var createVoucherDetailCommand in request.VouchersDetailsCreatedList)
//        {
//            var newVoucherDetail = _mapper.Map<VouchersDetail>(createVoucherDetailCommand);

//            await ValidateAccountsConnections(
//                createVoucherDetailCommand.AccountHeadId,
//                createVoucherDetailCommand.ReferenceId1,
//                createVoucherDetailCommand.AccountReferencesGroupId);
//            voucher.VouchersDetails.Add(newVoucherDetail);
//            _unitOfWork.VouchersDetails.Add(newVoucherDetail);
//        }
//    }
//    private async Task UpdateVoucherDetails(UpdateVouchersHeadCommand request, VouchersHead voucher)
//    {
//        foreach (var updateVoucherDetailCommand in request.VouchersDetailsUpdatedList)
//        {
//            await ValidateAccountsConnections(
//             updateVoucherDetailCommand.AccountHeadId,
//             updateVoucherDetailCommand.ReferenceId1,
//             updateVoucherDetailCommand.AccountReferencesGroupId);

//            var voucherDetailToUpdate = voucher.VouchersDetails.FirstOrDefault(x => x.Id == updateVoucherDetailCommand.Id);
//            voucherDetailToUpdate.VoucherDate = updateVoucherDetailCommand.VoucherDate;
//            voucherDetailToUpdate.AccountHeadId = updateVoucherDetailCommand.AccountHeadId;
//            voucherDetailToUpdate.AccountReferencesGroupId = updateVoucherDetailCommand.AccountReferencesGroupId;
//            voucherDetailToUpdate.VoucherRowDescription = updateVoucherDetailCommand.VoucherRowDescription;
//            voucherDetailToUpdate.Debit = updateVoucherDetailCommand.Debit;
//            voucherDetailToUpdate.Credit = updateVoucherDetailCommand.Credit;
//            voucherDetailToUpdate.RowIndex = updateVoucherDetailCommand.RowIndex;
//            voucherDetailToUpdate.DocumentId = updateVoucherDetailCommand.DocumentId;
//            voucherDetailToUpdate.ReferenceDate = updateVoucherDetailCommand.ReferenceDate;
//            voucherDetailToUpdate.Weight = updateVoucherDetailCommand.ReferenceQty;
//            voucherDetailToUpdate.ReferenceId1 = updateVoucherDetailCommand.ReferenceId1;
//            voucherDetailToUpdate.ReferenceId2 = updateVoucherDetailCommand.ReferenceId2;
//            voucherDetailToUpdate.ReferenceId3 = updateVoucherDetailCommand.ReferenceId3;
//            voucherDetailToUpdate.Level1 = updateVoucherDetailCommand.Level1;
//            voucherDetailToUpdate.Level2 = updateVoucherDetailCommand.Level2;
//            voucherDetailToUpdate.Level3 = updateVoucherDetailCommand.Level3;
//            voucherDetailToUpdate.CurrencyFee = updateVoucherDetailCommand.CurrencyFee;
//            voucherDetailToUpdate.CurrencyTypeBaseId = updateVoucherDetailCommand.CurrencyTypeBaseId;
//            voucherDetailToUpdate.CurrencyAmount = updateVoucherDetailCommand.CurrencyAmount;
//            voucherDetailToUpdate.TraceNumber = updateVoucherDetailCommand.TraceNumber;
//            voucherDetailToUpdate.Quantity = updateVoucherDetailCommand.Quantity;

//            _unitOfWork.VouchersDetails.Update(voucherDetailToUpdate);
//        }
//    }
//    private void DeleteVoucherDetails(UpdateVouchersHeadCommand request, VouchersHead voucher)
//    {
//        foreach (var deleteVoucherDetailCommand in request.VouchersDetailsDeletedList)
//        {
//            var voucherDetailToDelete = voucher.VouchersDetails.FirstOrDefault(x => x.Id == deleteVoucherDetailCommand.Id);
//            _unitOfWork.VouchersDetails.Delete(voucherDetailToDelete);
//        }
//    }

//    private async Task ValidateAccountsConnections(int accountHeadId, int? accountReferenceId, int? accountReferencesGroupId)
//    {
//        if (accountReferencesGroupId != null && accountReferencesGroupId != 0)
//        {
//            var accountHeadHasConnectionToAccountReferenceGroup = await _unitOfWork.AccountHeadRelReferenceGroups
//                .ExistsAsync(x => x.AccountHeadId == accountHeadId &&
//                    x.AccountReferencesGroup.Id == accountReferencesGroupId);

//            if (!accountHeadHasConnectionToAccountReferenceGroup) throw new Exception("AccountHead and AccountReferenceGroup are not connected");
//        }

//        if (accountReferenceId != null && accountReferenceId != 0)
//        {
//            var accountReferenceHasConnectionToAccountReferenceGroup = await _unitOfWork.AccountReferencesRelReferencesGroups
//                .ExistsAsync(x => x.ReferenceId == accountReferenceId &&
//                    x.ReferenceGroupId == accountReferencesGroupId);

//            if (!accountReferenceHasConnectionToAccountReferenceGroup) throw new Exception("AccountReference and AccountReferenceGroup are not connected");
//        }
//    }
//    private async Task ValidateVoucherTotalCreditAndTotalDebit(VouchersHead voucher)
//    {
//        voucher.TotalCredit = voucher.VouchersDetails.Where(x => !x.IsDeleted).Sum(x => x.Credit);
//        voucher.TotalDebit = voucher.VouchersDetails.Where(x => !x.IsDeleted).Sum(x => x.Debit);

//        //var accountingSettings = await new SystemSettings(_repository).Get(SubSystemType.AccountingSettings);
//        //var UnbalancedVoucherRegisterationSetting = accountingSettings.FirstOrDefault(x => x.UniqueName.Equals("UnbalancedVoucherRegisteration", StringComparison.OrdinalIgnoreCase));

//        bool voucherIsUnbalanced = voucher.TotalDebit != voucher.TotalCredit;

//        //if (bool.Parse(UnbalancedVoucherRegisterationSetting.Value) == false && voucherIsUnbalanced)
//        //    throw new UnbalancedVoucherRegisteration("UnbalancedVoucherRegisteration");
//    }

//    public string GetMetadataTitleByKey(string key)
//    {
//        var metadatas = new List<Metadata>();

//        metadatas.Add(new Metadata { Key = "VouchersHead", Title = "سند" });
//        metadatas.Add(new Metadata { Key = "Id", Title = "شناسه" });
//        metadatas.Add(new Metadata { Key = "CompanyId", Title = "کد شرکت" });
//        metadatas.Add(new Metadata { Key = "VoucherDailyId", Title = "کد سند" });
//        metadatas.Add(new Metadata { Key = "TraceNumber", Title = "شماره سند مرتبط" });
//        metadatas.Add(new Metadata { Key = "YearId", Title = "کد سال" });
//        metadatas.Add(new Metadata { Key = "VoucherNo", Title = "شماره سند" });
//        metadatas.Add(new Metadata { Key = "VoucherDate", Title = "تاریخ سند" });
//        metadatas.Add(new Metadata { Key = "VoucherDescription", Title = "شرح سند" });
//        metadatas.Add(new Metadata { Key = "CodeVoucherGroupId", Title = "کد گروه سند" });
//        metadatas.Add(new Metadata { Key = "VoucherStateId", Title = "کد وضعیت سند" });
//        metadatas.Add(new Metadata { Key = "VoucherStateName", Title = "نام وضعیت سند" });
//        metadatas.Add(new Metadata { Key = "AutoVoucherEnterGroup", Title = "گروه سند مکانیزه" });
//        metadatas.Add(new Metadata { Key = "TotalDebit", Title = "جمع بدهکار" });
//        metadatas.Add(new Metadata { Key = "TotalCredit", Title = "جمع بستانکار" });
//        metadatas.Add(new Metadata { Key = "Difference", Title = "اختلاف" });

//        metadatas.Add(new Metadata { Key = "VouchersDetail", Title = "آرتیکل" });
//        metadatas.Add(new Metadata { Key = "Id", Title = "شناسه" });
//        metadatas.Add(new Metadata { Key = "VoucherId", Title = "کد سند" });
//        metadatas.Add(new Metadata { Key = "VoucherDate", Title = "تاریخ سند" });
//        metadatas.Add(new Metadata { Key = "AccountHeadId", Title = "سرفصل حساب" });
//        metadatas.Add(new Metadata { Key = "AccountReferencesGroupId", Title = "گروه حساب" });
//        metadatas.Add(new Metadata { Key = "VoucherRowDescription", Title = "شرح آرتیکل  سند" });
//        metadatas.Add(new Metadata { Key = "Credit", Title = "بستانکار" });
//        metadatas.Add(new Metadata { Key = "Debit", Title = "بدهکار" });
//        metadatas.Add(new Metadata { Key = "RowIndex", Title = "ترتیب سطر" });
//        metadatas.Add(new Metadata { Key = "DocumentId", Title = "شماره سند مرتبط " });
//        metadatas.Add(new Metadata { Key = "DocumentNo", Title = "شماره سند مرتبط " });
//        metadatas.Add(new Metadata { Key = "DocumentIds", Title = "لیست اسناد مرتبط" });
//        metadatas.Add(new Metadata { Key = "ReferenceDate", Title = "تاریخ مرجع" });
//        metadatas.Add(new Metadata { Key = "FinancialOperationNumber", Title = "شماره فرم عملیات مالی" });
//        metadatas.Add(new Metadata { Key = "RequestNo", Title = "شماره درخواست " });
//        metadatas.Add(new Metadata { Key = "InvoiceNo", Title = "شماره فاکتور مشتری" });
//        metadatas.Add(new Metadata { Key = "Tag", Title = "تگ" });
//        metadatas.Add(new Metadata { Key = "Weight", Title = "مقدار مرجع" });
//        metadatas.Add(new Metadata { Key = "ReferenceId1", Title = "کد تفصیل شناور" });
//        metadatas.Add(new Metadata { Key = "ReferenceId2", Title = "کد مرجع2" });
//        metadatas.Add(new Metadata { Key = "ReferenceId3", Title = "کد مرجع3" });
//        metadatas.Add(new Metadata { Key = "Level1", Title = "سطح 1" });
//        metadatas.Add(new Metadata { Key = "Level2", Title = "سطح 2" });
//        metadatas.Add(new Metadata { Key = "Level3", Title = "سطح 3" });
//        metadatas.Add(new Metadata { Key = "DebitCreditStatus", Title = "وضعیت مانده حساب" });
//        metadatas.Add(new Metadata { Key = "CurrencyTypeBaseId", Title = "نوع ارز" });
//        metadatas.Add(new Metadata { Key = "CurrencyFee", Title = "نرخ ارزبه ریال" });
//        metadatas.Add(new Metadata { Key = "CurrencyAmount", Title = "مبلغ ارز" });
//        metadatas.Add(new Metadata { Key = "TraceNumber", Title = "ویژگی پیگیری دارد  " });
//        metadatas.Add(new Metadata { Key = "Quantity", Title = "مقدار ویژگی تعداد " });
//        metadatas.Add(new Metadata { Key = "Remain", Title = "باقیمانده" });

//        return metadatas.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase))?.Title;
//    }

//    public async Task<string> GetTranslatedMessage(List<Message> messages)
//    {
//        var translatedMessage = "";

//        foreach (var message in messages)
//        {
//            if (message.Changes.Count > 0)
//            {
//                translatedMessage += "***";
//            }
//            if (message.State == EntityState.Added)
//            {
//                translatedMessage += $"{GetMetadataTitleByKey(message.EntityName)} با شماره {message.EntityKey} با اطلاعات زیر اضافه شد" +
//                   "\n";

//                foreach (var change in message.Changes)
//                {
//                    translatedMessage += $"{GetMetadataTitleByKey(change.PropertyName)}:{await GetPropertyValueTitle(change.PropertyName, change.To)}" +
//                        "\n";
//                }
//            }
//            if (message.State == EntityState.Modified)
//            {
//                translatedMessage += $"در {GetMetadataTitleByKey(message.EntityName)} شماره {message.EntityKey} تغییرات زیر اعمال شده است:" +
//                 "\n";


//                foreach (var change in message.Changes)
//                {
//                    translatedMessage += $"{GetMetadataTitleByKey(change.PropertyName)} از {await GetPropertyValueTitle(change.PropertyName, change.From)} به {await GetPropertyValueTitle(change.PropertyName, change.To)} تغییر کرد" +
//                        "\n";
//                }
//            }
//            if (message.State == EntityState.Deleted)
//            {
//                translatedMessage += $"{GetMetadataTitleByKey(message.EntityName)} با شماره {message.EntityKey} با اطلاعات زیر حذف شد" +
//               "\n";
//                foreach (var change in message.Changes)
//                {
//                    translatedMessage += $"{GetMetadataTitleByKey(change.PropertyName)}:{await GetPropertyValueTitle(change.PropertyName, change.From)}" +
//                        "\n";
//                }
//            }
//        }
//        return translatedMessage;
//    }

//    public async Task<string> GetPropertyValueTitle(string propertyName, string value)
//    {
//        if (string.IsNullOrEmpty(value))
//        {
//            return "...";
//        }
//        if (propertyName.EqualsIgnoreCase("AccountHeadId"))
//        {
//            return await _unitOfWorkFind<Accounting.AccountHead>(x => x.ObjectId(value)).Select(x => x.Title).FirstOrDefaultAsync();
//        }
//        if (propertyName.EqualsIgnoreCase("AccountReferencesGroupId"))
//        {
//            return await _unitOfWorkFind<Accounting.AccountReferencesGroup>(x => x.ObjectId(value)).Select(x => x.Title).FirstOrDefaultAsync();
//        }
//        if (propertyName.EqualsIgnoreCase("ReferenceId1"))
//        {
//            return await _unitOfWorkFind<Accounting.AccountReference>(x => x.ObjectId(value)).Select(x => x.Title).FirstOrDefaultAsync();
//        }
//        if (propertyName.EqualsIgnoreCase("CodeVoucherGroupId"))
//        {
//            return await _unitOfWorkFind<Accounting.CodeVoucherGroup>(x => x.ObjectId(value)).Select(x => x.Title).FirstOrDefaultAsync();
//        }
//        return value;
//    }
//}

//public class Message
//{
//    public string EntityKey { get; set; }
//    public string EntityName { get; set; }
//    public EntityState State { get; set; }
//    public bool IsMain { get; set; }
//    public List<ChangesModel> Changes { get; set; } = new List<ChangesModel>();

//}
public class ChangesModel
{
    public string From { get; set; }
    public string To { get; set; }
    public string PropertyName { get; set; }
}

public class Metadata
{
    public string Key { get; set; }
    public string Title { get; set; }
}

public class PropertyNameToTypeMap
{
    public string PropertyName { get; set; }
    public Type Type { get; set; }
    public string TypeTitlePropertyName { get; set; }
}