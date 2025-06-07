using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.OldOperations.VoucherNoRenumber
{
    public class CreateVoucherNoRenumberCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateVoucherNoRenumberCommand>, ICommand
    {
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }

        public int? FromNo { get; set; }
        public int? ToNo { get; set; }

        public int? VoucherStateId { get; set; }
        public int? CodeVoucherGroupId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateVoucherNoRenumberCommand, Data.Entities.VouchersHead>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateVoucherNoRenumberCommandHandler : IRequestHandler<CreateVoucherNoRenumberCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _accountingUnitOfWork;

        public CreateVoucherNoRenumberCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor, IMediator mediator, IUnitOfWork accountingUnitOfWork)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _mediator = mediator;
            _accountingUnitOfWork = accountingUnitOfWork;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateVoucherNoRenumberCommand request, CancellationToken cancellationToken)
        {
            var year = await _repository.GetQuery<Data.Entities.Year>()
                .FirstOrDefaultAsync(x => x.Id == _currentUserAccessor.GetYearId(), cancellationToken: cancellationToken);

            var query = _repository.GetQuery<Data.Entities.VouchersHead>()
                .Where(x => x.CompanyId == _currentUserAccessor.GetCompanyId())
                ;

            if (request.FromDateTime != null)
            {
                query = query.Where(x =>
                        x.VoucherDate >= request.FromDateTime
                        );
            }
            else
            {
                query = query.Where(x =>
                    x.VoucherDate >= year.FirstDate
                );
            }

            if (request.ToDateTime != null)
            {
                query = query.Where(x =>
                    x.VoucherDate <= request.ToDateTime);
            }
            else
            {
                query = query.Where(x =>
                    x.VoucherDate <= year.LastDate);
            }

            if (request.FromNo != null)
            {
                query = query.Where(x =>
                    x.VoucherNo >= request.FromNo);
            }

            if (request.ToNo != null)
            {
                query = query.Where(x =>
                    x.VoucherNo <= request.ToNo);
            }

            if (request.VoucherStateId != null)
            {
                query = query.Where(
                    x => x.VoucherStateId == request.VoucherStateId);
            }

            if (request.CodeVoucherGroupId != null)
            {
                query = query.Where(
                    x => x.CodeVoucherGroupId == request.CodeVoucherGroupId);
            }

            query = query.OrderBy(x => x.CompanyId)
                .ThenBy(x => x.YearId)
                .ThenBy(x => x.VoucherDate)
                .ThenBy(x => x.CodeVoucherGroup.OrderIndex)
                .ThenBy(x => x.VoucherNo)
                .ThenBy(x => x.VoucherDailyId);

            var voucherHeads = await query.ToListAsync(cancellationToken: cancellationToken);

            var minVoucherNo = voucherHeads.First().VoucherNo - 1;
            if (request.FromNo == null && request.FromNo == null)
            {
                minVoucherNo = 0;
            }
            var settings = await new SystemSettings(_repository).Get(SubSystemType.AccountingSettings);
            var voucherNumberType = new int();

            foreach (var baseValue in settings)
            {
                if (baseValue.UniqueName == "VoucherNumberType")
                {
                    voucherNumberType = int.Parse(baseValue.Value);
                    break;
                }
            }


            var cy = voucherHeads.FirstOrDefault()?.YearId;
            foreach (var vouchersHead in voucherHeads)
            {
                if (vouchersHead.YearId != cy)
                {
                    cy = vouchersHead.YearId;
                    minVoucherNo = 0;
                }
                if (voucherNumberType == 2)
                {
                    vouchersHead.VoucherNo = int.Parse($"{_currentUserAccessor.GetBranchId()}{++minVoucherNo}");
                }
                else
                {
                    vouchersHead.VoucherNo = ++minVoucherNo;
                }

                _repository.Update(vouchersHead);
            }

            if (await _repository.SaveChangesAsync(request.MenueId, cancellationToken) > 0)
            {
                await CallRenumberSPForSalesSystem();
                return ServiceResult.Success();
            }

            return ServiceResult.Failure();
        }


        public async Task CallRenumberSPForSalesSystem()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter
            {
                ParameterName = "UserId",
                Value = _currentUserAccessor.GetId()
            });


            var response = await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[Stp_UpdateVouchersNoInSale]  {QueryUtility.SqlParametersToQuey(parameters)}",
                    parameters.ToArray(),
                    new CancellationToken());

        }
    }
}

