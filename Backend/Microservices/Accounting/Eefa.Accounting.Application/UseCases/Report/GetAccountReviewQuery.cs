using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Data.Databases.Sp;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Newtonsoft.Json;
using Library.Exceptions;
using Microsoft.EntityFrameworkCore;
using Library.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
namespace Eefa.Accounting.Application.UseCases.Report
{
    public class GetAccountReviewQuery : Pagination, IMapFrom<GetAccountReviewQuery>, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public int ReportType { get; set; }
        public int Level { get; set; }
        public int AccountReferencesGroupflag { get; set; }
        public int CompanyId { get; set; }
        public int[]? YearIds { get; set; }
        public int? VoucherStateId { get; set; }
        public int[]? CodeVoucherGroupIds { get; set; }
        public int? TransferId { get; set; }
        public int[]? AccountHeadIds { get; set; }
        public int[]? ReferenceGroupIds { get; set; }
        public int[]? ReferenceIds { get; set; }
        public int? ReferenceNo { get; set; } = 1;
        public int? VoucherNoFrom { get; set; }
        public int? VoucherNoTo { get; set; }
        public DateTime? VoucherDateFrom { get; set; }
        public DateTime? VoucherDateTo { get; set; }
        public long? DebitFrom { get; set; }
        public long? DebitTo { get; set; }
        public long? CreditFrom { get; set; }
        public long? CreditTo { get; set; }
        public int? DocumentIdFrom { get; set; }
        public int? DocumentIdTo { get; set; }
        public int? CurrencyTypeBaseId { get; set; }

        public string? VoucherDescription { get; set; }
        public string? VoucherRowDescription { get; set; }
        public bool Remain { get; set; }
        public string? ReportTitle { get; set; }
        public SsrsUtil.ReportFormat ReportFormat { get; set; } = SsrsUtil.ReportFormat.None;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetAccountReviewQuery, StpReportBalance6Input>()
                .ForMember(x => x.YearIds, opt => opt.MapFrom(t => (t.YearIds != null && t.YearIds.Length != 0) ? JsonConvert.SerializeObject(t.YearIds) : null))
                .ForMember(x => x.AccountHeadIds, opt => opt.MapFrom(t => (t.AccountHeadIds != null && t.AccountHeadIds.Length != 0) ? JsonConvert.SerializeObject(t.AccountHeadIds) : null))
                .ForMember(x => x.ReferenceGroupIds, opt => opt.MapFrom(t => (t.ReferenceGroupIds != null && t.ReferenceGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceGroupIds) : null))
                .ForMember(x => x.ReferenceIds, opt => opt.MapFrom(t => (t.ReferenceIds != null && t.ReferenceIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceIds) : null))
                .ForMember(x => x.CodeVoucherGroupIds, opt => opt.MapFrom(t => (t.CodeVoucherGroupIds != null && t.CodeVoucherGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.CodeVoucherGroupIds) : null))
                .IgnoreAllNonExisting();
        }
    }

    public class GetAccountReviewQueryHandler : IRequestHandler<GetAccountReviewQuery, ServiceResult>
    {
        private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IMapper _mapper;
        private readonly IAccountingUnitOfWork _context;
        private readonly DanaAccountingUnitOfWork _danaContext;

        public GetAccountReviewQueryHandler(IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, ICurrentUserAccessor currentUserAccessor, IMapper mapper, IAccountingUnitOfWork context, DanaAccountingUnitOfWork danaContext)
        {
            _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
            _currentUser = currentUserAccessor;

            _mapper = mapper;
            _context = context;
            this._danaContext = danaContext;
        }

        public async Task<ServiceResult> Handle(GetAccountReviewQuery request, CancellationToken cancellationToken)
        {

            PermissionExtention permission = new();
            string permissions = permission.GetPermissions<Data.Entities.VouchersDetail>(_context, _currentUser);


            if (_currentUser.GetYearId() == 3) request.YearIds = await _danaContext.Years.Where(x => x.FirstDate <= request.VoucherDateTo && x.LastDate >= request.VoucherDateFrom && x.UserYears.Any(x => x.UserId == _currentUser.GetId())).Select(a => a.Id).ToArrayAsync();
            else request.YearIds = await _context.Years.Where(x => x.FirstDate <= request.VoucherDateTo && x.LastDate >= request.VoucherDateFrom && x.UserYears.Any(x => x.UserId == _currentUser.GetId())).Select(a => a.Id).ToArrayAsync();

            if (request.VoucherDateTo.HasValue)
                request.VoucherDateTo = request.VoucherDateTo.Value.AddDays(1).AddSeconds(-1);


            var input = _mapper.Map<StpReportBalance6Input>(request);
            input.CompanyId = _currentUser.GetCompanyId();


            if (!string.IsNullOrEmpty(permissions))
                input.UserAccessCondition = permissions;

            var results = await _accountingUnitOfWorkProcedures
                .StpReportBalance6Async(input,
                    cancellationToken: cancellationToken
                );
            //results = results.ApplyPermission<StpReportBalance6Result, Data.Entities.VouchersDetail>//<List<StpReportBalance6Result>, Data.Entities.VouchersDetail>()
            //                       (_context, _currentUserAccessor, false, true);
            return ServiceResult.Success(results);
        }
    }
}