using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// TODO Handler class have to chack...
public class AutoVoucherCommand : IRequest<ServiceResult<List<VouchersDetail>>>
{
    public int VoucherHeadId { get; set; }
    public List<dynamic> DataList { get; set; }
}

//public class CreateAutoVoucherCommandHandler : IRequestHandler<AutoVoucherCommand, ServiceResult<List<VouchersDetail>>>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IApplicationUser _applicationUser;
//    private readonly IVoucherHeadCacheServices _voucherHeadCacheServices;

//    private List<AccountHead> AccountHeads { get; set; }
//    private List<AutoVoucherFormula> Formulas { get; set; }

//    public CreateAutoVoucherCommandHandler(IUnitOfWork unitOfWork, IApplicationUser applicationUser, IVoucherHeadCacheServices voucherHeadCacheServices)
//    {
//        _unitOfWork = unitOfWork;
//        _voucherHeadCacheServices = voucherHeadCacheServices;
//        _applicationUser = applicationUser;
//    }
//    public async Task<ServiceResult<List<VouchersDetail>>> Handle(AutoVoucherCommand request, CancellationToken cancellationToken)
//    {
//        //List<dynamic> dynamicData = JsonConvert.DeserializeObject<List<dynamic>>(JsonConvert.SerializeObject(request.DataList));
//        List<VouchersHead> voucherHeads = new List<VouchersHead>();
//        await CheckIfProvidedDatesAreValid(request.DataList);

//        var GroupedDataList = GroupDataListByDateAndCodeVoucherGroupId(request.DataList);
//        if (GroupedDataList?.Count > 1 && request.VoucherHeadId != default) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "امکان ویرایش چند سند مکانیزه به صورت همزمان وجود ندارد." });
//        Formulas = await GetAllRequiredFormulas(GroupedDataList);
//        AccountHeads = await _unitOfWork.AccountHeads.GetListAsync();
//        // Generate VoucherHeads
//        foreach (var groupedDataDictionary in GroupedDataList)
//        {
//            var groupedData = groupedDataDictionary.Value;

//            VouchersHead voucherHead;
//            if (request.VoucherHeadId != default)
//            {
//                voucherHead = await _unitOfWork.VouchersHeads.GetByIdAsync(request.VoucherHeadId, x => x.Include(y => y.VouchersDetails));
//                if (voucherHead.VoucherStateId != 1) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "سند مکانیزه ای که قصد ویرایش آن را دارید در وضعیت دائم می باشد." });
//                _unitOfWork.VouchersHeads.Update(voucherHead);
//            }
//            else
//            {
//                voucherHead = await GenerateVoucherHead(groupedData.FirstOrDefault());
//            }

//            voucherHeads.Add(voucherHead);

//            var groupFormulas = Formulas.Where(x => x.VoucherTypeId == voucherHead.CodeVoucherGroupId).ToList();
//            foreach (var data in groupedData)
//            {
//                int? documentId = data?.DocumentId ?? null;
//                var dupplicatedDetailsInAnotherVoucherHeadNumber = 0;
//                if (voucherHead.Id != default)
//                {
//                    if (documentId != null)
//                    {
//                        dupplicatedDetailsInAnotherVoucherHeadNumber = await _unitOfWork.VouchersDetails.GetAsync(x => !x.IsDeleted && x.DocumentId == documentId && x.Voucher.CodeVoucherGroupId == voucherHead.CodeVoucherGroupId && x.VoucherId != voucherHead.Id, y => y.Select(z => z.Voucher.VoucherNo));
//                        var voucherDetailsToRemove = voucherHead.VouchersDetails.Where(x => x.DocumentId == documentId).ToList();
//                        foreach (var voucherDetail in voucherDetailsToRemove)
//                        {
//                            _unitOfWork.VouchersDetails.Delete(voucherDetail);
//                        }
//                    }
//                    if (dupplicatedDetailsInAnotherVoucherHeadNumber != default) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"شماره: " + documentId + " در سند حسابداری " + dupplicatedDetailsInAnotherVoucherHeadNumber + " قبلا ثبت شده است." });
//                }
//                else
//                {
//                    dupplicatedDetailsInAnotherVoucherHeadNumber = await _unitOfWork.VouchersDetails.GetAsync(x => !x.IsDeleted && x.DocumentId == documentId && x.Voucher.CodeVoucherGroupId == voucherHead.CodeVoucherGroupId, y => y.Select(z => z.Voucher.VoucherNo));
//                    if (dupplicatedDetailsInAnotherVoucherHeadNumber != default) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"شماره: " + documentId + " در سند حسابداری " + dupplicatedDetailsInAnotherVoucherHeadNumber + " قبلا ثبت شده است." });
//                }

