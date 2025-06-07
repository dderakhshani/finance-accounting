using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.Word;
using Eefa.Accounting.Application.Common.Extensions;
using Eefa.Accounting.Application.Services.Logs;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.CreateAutoVoucher;
using Eefa.Accounting.Application.UseCases.VouchersHead.Services;
using FluentValidation.Results;
using Library.Exceptions;
using Library.Interfaces;
using Library.Models;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Eefa.Accounting.Application.UseCases.VouchersHead.Command.CreateAutoVoucher.CreateAutoVoucherCommandHandler;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.AutoVoucher
{
    public class AutoVoucherRefactoredCommand : IRequest<ServiceResult>
    {
        public int VoucherHeadId { get; set; }
        public List<dynamic> DataList { get; set; }
    }

    public class AutoVoucherRefactoredCommandHandler : IRequestHandler<AutoVoucherRefactoredCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IVoucherHeadCacheServices _voucherHeadCacheServices;
        private readonly IApplicationRequestLogManager _logManager;

        private List<Data.Entities.AccountHead> AccountHeads { get; set; }
        private List<Data.Entities.AutoVoucherFormula> Formulas { get; set; }

        public AutoVoucherRefactoredCommandHandler(IRepository repository, ICurrentUserAccessor currentUser, IVoucherHeadCacheServices voucherHeadCacheServices, IApplicationRequestLogManager logManager)
        {
            _repository = repository;
            _currentUser = currentUser;
            _voucherHeadCacheServices = voucherHeadCacheServices;
            _logManager = logManager;
        }
        public async Task<ServiceResult> Handle(AutoVoucherRefactoredCommand request, CancellationToken cancellationToken)
        {
            await _logManager.CommitLog(request);

            List<Data.Entities.VouchersHead> voucherHeads = new List<Data.Entities.VouchersHead>();
            await CheckIfProvidedDatesAreValid(request.DataList);
            SetVoucherHeadIdsIfNotProvided(request.DataList, request.VoucherHeadId);

            Formulas = await GetAllRequiredFormulas(request.DataList);
            AccountHeads = await _repository.GetAll<Data.Entities.AccountHead>().AsNoTracking().ToListAsync();

            ValidateData(request.DataList);
            // Find/Generate all the VoucherHeads that are needed for this request
            foreach (var document in request.DataList)
            {
                Data.Entities.VouchersHead voucherHead;
                var voucherHeadId = document.VoucherHeadId != null ? (int)document.VoucherHeadId : 0;
                var codeVoucherGroupId = (int)document.CodeVoucherGroupId;
                DateTime documentDate = document.DocumentDate;
                if (voucherHeadId != default)
                {
                    voucherHead = voucherHeads.FirstOrDefault(x => x.Id == voucherHeadId) ?? await _repository.GetAll<Data.Entities.VouchersHead>().Include(x => x.VouchersDetails).FirstOrDefaultAsync(x => x.Id == voucherHeadId);
                    if (voucherHead == null) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"سند  حسابداری با شناسه {voucherHeadId}  برای سند ارسالی شماره {document.DocumentId} یافت نشد." });

                    _repository.Update(voucherHead);
                    if (!voucherHeads.Any(x => x.Id == voucherHeadId)) voucherHeads.Add(voucherHead);
                    // if document date is different with the current voucher head date it should move to another voucher head
                    if (documentDate != voucherHead.VoucherDate || codeVoucherGroupId != voucherHead.CodeVoucherGroupId)
                    {
                        Data.Entities.VouchersHead voucherHeadThatThisDocumentBelongsTo = await GenerateOrFindRelatedVoucherHead(document);
                        if (!voucherHeads.Any(x => x.Id == voucherHeadThatThisDocumentBelongsTo.Id || (x.VoucherDate == documentDate && x.CodeVoucherGroupId == codeVoucherGroupId && x.Id == default)))
                        {
                            voucherHeads.Add(voucherHeadThatThisDocumentBelongsTo);
                            if (voucherHeadThatThisDocumentBelongsTo.Id != default) _repository.Update(voucherHeadThatThisDocumentBelongsTo);
                            if (voucherHeadThatThisDocumentBelongsTo.Id == default) _repository.Insert(voucherHeadThatThisDocumentBelongsTo);
                        }
                    }
                }
                else
                {
                    voucherHead = await GenerateOrFindRelatedVoucherHead(document);
                    if (voucherHead == null) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"خطا در ساخت سند جدید برای شماره سند ارسالی {document.DocumentId} " });

                    if (!voucherHeads.Any(x => (x.VoucherDate == documentDate && x.CodeVoucherGroupId == codeVoucherGroupId && x.Id == voucherHead.Id)))
                    {
                        if (voucherHead.Id == default) _repository.Insert(voucherHead);
                        voucherHeads.Add(voucherHead);
                    }

                }


            }

            var lockedVoucherHeads = voucherHeads.Where(x => x.VoucherStateId > 1).ToArray();
            if (lockedVoucherHeads.Any()) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"اسناد حسابداری با اطلاعات {string.Join(" ", lockedVoucherHeads.Select(x => $"شماره سند:{x.VoucherNo}, شناسه:{x.Id}").ToArray())} قفل میباشند." });

            // Delete old details and check if documents that doesnt have voucherId does not already exists
            foreach (var document in request.DataList)
            {
                Data.Entities.VouchersHead voucherHead;
                if (document.VoucherHeadId != null && document.DocumentId != null)
                {
                    int voucherHeadId = (int)document.VoucherHeadId;
                    int documentId = (int)document.DocumentId;
                    var codeVoucherGroupId = (int)document.CodeVoucherGroupId;

                    voucherHead = voucherHeads.FirstOrDefault(x => x.Id == voucherHeadId);
                    var voucherDetailsToRemove = voucherHead.VouchersDetails.Where(x => x.DocumentId == documentId).ToList();

                    if (voucherDetailsToRemove.Count == 0)
                    {
                        throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"سند ارسالی شماره {document.DocumentId} در سند حسابداری با شناسه {voucherHeadId} برای بروز رسانی یافت نشد." });
                    }
                    else
                    {
                        // check if documentId exists in another document
                        var voucherNoWhereDocumentIsAlreadyExistsAt = await _repository.GetAll<Data.Entities.VouchersHead>().Where(x => x.CodeVoucherGroupId == codeVoucherGroupId && x.Id != voucherHead.Id && x.VouchersDetails.Any(x => x.DocumentId == documentId)).Select(x => x.VoucherNo).FirstOrDefaultAsync();
                        if (voucherNoWhereDocumentIsAlreadyExistsAt != 0) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"سند ارسالی شماره {document.DocumentId} در سند حسابداری با شماره {voucherNoWhereDocumentIsAlreadyExistsAt} قبلا ثبت شده است." });
                    }


                    foreach (var voucherDetailToRemove in voucherDetailsToRemove)
                    {
                        _repository.Delete(voucherDetailToRemove);
                    }
                }
                else if (document.DocumentId != null)
                {
                    var documentId = (int)document.DocumentId;
                    var codeVoucherGroupId = (int)document.CodeVoucherGroupId;

                    // check if documentId exists in another document
                    var voucherNoWhereDocumentIsAlreadyExistsAt = await _repository.GetAll<Data.Entities.VouchersHead>().Where(x => x.CodeVoucherGroupId == codeVoucherGroupId && x.VouchersDetails.Any(x => x.DocumentId == documentId)).Select(x => x.VoucherNo).FirstOrDefaultAsync();
                    if (voucherNoWhereDocumentIsAlreadyExistsAt != 0) 
                        throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"سند ارسالی شماره {documentId} قبلا در سند حسابداری شماره {voucherNoWhereDocumentIsAlreadyExistsAt} ثبت شده است." });
                }
            }

            // Generate new details
            foreach (var document in request.DataList)
            {
                DateTime documentDate = document.DocumentDate;
                int codeVoucherGroupId = (int)document.CodeVoucherGroupId;
                int voucherHeadId = document.VoucherHeadId != null ? (int)document.VoucherHeadId : 0;
                Data.Entities.VouchersHead voucherHead =
                    voucherHeads.FirstOrDefault(x => x.Id != default && x.Id == voucherHeadId && x.VoucherDate == documentDate && x.CodeVoucherGroupId == codeVoucherGroupId) ??
                    voucherHeads.FirstOrDefault(x => x.Id != default && voucherHeadId != default && x.Id != voucherHeadId && x.VoucherDate == documentDate && x.CodeVoucherGroupId == codeVoucherGroupId) ??
                    voucherHeads.FirstOrDefault(x => x.Id != default && x.VoucherDate == documentDate && x.CodeVoucherGroupId == codeVoucherGroupId) ??
                    voucherHeads.FirstOrDefault(x => x.Id == default && x.VoucherDate == documentDate && x.CodeVoucherGroupId == codeVoucherGroupId);

                foreach (var formula in Formulas.Where(x => x.VoucherTypeId == codeVoucherGroupId).ToArray())
                {
                    bool isDeleted = document.IsDeleted != null ? (bool)document.IsDeleted : false;
                    // Generete VoucherDetails
                    if (IsVoucherDetailGenerationAllowed(document, formula) && !isDeleted)
                    {

                        Data.Entities.VouchersDetail voucherDetail = await this.GenerateVoucherDetail(document, formula);

                        var isCreditAndDebitValid = voucherDetail.Credit >= 0 && voucherDetail.Debit >= 0 && (voucherDetail.Credit > 0 || voucherDetail.Debit > 0);
                        if (!isCreditAndDebitValid)
                            throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"مقدار مبلغ بستانکار یا بدهکار نا معتبر میباشد." });


                        if (voucherDetail.RowIndex == default) voucherDetail.RowIndex = (voucherHead.VouchersDetails.Max(x => x.RowIndex) ?? 0) + 1;
                        if (voucherDetail.AccountHeadId == default) voucherDetail.AccountHeadId = formula.AccountHeadId;
                        if (voucherDetail.VoucherDate == default) voucherDetail.VoucherDate = voucherHead.VoucherDate;

                        SetVoucherDetailAccountHeads(voucherDetail);
                        await ValidateVoucherDetailAccountsRelations(voucherDetail);

                        voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.Trim();
                        voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.Replace("ي", "ی");
                        voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.Replace("ى", "ی");
                        voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.Replace("ك", "ک");
                        voucherDetail.VoucherRowDescription = voucherDetail.VoucherRowDescription?.ToEnglishNumbers();

                        _repository.Insert(voucherDetail);
                        voucherHead.VouchersDetails.Add(voucherDetail);

                    }
                }

            }


            await SetVoucherHeadsSums(voucherHeads);
            SetVoucherHeadsRowIndexes(voucherHeads);
            await AssignVouchersNumbers(voucherHeads);

            if (voucherHeads.Any(x => x.VoucherNo == 0)) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "Invalid Operation: VoucherHead with voucher number 0 detected." });
            await _repository.SaveChangesAsync();
            var documentIds = request.DataList.Select(x => x.DocumentId).ToList();

            var response = await _repository.GetAll<Data.Entities.VouchersDetail>()
                                                                      .Where(x => voucherHeads.Select(x => x.Id).Contains(x.VoucherId) && documentIds.Contains(x.DocumentId))
                                                                      .Select(x => new AutoVoucherDetailResult { VoucherHeadId = x.VoucherId, VoucherNo = x.Voucher.VoucherNo, DocumentId = x.DocumentId })
                                                                      .Distinct()
                                                                      .ToListAsync();
            return ServiceResult.Success(response);
        }

        private void ValidateData(List<dynamic> dataList)
        {
            // این کد بخاطر سیستم خزانه داری برای ثبت سند تجمیعی کامنت شده است
            // زیرا آرتیکل مجموع documentId مشترک با ی کی از آرتیکل ها را دارد
            // این کامنت باعث ثبت سند تکراری مکانیزه در صوردت ارسال 2 پیلود با داکیومنت آیدی تکراری میشود

            //if (dataList.Where(x => x.DocumentId != null).GroupBy(x => x.DocumentId).Where(g => g.Count() > 1).Count() != 0)
            //{
            //    throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"Duplicated DocumentId {string.Join(",", dataList.Where(x => x.DocumentId != null).GroupBy(x => x.DocumentId).Where(g => g.Count() > 1).Select(x => x.Key).ToArray())}" });
            //}

            var voucherDetailProperties = typeof(Data.Entities.VouchersDetail).GetProperties();

            foreach (dynamic data in dataList)
            {
                var correspondingFormulas = this.Formulas.Where(x => x.VoucherTypeId == (int)data.CodeVoucherGroupId).ToList();

                foreach (var formula in correspondingFormulas)
                {
                    if (IsVoucherDetailGenerationAllowed(data, formula))
                    {
                        List<FormulasModel.Formula> voucherDetailFormulas = JsonConvert.DeserializeObject<List<FormulasModel.Formula>>(formula.Formula);
                        if (voucherDetailFormulas?.Count == 0)
                        {
                            throw new ApplicationValidationException(new ApplicationErrorModel { Message = "Invalid Empty Formulas on FormulaId: " + formula.Id });
                        }
                        var entityPropertiesToGetFilled = new List<string>();
                        voucherDetailFormulas.ForEach(x => entityPropertiesToGetFilled.Add(x.Property));

                        entityPropertiesToGetFilled.ForEach(x =>
                        {
                            if (voucherDetailProperties.FirstOrDefault(p => p?.Name?.ToLower() == x?.ToLower()) == null)
                                throw new ApplicationValidationException(new ApplicationErrorModel { Source = $"on DocumentId: \"{data?.DocumentId}\", Voucher Detail doesnt have a property named \"{x}\" which is required in formulaId \"{formula.Id}\"", Message = "خطا در اطلاعات ورودی / فرمول" });
                        });

                        var requiredProperties = new List<string>();
                        voucherDetailFormulas.ForEach(x => x?.Value?.Properties?.ForEach(x => { if (!string.IsNullOrEmpty(x?.Name)) requiredProperties.Add(x?.Name); }));

                        requiredProperties.ForEach(x =>
                        {
                            //if (data[x] == null)
                            if (!data.ToString()?.Contains(x))
                                throw new ApplicationValidationException(new ApplicationErrorModel { Source = $"on DocumentId: \"{data?.DocumentId}\", Payload doesnt have a property named \"{x}\" or it has no Value,  which is required in formulaId \"{formula.Id}\"", Message = "خطا در اطلاعات ورودی / فرمول" });

                        });
                    }
                }
            }
        }

        private async Task<bool> CheckIfProvidedDatesAreValid(List<dynamic> dataList)
        {
            var userYear = await _repository.GetQuery<Data.Entities.Year>().FirstOrDefaultAsync(x => x.Id == _currentUser.GetYearId());
            if (!userYear.IsEditable) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"سال مالی {userYear.YearName} قابل ویرایش نمیباشد." });

            foreach (var data in dataList)
            {
                DateTime documentDate = Convert.ToDateTime(data.DocumentDate);
                documentDate = DateTime.SpecifyKind(documentDate, DateTimeKind.Utc);
                var persianDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(documentDate, "Iran Standard Time");
                data.DocumentDate = new DateTime(persianDate.Year, persianDate.Month, persianDate.Day, 12, 0, 0, DateTimeKind.Utc);

                if (!(userYear.FirstDate <= (DateTime)data.DocumentDate && userYear.LastDate >= (DateTime)data.DocumentDate)) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"تاریخ سند شماره {data.DocumentId} خارج از بازه سال مالی {userYear.YearName} است." });
            }
            // Check if documentDate is not in holidays
            return true;
        }
        private void SetVoucherHeadIdsIfNotProvided(List<dynamic> dataList, int voucherHeadId)
        {
            foreach (var data in dataList)
            {
                if (data.VoucherHeadId == null && voucherHeadId != default) data.VoucherHeadId = voucherHeadId;
            }
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
        private async Task<List<Data.Entities.AutoVoucherFormula>> GetAllRequiredFormulas(List<dynamic> dataList)
        {
            List<int> codeVoucherGroupIds = dataList.Select(x => (int)(x.CodeVoucherGroupId)).Distinct().ToList();
            var formulas = await this._repository.GetAll<Data.Entities.AutoVoucherFormula>().Where(x => codeVoucherGroupIds.Contains(x.VoucherTypeId)).OrderBy(x => x.OrderIndex).AsNoTracking().ToListAsync();
            foreach (var id in codeVoucherGroupIds)
            {
                if (formulas.Where(x => x.VoucherTypeId == id).ToList().Count == 0)
                {
                    throw new ApplicationValidationException(new ApplicationErrorModel { Message = " فرمولی برای نوع سند " + id + " یافت نشد." });
                }
            }
            return formulas;
        }
        private async Task<Data.Entities.VouchersHead> GenerateOrFindRelatedVoucherHead(dynamic data)
        {
            var codeVoucherGroup = await _repository.Find<Data.Entities.CodeVoucherGroup>(x => x.ObjectId(data.CodeVoucherGroupId)).AsNoTracking().FirstOrDefaultAsync();
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
                voucherHead.VoucherDailyId = (await _repository.GetQuery<Data.Entities.VouchersHead>().Where(x => x.VoucherDate == voucherHead.VoucherDate).Select(x => (int?)x.VoucherDailyId).MaxAsync(x => x) ?? 0) + 1;

            }
            else _repository.Update(voucherHead);

            return voucherHead;
        }
        private async Task<Data.Entities.VouchersDetail> GenerateVoucherDetail(dynamic data, Data.Entities.AutoVoucherFormula formula)
        {
            var voucherDetail = new Data.Entities.VouchersDetail();
            List<FormulasModel.Formula> voucherDetailFormulas = JsonConvert.DeserializeObject<List<FormulasModel.Formula>>(formula.Formula);
            if (voucherDetailFormulas?.Count == 0)
            {
                throw new ApplicationValidationException(new ApplicationErrorModel { Message = "Invalid Empty Formulas on FormulaId: " + formula.Id });
            }
            foreach (var voucherDetailFormula in voucherDetailFormulas)
            {

                var destinationPropertyInfo = voucherDetail.GetType().GetProperties().FirstOrDefault(x => x.Name == voucherDetailFormula.Property);
                var destinationPropertyType = destinationPropertyInfo.PropertyType;
                if (destinationPropertyType.IsGenericType && destinationPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) destinationPropertyType = Nullable.GetUnderlyingType(destinationPropertyType);


                string newValue = voucherDetailFormula.Value.Text.ToString().Trim();
                var propertyIndex = 1;
                foreach (var property in voucherDetailFormula.Value.Properties.Where(x => !string.IsNullOrEmpty(x.Name)))
                {
                    if (destinationPropertyType != typeof(DateTime) && property.Name.ToLower().Contains("date"))
                    {
                        if (newValue.Contains("[" + propertyIndex + "]"))
                            newValue = newValue.Replace("[" + propertyIndex + "]", Convert.ToDateTime(data[property.Name]).ToString("yyyy/MM/dd", new CultureInfo("fa-IR")));
                    }

                    else
                    {
                        if (newValue.Contains("[" + propertyIndex + "]"))
                            newValue = newValue.Replace("[" + propertyIndex + "]", data[property.Name].ToString());
                    }

                    propertyIndex++;
                }
                if (!string.IsNullOrEmpty(newValue?.ToString()))
                {
                    destinationPropertyInfo.SetValue(voucherDetail,
                               Convert.ChangeType(newValue, destinationPropertyType),
                               null);
                }
            }

            return voucherDetail;
        }
        private bool IsVoucherDetailGenerationAllowed(dynamic data, Data.Entities.AutoVoucherFormula formula)
        {
            if (formula.Conditions != null)
            {
                List<FormulasModel.FormulaCondition> formulaConditions = JsonConvert.DeserializeObject<List<FormulasModel.FormulaCondition>>(formula.Conditions) ?? new List<FormulasModel.FormulaCondition>();
                if (formula.Conditions?.Trim()?.Length > 0 && formulaConditions?.Count == 0)
                {
                    throw new ApplicationValidationException(new ApplicationErrorModel { Message = "Invalid Formula Conditions on FormulaId: " + formula.Id });
                }
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
            if (voucherDetail.AccountHeadId != default && (voucherDetail.AccountReferencesGroupId == default || voucherDetail.ReferenceId1 == default))
            {
                var hasAvailableGroups = await _repository.GetQuery<Data.Entities.AccountHeadRelReferenceGroup>().AnyAsync(x => x.AccountHeadId == voucherDetail.AccountHeadId);
                if (hasAvailableGroups) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "در شماره: " + (voucherDetail.RowIndex != default ? voucherDetail.RowIndex + " / " : "") + voucherDetail.DocumentId + " گروه تفصیل و تفصیل شناور نمیتواند خالی باشد." });

            }

            if (voucherDetail.AccountReferencesGroupId != default && voucherDetail.AccountReferencesGroupId != null)
            {
                var areGroupAndAccountHeadRelated = await _repository.GetAll<Data.Entities.AccountHeadRelReferenceGroup>().AnyAsync(x => !x.IsDeleted && x.AccountHeadId == voucherDetail.AccountHeadId && x.ReferenceGroupId == voucherDetail.AccountReferencesGroupId);
                if (!areGroupAndAccountHeadRelated)
                {
                    var accountHead = AccountHeads.Find(x => x.Id == voucherDetail.AccountHeadId);
                    if (accountHead == null) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"حساب با شناسه {voucherDetail.AccountHeadId} یافت نشد." });
                    var accountReferenceGroup = await _repository.GetQuery<Data.Entities.AccountReferencesGroup>().FirstOrDefaultAsync(x => x.Id == voucherDetail.AccountReferencesGroupId);
                    if (accountReferenceGroup == null) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"گروه با شناسه {voucherDetail.AccountReferencesGroupId} یافت نشد." });
                    throw new ApplicationValidationException(new ApplicationErrorModel { Message = "در شماره: " + (voucherDetail.RowIndex != default ? voucherDetail.RowIndex + " / " : "") + voucherDetail.DocumentId + $" ارتباط حساب  {accountHead.Title + " " + accountHead.Code} و گروه {accountReferenceGroup.Title + " " + accountReferenceGroup.Code}معتبر نیست." });
                }
            }


            if (voucherDetail.ReferenceId1 != default && voucherDetail.ReferenceId1 != null)
            {
                var areGroupAndReferenceRelated = await _repository.GetAll<Data.Entities.AccountReferencesRelReferencesGroup>().AnyAsync(x => !x.IsDeleted && x.ReferenceId == voucherDetail.ReferenceId1 && x.ReferenceGroupId == voucherDetail.AccountReferencesGroupId);
                if (!areGroupAndReferenceRelated)
                {
                    var accountReferenceGroup = await _repository.GetQuery<Data.Entities.AccountReferencesGroup>().FirstOrDefaultAsync(x => x.Id == voucherDetail.AccountReferencesGroupId);
                    if (accountReferenceGroup == null) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"گروه با شناسه {voucherDetail.AccountReferencesGroupId} یافت نشد." });

                    var accountReference = await _repository.GetQuery<Data.Entities.AccountReference>().FirstOrDefaultAsync(x => x.Id == voucherDetail.ReferenceId1);
                    if (accountReference == null) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"تفصیل شناور با شناسه {voucherDetail.ReferenceId1} یافت نشد." });

                    throw new ApplicationValidationException(new ApplicationErrorModel { Message = "در شماره: " + (voucherDetail.RowIndex != default ? voucherDetail.RowIndex + " / " : "") + voucherDetail.DocumentId + $" .ارتباط تفصیل شناور {accountReference.Title + " " + accountReference.Code} و گروه {accountReferenceGroup.Title + " " + accountReferenceGroup.Code} معتبر نیست" });
                }
            }

        }
        private bool IsVoucherHeadBalanced(Data.Entities.VouchersHead voucherHead)
        {
            return voucherHead.TotalCredit == voucherHead.TotalCredit;
        }
        private async Task AssignVouchersNumbers(List<Data.Entities.VouchersHead> voucherHeads)
        {
            foreach (var voucherHead in voucherHeads.Where(x => x.Id == 0).ToList())
            {
                voucherHead.VoucherNo = await _voucherHeadCacheServices.GetNewVoucherNumber();
            }
        }
        private async Task SetVoucherHeadsSums(List<Data.Entities.VouchersHead> voucherHeads)
        {
            foreach (var voucherHead in voucherHeads)
            {
                voucherHead.TotalCredit = voucherHead.VouchersDetails.Where(x => !x.IsDeleted).Sum(x => x.Credit);
                voucherHead.TotalDebit = voucherHead.VouchersDetails.Where(x => !x.IsDeleted).Sum(x => x.Debit);
            }
        }
        private void SetVoucherHeadsRowIndexes(List<Data.Entities.VouchersHead> voucherHeads)
        {
            foreach (var voucherHead in voucherHeads)
            {
                var voucherDetails = voucherHead.VouchersDetails.Where(x => !x.IsDeleted).OrderBy(x => x.RowIndex).ThenBy(x => x.CreatedAt).ToArray();
                for (int i = 0; i < voucherDetails.Length; i++)
                {
                    var voucherDetail = voucherDetails.ElementAt(i);
                    voucherDetail.RowIndex = i + 1;
                    if (voucherDetail.Id != default) _repository.Update(voucherDetail);
                }
            }
        }


    }
}
