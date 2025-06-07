using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersHead.Utility;
using Eefa.Accounting.Data.Entities;
using Library.Common;
using Library.Exceptions;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.Adjustment
{
    public class AdjustmentCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public bool ReplaceAdjustmentVoucherFlag { get; set; } = false;
    }

    public class AdjustmentCommandHandler : IRequestHandler<AdjustmentCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public AdjustmentCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(AdjustmentCommand request, CancellationToken cancellationToken)
        {
            var accountHeads = await _repository.GetQuery<Data.Entities.AccountHead>()
                .Where(x => (x.BalanceId == 1 || x.BalanceId == 2) && x.LastLevel)
                .ToListAsync(cancellationToken);

            var balancingCodeVoucherGroup = await _repository.GetQuery<Data.Entities.CodeVoucherGroup>()
                .FirstOrDefaultAsync(x => x.UniqueName == "balancing", cancellationToken);

            var year = await _repository.GetQuery<Year>()
                .FirstOrDefaultAsync(x => x.Id == _currentUserAccessor.GetYearId(), cancellationToken);


            var previousbalancingCodeVoucher = await _repository.GetQuery<Data.Entities.VouchersHead>()
                .FirstOrDefaultAsync(x => x.CodeVoucherGroupId == balancingCodeVoucherGroup.Id, cancellationToken: cancellationToken);

            if (previousbalancingCodeVoucher != null)
            {
                if (request.ReplaceAdjustmentVoucherFlag)
                {
                    if (previousbalancingCodeVoucher.VouchersDetails != null)
                        foreach (var vouchersDetail in previousbalancingCodeVoucher.VouchersDetails)
                        {
                            _repository.Delete(vouchersDetail);
                        }

                    _repository.Delete(previousbalancingCodeVoucher);
                }
                else
                {
                    throw new BalancingVoucherIsAlreadyExists("BalancingVoucherIsAlreadyExists");
                }
            }


            var lastVoucherId = (await _repository.GetQuery<Data.Entities.VouchersHead>().Where(x =>
                    x.VoucherDate.Date == DateTime.Now).OrderBy(x => x.Id)
                .LastOrDefaultAsync(cancellationToken))?.VoucherDailyId ?? 0;

            var voucherNo =
                await VoucherNo.GetNewVoucherNo(_repository, _currentUserAccessor, DateTime.Now, null);

            var balancingVoucherHead = _repository.Insert(new Data.Entities.VouchersHead()
            {
                VoucherNo = voucherNo,
                VoucherDailyId = lastVoucherId + 1,
                VoucherDate = DateTime.Now,
                CodeVoucherGroupId = balancingCodeVoucherGroup.Id,
                CompanyId = _currentUserAccessor.GetCompanyId(),
                VoucherStateId = 1, // موقت
                YearId = _currentUserAccessor.GetYearId(),
                VoucherDescription = "بابت تعدیل حساب"
            });

            foreach (var accountHead in accountHeads)
            {
                var accountDetails = _repository.GetQuery<Data.Entities.VouchersDetail>()
                    .Where(x => x.AccountHeadId == accountHead.Id
                                && x.VoucherDate >= year.FirstDate
                                && x.VoucherDate <= year.LastDate
                                && x.Voucher.CompanyId == _currentUserAccessor.GetCompanyId()
                    );

                var sumCredit = accountDetails.Sum(vouchersDetail => vouchersDetail.Credit);

                var sumDebit = accountDetails.Sum(vouchersDetail => vouchersDetail.Debit);

                var def = sumDebit - sumCredit;
                if (def != 0)
                {
                    if (def > 0) // بدهکار
                    {
                        _repository.Insert(new Data.Entities.VouchersDetail()
                        {
                            Voucher = balancingVoucherHead.Entity,
                            AccountHeadId = accountHead.Id,
                            Debit = def,
                            Credit = 0,
                            VoucherRowDescription = "بابت تعدیل حساب"
                        });
                    }

                    if (def < 0)
                    {
                        if (def > 0) // بدهکار
                        {
                            _repository.Insert(new Data.Entities.VouchersDetail()
                            {
                                Voucher = balancingVoucherHead.Entity,
                                AccountHeadId = accountHead.Id,
                                Debit = 0,
                                Credit = def,
                                VoucherRowDescription = "بابت تعدیل حساب"
                            });
                        }
                    }
                }
            }


            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId, cancellationToken) > 0)
                {
                    return ServiceResult.Success();
                }
            }
            else
            {
                return ServiceResult.Failure();
            }

            return ServiceResult.Failure();
        }
    }
}