//                foreach (var formula in groupFormulas)
//                {
//                    // Generete VoucherDetails
//                    if (IsVoucherDetailGenerationAllowed(data, formula))
//                    {
//                        VouchersDetail voucherDetail = await this.GenerateVoucherDetail(data, formula);
//                        if (voucherDetail.RowIndex == default) voucherDetail.RowIndex = (voucherHead.VouchersDetails.Max(x => x.RowIndex) ?? 0) + 1;
//                        if (voucherDetail.AccountHeadId == default) voucherDetail.AccountHeadId = formula.AccountHeadId;
//                        if (voucherDetail.VoucherDate == default) voucherDetail.VoucherDate = voucherHead.VoucherDate;

//                        SetVoucherDetailAccountHeads(voucherDetail);
//                        //await ValidateVoucherDetailAccountsRelations(voucherDetail);

//                        voucherHead.VouchersDetails.Add(voucherDetail);
//                    }
//                }
//            }
//            voucherHead.TotalCredit = voucherHead.VouchersDetails.Where(x => !x.IsDeleted).Sum(x => x.Credit);
//            voucherHead.TotalDebit = voucherHead.VouchersDetails.Where(x => !x.IsDeleted).Sum(x => x.Debit);
//            //if (!IsVoucherHeadBalanced(voucherHead)) throw new ApplicationValidationException(new ApplicationErrorModel { Message= "امکان ثبت سند مکانیزه غیرموازنه وجود ندارد."});
//        }

//        await AssignVouchersNumbers(voucherHeads);

//        await _unitOfWork.SaveChangesAsync();

//        var response = await _unitOfWork.VouchersDetails
//                .GetListAsync(x => voucherHeads.Select(y => y.Id).Contains(x.VoucherId),
//                              x => x.Select(y => new AutoVoucherDetailResult
//                              {
//                                  VoucherHeadId = y.VoucherId,
//                                  VoucherNo = y.Voucher.VoucherNo,
//                                  DocumentId = y.DocumentId
//                              })
//                                   .Distinct());

//        return ServiceResult.Success(response);
//    }

//    public class AutoVoucherDetailResult
//    {
//        public int VoucherHeadId { get; set; }
//        public int? DocumentId { get; set; }
//        public int VoucherNo { get; set; }
//    }

//    private async Task<bool> CheckIfProvidedDatesAreValid(List<dynamic> dataList)
//    {
//        // Check if year is active or allowed to be editted (and till when) and documentDate is valid within the year
//        // Check if documentDate is not in holidays
//        return true;
//    }
//    private IDictionary<int, List<dynamic>> GroupDataListByDateAndCodeVoucherGroupId(List<dynamic> dataList)
//    {
//        var dictionary = new Dictionary<int, List<dynamic>>();

//        var groupedData = dataList.GroupBy(x => ((x.CodeVoucherGroupId.ToString() + Convert.ToDateTime(Convert.ToDateTime(x.DocumentDate).ToShortDateString()).ToUniversalTime().ToString()).GetHashCode()));
//        foreach (var group in groupedData)
//        {
//            dictionary.Add(group.Key, group.ToList());
//        }

