using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Delete
{
    public class DeleteChequeBookCommand : CommandBase, IRequest<ServiceResult<Payables_ChequeBooks>>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteChequeBookCommandHandler : IRequestHandler<DeleteChequeBookCommand, ServiceResult<Payables_ChequeBooks>>
    {
        private readonly IBursaryUnitOfWork _uow;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public DeleteChequeBookCommandHandler(IBursaryUnitOfWork uow, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<Payables_ChequeBooks>> Handle(DeleteChequeBookCommand request, CancellationToken cancellationToken)
        {
            var chequeBook = await _uow.Payables_ChequeBooks.AsNoTracking().AsQueryable().FirstOrDefaultAsync(w => w.Id == request.Id);
            if (chequeBook == null)
            {
                throw new ValidationError("دسته چک مورد نظر در سیستم وجود ندارد");
            }

            chequeBook.IsDeleted = true;
            chequeBook.ModifiedAt = DateTime.UtcNow;
            chequeBook.ModifiedById = _currentUserAccessor.GetId();
            _uow.Payables_ChequeBooks.Update(chequeBook);

            var r = await _uow.Payables_ChequeBooksSheets.AsQueryable().Where(w=>w.ChequeBookId == request.Id).ToListAsync();
            if (r != null && r.Count > 0)
            {
                foreach (var item in r)
                {
                    item.IsDeleted = true;
                    item.ModifiedAt = DateTime.UtcNow;
                    item.ModifiedById = _currentUserAccessor.GetId();
                    _uow.Payables_ChequeBooksSheets.Update(item);
                }
            }

            var value = await _uow.SaveChangesAsync(cancellationToken);
            if (value <= 0)
            {
                throw new ValidationError("بروز خطا در حذف دسته چک");
            }
            return ServiceResult<Payables_ChequeBooks>.Success(chequeBook);
        }
    }

}
