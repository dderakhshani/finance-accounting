using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eefa.Accounting.Data.Entities;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using LinqToDB.Common;
using MD.PersianDateTime.Standard;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eefa.Accounting.Application.Services
{
    internal class VouchersService
    {

        private readonly IRepository _repository;

        private readonly ICurrentUserAccessor _currentUserAccessor;


        public List<AccountHead> accountHeads;
        public List<CodeVoucherGroup> codeVoucherGroups;
        public List<AutoVoucherFormula> formulas;
        public VouchersService(IRepository repository,
            ICurrentUserAccessor currentUserAccessor)
        {
            _currentUserAccessor = currentUserAccessor;

            _repository = repository;
        }

        public  async Task GetRequiredData(List<int> codeVoucherGroupIds)
        {
            this.accountHeads = await _repository.GetQuery<AccountHead>().ToListAsync();
            this.codeVoucherGroups = await _repository.GetQuery<Data.Entities.CodeVoucherGroup>().Where(x => codeVoucherGroupIds.Contains(x.Id)).ToListAsync();
            this.formulas = await _repository.GetQuery<Data.Entities.AutoVoucherFormula>()
                    .Where(x => codeVoucherGroupIds.Contains(x.VoucherTypeId))
                    .OrderBy(x => x.OrderIndex)
                    .ToListAsync();
        }
        public Dictionary<T, List<JObject>> GroupByDataList<T>(string groupBy, IEnumerable<JObject> dataList)
        {
            var groupByDataList = new Dictionary<T, List<JObject>>();
            foreach (var item in dataList.GroupBy(x => x.GetValue(groupBy)))
            {
                groupByDataList.Add(item.Key.Value<T>(), item.ToList());
            }

            return groupByDataList;
        }

        public async Task CreateVoucherDetail(Data.Entities.CodeVoucherGroup codeVoucherGroup, JObject row, Data.Entities.AutoVoucherFormula autoVoucherFormula,
          List<FormulasModel.Formula> formulas, Data.Entities.VouchersHead voucherHead, KeyValuePair<int, List<JObject>> dataList)
        {
            var accountHeadId = autoVoucherFormula.AccountHeadId;

            var voucherDetail = await GenerateDetail(row, accountHeadId, voucherHead);

            voucherDetail = CustomizVouchersDetail(formulas, dataList, row, voucherDetail);

            SetVoucherDetailAccountHeads(voucherDetail);

            _repository.Insert(voucherDetail);
        }
        public void SetVoucherDetailAccountHeads(VouchersDetail voucherDetail)
        {
            AccountHead level3 = null;
            AccountHead level2 = null;
            AccountHead level1 = null;

             level3 = accountHeads.FirstOrDefault(x => x.Id == voucherDetail.AccountHeadId);
            if (level3.ParentId != default) level2 = accountHeads.FirstOrDefault(x => x.Id == level3.ParentId);
            if (level2.ParentId != default)  level1 = accountHeads.FirstOrDefault(x => x.Id == level2.ParentId);

            voucherDetail.Level1 = level1?.Id;
            voucherDetail.Level2 = level2?.Id;
            voucherDetail.Level3 = level3?.Id;
        }
        public async Task<Data.Entities.VouchersDetail> GenerateDetail(JObject row, int accountHeadId, Data.Entities.VouchersHead insertVoucherHead)
        {
           

            var voucherDetail = new Data.Entities.VouchersDetail()
            {
                Voucher = insertVoucherHead,
                VoucherDate = insertVoucherHead.VoucherDate,
                AccountHeadId = accountHeadId,
               
            };

            try
            {
                var documentId = row.TryGetValue<int>(nameof(Data.Entities.VouchersDetail.DocumentId));
                voucherDetail.DocumentId = documentId;
            }
            catch
            {

            }

            try
            {
                //TODO: Change DocumentDate to ReferenceDate
                var documentDate = DateTime.Parse(row.TryGetValue<string>("DocumentDate"));
                voucherDetail.ReferenceDate = documentDate;
            }
            catch
            {

            }


            //try
            //{
            //    voucherDetail.AccountReferencesGroupId = row.TryGetValue<int>("AccountReferencesGroupId");
            //}
            //catch
            //{
            //    voucherDetail.AccountReferencesGroupId = null;
            //}


            try
            {
                voucherDetail.DocumentIds = row.TryGetValue<string>(nameof(Data.Entities.VouchersDetail.DocumentIds));
            }
            catch
            {
            }

            try
            {
                voucherDetail.Tag = row.TryGetValue<string>(nameof(Data.Entities.VouchersDetail.Tag));
            }
            catch
            {
            }

            return voucherDetail;
        }
        public Data.Entities.VouchersDetail CustomizVouchersDetail(List<FormulasModel.Formula> formulas, KeyValuePair<int, List<JObject>> dataList, JObject row, Data.Entities.VouchersDetail voucherDetail)
        {
            foreach (var formula in formulas)
            {
                var calculatedValues = CalculateProperisValue(formula, dataList.Value, row);

                var propertyInfo = voucherDetail.GetType().GetProperties().FirstOrDefault(x => x.Name == formula.Property);
                if (!string.IsNullOrEmpty(formula.Value.Text))
                {
                    var stringRes =
                        GenerateSpecificStringValue(formula.Value.Text, calculatedValues);
                    if (!stringRes.Trim().IsNullOrEmpty()) 
                    propertyInfo.SetValue(voucherDetail,
                        stringRes.ChangeType(propertyInfo.PropertyType),
                        null);
                }
                else
                {
                    propertyInfo.SetValue(voucherDetail,
                        calculatedValues.First().ChangeType(propertyInfo.PropertyType),
                        null);
                }
            }

            return voucherDetail;
        }
        public bool CheckCondition(string? conditionsStr, IReadOnlyCollection<JObject> dataList = null, JObject row = null)
        {
            bool conditionRes = true;
            if (!string.IsNullOrEmpty(conditionsStr))
            {
                var calculatedValues = new LinkedList<object>();
                var conditions =
                    JsonConvert.DeserializeObject<List<FormulasModel.FormulaCondition>>(conditionsStr);
                foreach (var condition in conditions)
                {
                    foreach (var conditionProperty in condition.Properties)
                    {
                        calculatedValues.FetchValueFromDataRow(dataList, row, conditionProperty);
                    }

                    var a = GenerateSpecificStringValue(condition.Expression, calculatedValues);
                    if (bool.TryParse(a, out var conditionResult))
                    {
                        if (conditionResult)
                        {
                            conditionResult = true;
                            continue;
                        }
                        else
                        {
                            conditionRes = false;
                            break;
                        }
                    }
                    else
                    {
                        throw new Exception("InvalidFromat");
                    }
                }
            }

            return conditionRes;
        }
        public LinkedList<object> CalculateProperisValue(FormulasModel.Formula formula, List<JObject> dataList, JObject row)
        {
            var calculatedValues = new LinkedList<object>();
            if (formula.Value?.Properties != null)
                foreach (var valueObject in formula.Value?.Properties)
                {
                    calculatedValues.FetchValueFromDataRow(dataList, row, valueObject);
                }

            return calculatedValues;
        }

        public async Task<int> GetDailyId(DateTime time)
        {

            var DailyId = ((await _repository.GetQuery<Data.Entities.VouchersHead>().Where(x =>
                      x.VoucherDate.Date == time)
                  .OrderBy(x => x.Id)
                  .LastOrDefaultAsync())?.VoucherDailyId ?? 0) + 1;


            return DailyId++;
        }
        public string GenerateSpecificStringValue(string text, LinkedList<object> calculatedValues)
        {
            if (calculatedValues?.First == null)
            {
                return text;
            }
            var computingString = new StringBuilder();
            var messageBuilder = new StringBuilder();

            var splited = text.Split(' ');
            var workingOnMath = false;
            if (!string.IsNullOrEmpty(text))
            {
                var node = 1;
                for (var i = 0; i < splited.Length; i++)
                {
                    if (splited[i] == $"[{node}]")
                    {
                        if (workingOnMath == false)
                        {
                            if (string.IsNullOrEmpty(computingString.ToString()))
                            {
                                var isDate = DateTime.TryParse(calculatedValues.First.Value.ToString(), out var date);
                                if (isDate)
                                {

                                    var iranStandardDateTime = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time"));
                                    var persianDate = new PersianDateTime(iranStandardDateTime);

                                    messageBuilder.Append(persianDate.ToString("yyyy/MM/dd"));
                                }
                                else
                                {
                                    messageBuilder.Append(calculatedValues.First.Value);
                                }
                                calculatedValues.RemoveFirst();
                            }
                        }
                        else
                        {
                            computingString.Append(calculatedValues.First.Value);
                            computingString.Append(" ");
                            calculatedValues.RemoveFirst();
                        }

                        node++;
                    }
                    else if (splited[i] == $"[startf]")
                    {
                        workingOnMath = true;
                    }
                    else if (splited[i] == $"[endf]")
                    {
                        if (workingOnMath)
                        {
                            workingOnMath = false;
                            if (!string.IsNullOrEmpty(computingString.ToString()))
                            {
                                var answer =
                                    Library.Utility.Utility.EvaluatString(computingString.ToString());
                                messageBuilder.Append(answer);
                            }
                        }
                    }
                    else
                    {
                        if (workingOnMath)
                        {
                            if (splited[i] is "*" or "+" or "-" or "/" or "(" or ")" or ">=" or "<=" or "==" or "=" or ">" or "<" or "!="
                                    or "AND" or "OR"
                                ||
                                int.TryParse(splited[i], out var num))
                            {
                                computingString.Append(splited[i]);
                                computingString.Append(" ");
                            }
                        }
                        else
                        {
                            messageBuilder.Append(splited[i]);
                            messageBuilder.Append(" ");
                        }
                    }
                }
            }

            return messageBuilder.ToString();
        }


    }
}