//        return dictionary;
//    }
//    private async Task<List<AutoVoucherFormula>> GetAllRequiredFormulas(IDictionary<int, List<dynamic>> groupedDataList)
//    {
//        List<int> codeVoucherGroupIds = groupedDataList.Select(x => (int)(x.Value.FirstOrDefault().CodeVoucherGroupId)).Distinct().ToList();
//        var formulas = await this._unitOfWork.AutoVoucherFormulas.GetListAsync(x => codeVoucherGroupIds.Contains(x.VoucherTypeId));
//        foreach (var id in codeVoucherGroupIds)
//        {
//            if (formulas.Where(x => x.VoucherTypeId == id).ToList().Count == 0)
//            {
//                throw new ApplicationValidationException(new ApplicationErrorModel { Message = " فرمولی برای نوع سند " + id + " یافت نشد." });
//            }
//        }
//        return formulas;
//    }
//    private async Task<VouchersHead> GenerateVoucherHead(dynamic data)
//    {


//        var codeVoucherGroup = await _unitOfWork.CodeVoucherGroups.GetByIdAsync(data.CodeVoucherGroupId);
//        DateTime documentDate = Convert.ToDateTime(data.DocumentDate);

//        var voucherHead = await _unitOfWork.VouchersHeads
//            .GetAsync(x => x.VoucherDate == documentDate &&
//                           x.YearId == _applicationUser.YearId &&
//                           x.VoucherStateId == 1 &&
//                           x.CodeVoucherGroupId == codeVoucherGroup.Id,
//                           x => x.Include(y => y.VouchersDetails));
//        if (voucherHead == null)
//        {
//            var persianDateString = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(documentDate.Kind == DateTimeKind.Utc ? documentDate : documentDate.ToUniversalTime(), "Iran Standard Time").ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));
//            voucherHead = new VouchersHead
//            {
//                CompanyId = _applicationUser.CompanyId,
//                YearId = _applicationUser.YearId,
//                CodeVoucherGroupId = data.CodeVoucherGroupId,
//                VoucherStateId = 1,
//                VoucherDate = data.DocumentDate,
//                VoucherDescription = $"سند مکانیزه {codeVoucherGroup.Title} مورخ {persianDateString}",
//                VouchersDetails = new List<VouchersDetail>()
//            };
//            _unitOfWork.VouchersHeads.Add(voucherHead);
//        }
//        else
//        {
//            _unitOfWork.VouchersHeads.Update(voucherHead);
//        }
//        return voucherHead;
//    }
//    private async Task<VouchersDetail> GenerateVoucherDetail(dynamic data, AutoVoucherFormula formula)
//    {
//        var voucherDetail = new VouchersDetail();
//        List<FormulasModel.Formula> voucherDetailFormulas = JsonConvert.DeserializeObject<List<FormulasModel.Formula>>(formula.Formula);

//        foreach (var voucherDetailFormula in voucherDetailFormulas)
//        {

//            var destinationPropertyInfo = voucherDetail.GetType().GetProperties().FirstOrDefault(x => x.Name == voucherDetailFormula.Property);
//            var destinationPropertyType = destinationPropertyInfo.PropertyType;
//            if (destinationPropertyType.IsGenericType && destinationPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) destinationPropertyType = Nullable.GetUnderlyingType(destinationPropertyType);


//            string newValue = voucherDetailFormula.Value.Text.ToString().Trim();
//            var propertyIndex = 1;
//            foreach (var property in voucherDetailFormula.Value.Properties)
//            {
//                if (destinationPropertyType != typeof(DateTime) && property.Name.ToLower().Contains("date"))
//                    newValue = newValue.Replace("[" + propertyIndex + "]", Convert.ToDateTime(data[property.Name]).ToString("yyyy/MM/dd", new CultureInfo("fa-IR")));
//                else
//                    newValue = newValue.Replace("[" + propertyIndex + "]", data[property.Name].ToString());

//                propertyIndex++;
//            }
//            if (!string.IsNullOrEmpty(newValue?.ToString()))
//            {
//                destinationPropertyInfo.SetValue(voucherDetail,
//                           Convert.ChangeType(newValue, destinationPropertyType),
//                           null);
//            }
//        }

