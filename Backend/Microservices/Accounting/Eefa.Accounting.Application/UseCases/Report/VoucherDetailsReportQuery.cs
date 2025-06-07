using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Eefa.Accounting.Application.UseCases.Report.Model;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.Report
{
    public class VoucherDetailsReportQuery : Pagination, IRequest<ServiceResult>
    {
        public List<Condition> Conditions { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<string> groupingKeys { get; set; }
        public bool ShowLastLevelDetails { get; set; }
    }


    public class VoucherDetailsReportQueryHandler : IRequestHandler<VoucherDetailsReportQuery, ServiceResult>
    {
        private readonly AccountingUnitOfWork _context;
        private readonly IMapper _mapper;

        public VoucherDetailsReportQueryHandler(AccountingUnitOfWork context, IMapper mapper)
        {
            this._context = context;
            _mapper = mapper;
        }


        public async Task<ServiceResult> Handle(VoucherDetailsReportQuery request, CancellationToken cancellationToken)
        {
            var query = _context.VouchersDetails
                .AsNoTracking()
                .AsQueryable()
                .Include(x => x.AccountHead)
                .Include(x => x.ReferenceId1Navigation)
                .Include(x => x.AccountReferencesGroup)
                .ProjectTo<VoucherDetailReportResultModel>(_mapper.ConfigurationProvider);

            if (request.ToDate != default && request.FromDate != default) query = query.Where(x => x.Date <= request.ToDate && x.Date >= request.FromDate);

            query = query.WhereQueryMaker(request.Conditions);

            var result = new List<VoucherDetailReportResultModel>();

            if(request.groupingKeys.Count > 0 && !request.ShowLastLevelDetails)
            {
                result = await query.OrderByMultipleColumns(request.OrderByProperty).ToListAsync();
            } else
            {
                result = await query.OrderByMultipleColumns(request.OrderByProperty).Paginate(request.Paginator()).ToListAsync();
            }

            if (request.groupingKeys?.Count > 0)
            {
                result = this.GroupAndSummarize(result, request.groupingKeys, 0, request.ShowLastLevelDetails);
            }
            else
            {
                this.CalculateRemain(result);
            }



            return ServiceResult.Success(new
            {
                Data = result,
                TotalCount = await query.CountAsync(),
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
            });
        }


        List<VoucherDetailReportResultModel> GroupAndSummarize(IEnumerable<VoucherDetailReportResultModel> details, List<string> groupingOrder, int level, bool showLastLevelDetails = false)
        {
            if (level >= groupingOrder.Count) return new List<VoucherDetailReportResultModel>();

            var grouped = details.GroupBy(x => x.GetType().GetProperty(groupingOrder[level])?.GetValue(x, null));
            List<VoucherDetailReportResultModel> result = new List<VoucherDetailReportResultModel>();

            foreach (var group in grouped)
            {
                var parent = new VoucherDetailReportResultModel
                {
                    Title = groupingOrder[level],
                    Code = group.Key != null ? group.Key.ToString() : "",
                    Credit = group.Sum(x => x.Credit),
                    Debit = group.Sum(x => x.Debit),
                    Color = GetColorForLevel(level),
                    Level = level,
                };
                result.Add(parent);


                // Check if this is not the last level or the last grouping property is not AccountReferenceId
                if (level < groupingOrder.Count - 1)
                {
                    var children = GroupAndSummarize(group, groupingOrder, level + 1);
                    result.AddRange(children);
                }
                else if (showLastLevelDetails)
                {
                    result.AddRange(group.Select(g => { g.Color = GetColorForLevel(level + 1); g.Level = level + 1; g.Title = ""; return g; })); // Set color for individual items
                }
            }

            return result;
        }


        string GetTranslatedLevel(string level)
        {
            if (level.ToLower().Equals(nameof(VoucherDetailReportResultModel.Level1).ToLower())) return "گروه";
            if (level.ToLower().Equals(nameof(VoucherDetailReportResultModel.Level2).ToLower())) return "کل";
            if (level.ToLower().Equals(nameof(VoucherDetailReportResultModel.Level3).ToLower())) return "معین";
            if (level.ToLower().Equals(nameof(VoucherDetailReportResultModel.AccountReferenceId).ToLower())) return "تفصیل";
            if (level.ToLower().Equals(nameof(VoucherDetailReportResultModel.AccountReferenceGroupId).ToLower())) return "گروه تفصیل";
            if (level.ToLower().Equals(nameof(VoucherDetailReportResultModel.Date).ToLower())) return "تاریخ";
            return "";
        }


        string GetColorForLevel(int level)
        {
            switch (level)
            {
                case 0: return "#006400"; // DarkGreen
                case 1: return "#0000FF"; // Blue
                case 2: return "#008B8B"; // DarkCyan
                case 3: return "#00FFFF"; // Cyan
                case 4: return "#8B0000"; // DarkRed
                case 5: return "#8B008B"; // DarkMagenta
                case 6: return "#B8860B"; // DarkYellow
                case 7: return "#0000FF"; // Blue (repeated)
                default: return "#008000"; // Green

            }
        }

        private void CalculateRemain(List<VoucherDetailReportResultModel> entries)
        {
            double previousRemain = 0;

            foreach (var entry in entries)
            {
                entry.Remain = previousRemain + (entry.Credit - entry.Debit);
                previousRemain = entry.Remain;
            }
        }
    }
}
