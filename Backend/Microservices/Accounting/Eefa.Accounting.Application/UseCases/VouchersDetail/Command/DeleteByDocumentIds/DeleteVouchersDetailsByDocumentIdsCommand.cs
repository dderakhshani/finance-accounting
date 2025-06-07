using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Library.Exceptions;


namespace Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Delete
{
    public class DeleteVouchersDetailsByDocumentIdsCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int VoucherId { get; set; }
        public List<int> DocumentIds { get; set; }


    }

    public class DeleteVouchersDetailsByDocumentIdsCommandHandler : IRequestHandler<DeleteVouchersDetailsByDocumentIdsCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteVouchersDetailsByDocumentIdsCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteVouchersDetailsByDocumentIdsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.VouchersHead>(c =>
             c.ObjectId(request.VoucherId))
                .Include(x => x.VouchersDetails)
             .FirstOrDefaultAsync(cancellationToken);
            if (entity.VoucherStateId > 1) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "سندی که قصد ویرایش آن را دارید در وضعیت دائم میباشد." });
            var voucherDetailsToRemove = entity?.VouchersDetails.Where(x => x.DocumentId != null && request.DocumentIds.Contains((int)x.DocumentId)).ToList();
            foreach (var detail in voucherDetailsToRemove)
            {
                _repository.Delete(detail);
                entity.TotalCredit -= detail.Credit;
                entity.TotalDebit -= detail.Debit;
                var relatedDetail = entity.VouchersDetails.Where(x => x.DocumentIds != null && x.DocumentIds.Contains(detail.DocumentId.ToString()) && !x.IsDeleted).FirstOrDefault();
                if (relatedDetail != null)
                {
                    relatedDetail.Credit -= detail.Credit;
                    relatedDetail.Debit -= detail.Debit;
                    _repository.Update(relatedDetail);
                }
            }
            var doesVoucherHasAnyOtherVoucherDetails = voucherDetailsToRemove.Count != entity.VouchersDetails.Count;
            if (entity.TotalDebit == 0 && entity.TotalCredit == 0 && !doesVoucherHasAnyOtherVoucherDetails) _repository.Delete(entity);
            else _repository.Update(entity);

            await _repository.SaveChangesAsync(request.MenueId, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
