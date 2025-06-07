using Library.Common;
using Library.Exceptions;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.MoveVoucherDetails
{
    public class MoveVoucherDetailsCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int VoucherHeadId { get; set; }
        public List<int> VoucherDetailIds { get; set; }
    }

    public class MoveVoucherDetailsCommandHandler : IRequestHandler<MoveVoucherDetailsCommand, ServiceResult>
    {
        private readonly IRepository _repository;

        public MoveVoucherDetailsCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(MoveVoucherDetailsCommand request, CancellationToken cancellationToken)
        {
            var voucherDetails = await _repository.GetAll<Data.Entities.VouchersDetail>().Where(x => request.VoucherDetailIds.Contains(x.Id)).ToListAsync();
            var voucherHeads = await _repository.GetAll<Data.Entities.VouchersHead>().Where(x => x.Id == request.VoucherHeadId || x.Id == voucherDetails.FirstOrDefault().VoucherId).Include(x => x.CodeVoucherGroup).ToListAsync();
            if (voucherHeads.Any(x => x.VoucherStateId > 1)) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "امکان جابجایی آرتیکل در اسناد دائم ممکن نیست." });
            if (voucherHeads.Any(x => !x.CodeVoucherGroup.IsEditable)) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "امکان جابجایی آرتیکل از / به سند مقصد یا مبدا ممکن نیست." });
            var sourceVoucherHead = voucherHeads.FirstOrDefault(x => x.Id == voucherDetails.FirstOrDefault().VoucherId);
            var destinationVoucherHead = voucherHeads.FirstOrDefault(x => x.Id == request.VoucherHeadId);
            if (voucherDetails.Any(x => x.DocumentId > 0)) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "امکان جابجایی آرتیکل  های اسناد مکانیزه ممکن نیست." });

            foreach (var voucherDetail in voucherDetails)
            {
                voucherDetail.VoucherId = request.VoucherHeadId;
                _repository.Update(voucherDetail);
            }

            sourceVoucherHead.TotalCredit -= voucherDetails.Sum(x => x.Credit);
            sourceVoucherHead.TotalDebit -= voucherDetails.Sum(x => x.Debit);
            _repository.Update(sourceVoucherHead);

            destinationVoucherHead.TotalCredit += voucherDetails.Sum(x => x.Credit);
            destinationVoucherHead.TotalDebit += voucherDetails.Sum(x => x.Debit);
            _repository.Update(destinationVoucherHead);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }

    }
}
