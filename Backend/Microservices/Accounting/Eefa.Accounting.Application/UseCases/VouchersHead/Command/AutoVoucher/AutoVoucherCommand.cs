using Library.Interfaces;
using Library.Models;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Exceptions;
using Eefa.Accounting.Application.UseCases.VouchersHead.Services;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.CreateAutoVoucher
{
    public class AutoVoucherCommand : IRequest<ServiceResult>
    {
        public int VoucherHeadId { get; set; }
        public List<dynamic> DataList { get; set; }
    }

    public partial class CreateAutoVoucherCommandHandler : IRequestHandler<AutoVoucherCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IVoucherHeadCacheServices _voucherHeadCacheServices;

        private List<Data.Entities.AccountHead> AccountHeads { get; set; }
        private List<Data.Entities.AutoVoucherFormula> Formulas { get; set; }

        public CreateAutoVoucherCommandHandler(IRepository repository, ICurrentUserAccessor currentUser, IVoucherHeadCacheServices voucherHeadCacheServices)
        {
            _repository = repository;
            _currentUser = currentUser;
            _voucherHeadCacheServices = voucherHeadCacheServices;
        }
        public async Task<ServiceResult> Handle(AutoVoucherCommand request, CancellationToken cancellationToken)
        {
            //List<dynamic> dynamicData = JsonConvert.DeserializeObject<List<dynamic>>(JsonConvert.SerializeObject(request.DataList));
            List<Data.Entities.VouchersHead> voucherHeads = new List<Data.Entities.VouchersHead>();
            await CheckIfProvidedDatesAreValid(request.DataList);

            var GroupedDataList = GroupDataListByDateAndCodeVoucherGroupId(request.DataList);
            if (GroupedDataList?.Count > 1 && request.VoucherHeadId != default) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "امکان ویرایش چند سند مکانیزه به صورت همزمان وجود ندارد." });
            Formulas = await GetAllRequiredFormulas(GroupedDataList);
            AccountHeads = await _repository.GetAll<Data.Entities.AccountHead>().ToListAsync();
            // Generate VoucherHeads
            foreach (var groupedDataDictionary in GroupedDataList)
            {
                var groupedData = groupedDataDictionary.Value;

                Data.Entities.VouchersHead voucherHead;
                if (request.VoucherHeadId != default)
                {
                    voucherHead = await _repository.Find<Data.Entities.VouchersHead>().Include(x => x.VouchersDetails).FirstOrDefaultAsync(x => x.Id == request.VoucherHeadId);
                    if (voucherHead.VoucherStateId != 1) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"سند مکانیزه ای ({voucherHead.VoucherNo}) که قصد ویرایش آن را دارید در وضعیت دائم می باشد." });
                    _repository.Update(voucherHead);
                }
                else
                {
                    voucherHead = await GenerateVoucherHead(groupedData.FirstOrDefault());
                }

                voucherHeads.Add(voucherHead);

                var groupFormulas = Formulas.Where(x => x.VoucherTypeId == voucherHead.CodeVoucherGroupId).ToList();
                foreach (var data in groupedData)
                {
                    int? documentId = data?.DocumentId ?? null;
                    DateTime documentDate = data.DocumentDate;
                    int codeVoucherGroupId = data.CodeVoucherGroupId ?? 0;

                    var dupplicatedDetailsInAnotherVoucherHeadNumber = 0;
                    if (voucherHead.Id != default)
                    {
                        if (documentId != null)
                        {
                            dupplicatedDetailsInAnotherVoucherHeadNumber = await _repository.GetAll<Data.Entities.VouchersDetail>().Where(x => !x.IsDeleted && x.DocumentId == documentId && x.Voucher.CodeVoucherGroupId == voucherHead.CodeVoucherGroupId && x.VoucherId != voucherHead.Id).Select(x => x.Voucher.VoucherNo).FirstOrDefaultAsync();
                            var voucherDetailsToRemove = voucherHead.VouchersDetails.Where(x => x.DocumentId == documentId && x.Id > 0 && !x.IsDeleted).ToList();
                            foreach (var voucherDetail in voucherDetailsToRemove)
                            {
                                _repository.Delete(voucherDetail);
                            }
                        }
                        if (dupplicatedDetailsInAnotherVoucherHeadNumber != default) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"شماره: " + documentId + " در سند حسابداری " + dupplicatedDetailsInAnotherVoucherHeadNumber + " قبلا ثبت شده است." });


                    }
                    else
                    {
                        dupplicatedDetailsInAnotherVoucherHeadNumber = await _repository.GetAll<Data.Entities.VouchersDetail>().Where(x => !x.IsDeleted && x.DocumentId == documentId && x.Voucher.CodeVoucherGroupId == voucherHead.CodeVoucherGroupId).Select(x => x.Voucher.VoucherNo).FirstOrDefaultAsync();
                        if (dupplicatedDetailsInAnotherVoucherHeadNumber != default) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"شماره: " + documentId + " در سند حسابداری " + dupplicatedDetailsInAnotherVoucherHeadNumber + " قبلا ثبت شده است." });
                    }

                    var voucherHeadClone = voucherHead;
                    if (documentDate != voucherHead.VoucherDate)
                    {
                        var alreadyCreatedVoucherHead = voucherHeads.FirstOrDefault(x => x.VoucherDate == documentDate && x.CodeVoucherGroupId == codeVoucherGroupId);
                        if (alreadyCreatedVoucherHead != null) voucherHead = alreadyCreatedVoucherHead;
                        else
                        {
                            voucherHead = await GenerateVoucherHead(data);
                            voucherHeads.Add(voucherHead);
                        }
                    }
                    foreach (var formula in groupFormulas)
                    {
                        // Generete VoucherDetails
                        if (IsVoucherDetailGenerationAllowed(data, formula))
                        {

                            Data.Entities.VouchersDetail voucherDetail = await this.GenerateVoucherDetail(data, formula);
                            if (voucherDetail.RowIndex == default) voucherDetail.RowIndex = (voucherHead.VouchersDetails.Max(x => x.RowIndex) ?? 0) + 1;
                            if (voucherDetail.AccountHeadId == default) voucherDetail.AccountHeadId = formula.AccountHeadId;
                            if (voucherDetail.VoucherDate == default) voucherDetail.VoucherDate = voucherHead.VoucherDate;

                            SetVoucherDetailAccountHeads(voucherDetail);
                            //await ValidateVoucherDetailAccountsRelations(voucherDetail);


                            voucherHead.VouchersDetails.Add(voucherDetail);
                        }
                    }

                    voucherHead = voucherHeadClone;
                    voucherHeadClone = null;
                }

                //if (!IsVoucherHeadBalanced(voucherHead)) throw new ApplicationValidationException(new ApplicationErrorModel { Message= "امکان ثبت سند مکانیزه غیرموازنه وجود ندارد."});
            }
            await SetVoucherHeadsSums(voucherHeads);
            await AssignVouchersNumbers(voucherHeads);

            await _repository.SaveChangesAsync();
            var response = await _repository.GetAll<Data.Entities.VouchersDetail>()
                                                                      .Where(x => voucherHeads.Select(x => x.Id).Contains(x.VoucherId))
                                                                      .Select(x => new AutoVoucherDetailResult { VoucherHeadId = x.VoucherId, VoucherNo = x.Voucher.VoucherNo, DocumentId = x.DocumentId })
                                                                      .Distinct()
                                                                      .ToListAsync();
            return ServiceResult.Success(response);
        }

        private async Task SetVoucherHeadsSums(List<Data.Entities.VouchersHead> voucherHeads)
        {
            foreach (var voucherHead in voucherHeads)
            {
                voucherHead.TotalCredit = voucherHead.VouchersDetails.Where(x => !x.IsDeleted).Sum(x => x.Credit);
                voucherHead.TotalDebit = voucherHead.VouchersDetails.Where(x => !x.IsDeleted).Sum(x => x.Debit);
            }
        }



        private async Task<bool> CheckIfProvidedDatesAreValid(List<dynamic> dataList)
        {
            foreach (var data in dataList)
            {
                DateTime documentDate = Convert.ToDateTime(data.DocumentDate);
                documentDate = DateTime.SpecifyKind(documentDate, DateTimeKind.Utc);
                var persianDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(documentDate, "Iran Standard Time");
                data.DocumentDate = new DateTime(persianDate.Year, persianDate.Month, persianDate.Day, 12, 0, 0, DateTimeKind.Utc);
            }
            // Check if year is active or allowed to be editted (and till when) and documentDate is valid within the year
            // Check if documentDate is not in holidays
            return true;
        }
        private IDictionary<int, List<dynamic>> GroupDataListByDateAndCodeVoucherGroupId(List<dynamic> dataList)
        {
            var dictionary = new Dictionary<int, List<dynamic>>();

            var groupedData = dataList.GroupBy(x => ((x.CodeVoucherGroupId.ToString() + Convert.ToDateTime(Convert.ToDateTime(x.DocumentDate).ToShortDateString()).ToUniversalTime().ToString()).GetHashCode()));
            foreach (var group in groupedData)
            {
                dictionary.Add(group.Key, group.ToList());
            }

            return dictionary;
        }
        private async Task<List<Data.Entities.AutoVoucherFormula>> GetAllRequiredFormulas(IDictionary<int, List<dynamic>> groupedDataList)
        {
            List<int> codeVoucherGroupIds = groupedDataList.Select(x => (int)(x.Value.FirstOrDefault().CodeVoucherGroupId)).Distinct().ToList();
            var formulas = await this._repository.GetAll<Data.Entities.AutoVoucherFormula>().Where(x => codeVoucherGroupIds.Contains(x.VoucherTypeId)).OrderBy(x => x.OrderIndex).ToListAsync();
            foreach (var id in codeVoucherGroupIds)
            {
                if (formulas.Where(x => x.VoucherTypeId == id).ToList().Count == 0)
                {
                    throw new ApplicationValidationException(new ApplicationErrorModel { Message = " فرمولی برای نوع سند " + id + " یافت نشد." });
                }
            }
            return formulas;
        }
        private async Task<Data.Entities.VouchersHead> GenerateVoucherHead(dynamic data)
        {
            var codeVoucherGroup = await _repository.Find<Data.Entities.CodeVoucherGroup>(x => x.ObjectId(data.CodeVoucherGroupId)).FirstOrDefaultAsync();
            DateTime documentDate = data.DocumentDate;
            var voucherHead = await _repository.Find<Data.Entities.VouchersHead>().Include(x => x.VouchersDetails).FirstOrDefaultAsync(x => x.VoucherDate == documentDate && x.CodeVoucherGroupId == codeVoucherGroup.Id && x.YearId == _currentUser.GetYearId() && x.VoucherStateId == 1);
            if (voucherHead == null)
            {
                var persianDateString = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(documentDate.Kind == DateTimeKind.Utc ? documentDate : documentDate.ToUniversalTime(), "Iran Standard Time").ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));
                voucherHead = new Data.Entities.VouchersHead
                {
                    CompanyId = _currentUser.GetCompanyId(),
                    YearId = _currentUser.GetYearId(),
                    CodeVoucherGroupId = data.CodeVoucherGroupId,
                    VoucherStateId = 1,
                    VoucherDate = Convert.ToDateTime(data.DocumentDate).ToUniversalTime(),
                    VoucherDescription = $"سند مکانیزه {codeVoucherGroup.Title} مورخ {persianDateString}",
                    VouchersDetails = new List<Data.Entities.VouchersDetail>()
                };
                _repository.Insert(voucherHead);
            }
            else
            {
                _repository.Update(voucherHead);
            }
            return voucherHead;
        }
        private async Task<Data.Entities.VouchersDetail> GenerateVoucherDetail(dynamic data, Data.Entities.AutoVoucherFormula formula)
        {
            var voucherDetail = new Data.Entities.VouchersDetail();
            List<FormulasModel.Formula> voucherDetailFormulas = JsonConvert.DeserializeObject<List<FormulasModel.Formula>>(formula.Formula);

            foreach (var voucherDetailFormula in voucherDetailFormulas)
            {

                var destinationPropertyInfo = voucherDetail.GetType().GetProperties().FirstOrDefault(x => x.Name == voucherDetailFormula.Property);
                var destinationPropertyType = destinationPropertyInfo.PropertyType;
                if (destinationPropertyType.IsGenericType && destinationPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) destinationPropertyType = Nullable.GetUnderlyingType(destinationPropertyType);


                string newValue = voucherDetailFormula.Value.Text.ToString().Trim();
                var propertyIndex = 1;
                foreach (var property in voucherDetailFormula.Value.Properties)
                {
                    if (destinationPropertyType != typeof(DateTime) && property.Name.ToLower().Contains("date"))
                        newValue = newValue.Replace("[" + propertyIndex + "]", Convert.ToDateTime(data[property.Name]).ToString("yyyy/MM/dd", new CultureInfo("fa-IR")));
                    else
                        newValue = newValue.Replace("[" + propertyIndex + "]", data[property.Name].ToString());

                    propertyIndex++;
                }
                if (!string.IsNullOrEmpty(newValue?.ToString()))
                {
                    destinationPropertyInfo.SetValue(voucherDetail,
                               Convert.ChangeType(newValue, destinationPropertyType),
                               null);
                }
            }

            _repository.Insert(voucherDetail);
            return voucherDetail;
        }
        private bool IsVoucherDetailGenerationAllowed(dynamic data, Data.Entities.AutoVoucherFormula formula)
        {
            if (formula.Conditions != null)
            {
                List<FormulasModel.FormulaCondition> formulaConditions = JsonConvert.DeserializeObject<List<FormulasModel.FormulaCondition>>(formula.Conditions);
                foreach (var condition in formulaConditions)
                {
                    var expression = condition.Expression.Replace("[startf]", "").Replace("[endf]", "");
                    var propertyIndex = 1;
                    foreach (var property in condition.Properties)
                    {
                        expression = expression.Replace("[" + propertyIndex + "]", data[property.Name].ToString());
                        propertyIndex++;
                    }
                    DataTable table = new DataTable();
                    table.Columns.Add("expression", typeof(bool));
                    DataRow row = table.NewRow();
                    row["expression"] = Convert.ToBoolean((object)table.Compute(expression, ""));
                    bool result = (bool)row["expression"];

                    if (result == false) return false;
                }
            }
            return true;
        }
        private void SetVoucherDetailAccountHeads(Data.Entities.VouchersDetail voucherDetail)
        {
            Data.Entities.AccountHead level3 = null;
            Data.Entities.AccountHead level2 = null;
            Data.Entities.AccountHead level1 = null;

            level3 = AccountHeads.FirstOrDefault(x => x.Id == voucherDetail.AccountHeadId);
            if (level3 != null && level3.ParentId != null) level2 = AccountHeads.FirstOrDefault(x => x.Id == level3.ParentId);
            if (level2 != null && level2.ParentId != null) level1 = AccountHeads.FirstOrDefault(x => x.Id == level2.ParentId);

            if (level1 == null || level2 == null || level3 == null)
            {
                throw new ApplicationValidationException(new ApplicationErrorModel { Message = "در شماره:  " + (voucherDetail.RowIndex != default ? voucherDetail.RowIndex + " / " : "") + voucherDetail.DocumentId + " سطوح حساب معتبر نیست." });
            }

            voucherDetail.Level1 = level1.Id;
            voucherDetail.Level2 = level2.Id;
            voucherDetail.Level3 = level3.Id;
        }
        private async Task ValidateVoucherDetailAccountsRelations(Data.Entities.VouchersDetail voucherDetail)
        {
            if (voucherDetail.AccountReferencesGroupId != default && voucherDetail.AccountReferencesGroupId != null)
            {
                var areGroupAndAccountHeadRelated = await _repository.GetAll<Data.Entities.AccountHeadRelReferenceGroup>().AnyAsync(x => !x.IsDeleted && x.AccountHeadId == voucherDetail.AccountHeadId && x.ReferenceGroupId == voucherDetail.AccountReferencesGroupId);
                if (!areGroupAndAccountHeadRelated)
                    throw new ApplicationValidationException(new ApplicationErrorModel { Message = "در شماره: " + (voucherDetail.RowIndex != default ? voucherDetail.RowIndex + " / " : "") + voucherDetail.DocumentId + " ارتباط حساب و گروه معتبر نیست." });
            }


            if (voucherDetail.ReferenceId1 != default && voucherDetail.ReferenceId1 != null)
            {
                var areGroupAndReferenceRelated = await _repository.GetAll<Data.Entities.AccountReferencesRelReferencesGroup>().AnyAsync(x => !x.IsDeleted && x.ReferenceId == voucherDetail.ReferenceId1 && x.ReferenceGroupId == voucherDetail.AccountReferencesGroupId);
                if (!areGroupAndReferenceRelated)
                    throw new ApplicationValidationException(new ApplicationErrorModel { Message = "در شماره: " + (voucherDetail.RowIndex != default ? voucherDetail.RowIndex + " / " : "") + voucherDetail.DocumentId + " .ارتباط تفصیل شناور و گروه معتبر نیست" });
            }

        }
        private bool IsVoucherHeadBalanced(Data.Entities.VouchersHead voucherHead)
        {
            return voucherHead.TotalCredit == voucherHead.TotalCredit;
        }
        private async Task AssignVouchersNumbers(List<Data.Entities.VouchersHead> voucherHeads)
        {
            foreach (var voucherHead in voucherHeads.Where(x => x.Id == default).ToList())
            {
                voucherHead.VoucherNo = await _voucherHeadCacheServices.GetNewVoucherNumber();
            }
        }

    }

}