//        _unitOfWork.VouchersDetails.Add(voucherDetail);
//        return voucherDetail;
//    }
//    private bool IsVoucherDetailGenerationAllowed(dynamic data, AutoVoucherFormula formula)
//    {
//        if (formula.Conditions != null)
//        {
//            List<FormulasModel.FormulaCondition> formulaConditions = JsonConvert.DeserializeObject<List<FormulasModel.FormulaCondition>>(formula.Conditions);
//            foreach (var condition in formulaConditions)
//            {
//                var expression = condition.Expression.Replace("[startf]", "").Replace("[endf]", "");
//                var propertyIndex = 1;
//                foreach (var property in condition.Properties)
//                {
//                    expression = expression.Replace("[" + propertyIndex + "]", data[property.Name].ToString());
//                    propertyIndex++;
//                }
//                DataTable table = new DataTable();
//                table.Columns.Add("expression", typeof(bool));
//                DataRow row = table.NewRow();
//                row["expression"] = Convert.ToBoolean((object)table.Compute(expression, ""));
//                bool result = (bool)row["expression"];

//                if (result == false) return false;
//            }
//        }
//        return true;
//    }
//    private void SetVoucherDetailAccountHeads(VouchersDetail voucherDetail)
//    {
//        AccountHead level3 = null;
//        AccountHead level2 = null;
//        AccountHead level1 = null;

//        level3 = AccountHeads.FirstOrDefault(x => x.Id == voucherDetail.AccountHeadId);
//        if (level3 != null && level3.ParentId != null) level2 = AccountHeads.FirstOrDefault(x => x.Id == level3.ParentId);
//        if (level2 != null && level2.ParentId != null) level1 = AccountHeads.FirstOrDefault(x => x.Id == level2.ParentId);

//        if (level1 == null || level2 == null || level3 == null)
//        {
//            throw new ApplicationValidationException(new ApplicationErrorModel { Message = "در شماره:  " + (voucherDetail.RowIndex != default ? voucherDetail.RowIndex + " / " : "") + voucherDetail.DocumentId + " سطوح حساب معتبر نیست." });
//        }

//        voucherDetail.Level1 = level1.Id;
//        voucherDetail.Level2 = level2.Id;
//        voucherDetail.Level3 = level3.Id;
//    }
//    private async Task ValidateVoucherDetailAccountsRelations(VouchersDetail voucherDetail)
//    {
//        if (voucherDetail.AccountReferencesGroupId != default && voucherDetail.AccountReferencesGroupId != null)
//        {
//            var areGroupAndAccountHeadRelated = await _unitOfWork.AccountHeadRelReferenceGroups.ExistsAsync(x => !x.IsDeleted && x.AccountHeadId == voucherDetail.AccountHeadId && x.ReferenceGroupId == voucherDetail.AccountReferencesGroupId);
//            if (!areGroupAndAccountHeadRelated)
//                throw new ApplicationValidationException(new ApplicationErrorModel { Message = "در شماره: " + (voucherDetail.RowIndex != default ? voucherDetail.RowIndex + " / " : "") + voucherDetail.DocumentId + " ارتباط حساب و گروه معتبر نیست." });
//        }


//        if (voucherDetail.ReferenceId1 != default && voucherDetail.ReferenceId1 != null)
//        {
//            var areGroupAndReferenceRelated = await _unitOfWork.AccountReferencesRelReferencesGroups.ExistsAsync(x => !x.IsDeleted && x.ReferenceId == voucherDetail.ReferenceId1 && x.ReferenceGroupId == voucherDetail.AccountReferencesGroupId);
//            if (!areGroupAndReferenceRelated)
//                throw new ApplicationValidationException(new ApplicationErrorModel { Message = "در شماره: " + (voucherDetail.RowIndex != default ? voucherDetail.RowIndex + " / " : "") + voucherDetail.DocumentId + " .ارتباط تفصیل شناور و گروه معتبر نیست" });
//        }

//    }
//    private bool IsVoucherHeadBalanced(VouchersHead voucherHead)
//    {
//        return voucherHead.TotalCredit == voucherHead.TotalCredit;
//    }
//    private async Task AssignVouchersNumbers(List<VouchersHead> voucherHeads)
//    {
//        foreach (var voucherHead in voucherHeads.Where(x => x.Id == default).ToList())
//        {
//            voucherHead.VoucherNo = await _voucherHeadCacheServices.GetNewVoucherNumber();
//        }
//    }
//}