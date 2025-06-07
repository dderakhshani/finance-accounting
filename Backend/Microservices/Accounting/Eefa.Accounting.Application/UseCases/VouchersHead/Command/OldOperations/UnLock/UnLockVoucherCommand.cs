using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.UnLock
{
    public class UnLockVoucherCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }

        public int? FromNo { get; set; }
        public int? ToNo { get; set; }

        public int? VoucherStateId { get; set; }
        public int? CodeVoucherGroupId { get; set; }

    }

    public class UnLockVoucherCommandHandler : IRequestHandler<UnLockVoucherCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UnLockVoucherCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UnLockVoucherCommand request, CancellationToken cancellationToken)
        {
            var year = await _repository.GetQuery<Data.Entities.Year>()
                .FirstOrDefaultAsync(x => x.Id == _currentUserAccessor.GetYearId(), cancellationToken: cancellationToken);

            var query = _repository
                .GetQuery<Data.Entities.VouchersHead>();


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



            var resQuery = await query.UpdateFromQueryAsync(x => new Data.Entities.VouchersHead()
            {
                VoucherStateId = 3
            },
                cancellationToken);


            //foreach (var vouchersHead in await query.ToListAsync(cancellationToken))
            //{
            //    vouchersHead.VoucherStateId = 3; // دائم

            //    _repository.Update(vouchersHead);
            //}


            var res = await _repository.SaveChangesAsync(request.MenueId, cancellationToken);
            if (res > 0 || resQuery > 0)
            {
                return ServiceResult.Success(res);
            }
            return ServiceResult.Failure();
        }
    }
}