using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Delete
{
    public class DeleteChequeBookSheetCommand : CommandBase, IRequest<ServiceResult<Payables_ChequeBooksSheets>>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteChequeBookSheetCommandHandler : IRequestHandler<DeleteChequeBookSheetCommand, ServiceResult<Payables_ChequeBooksSheets>>
    {
        private readonly IBursaryUnitOfWork _uow;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public DeleteChequeBookSheetCommandHandler(IBursaryUnitOfWork uow, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<Payables_ChequeBooksSheets>> Handle(DeleteChequeBookSheetCommand request, CancellationToken cancellationToken)
        {
            var sheet = await _uow.Payables_ChequeBooksSheets.FirstOrDefaultAsync(w => w.Id == request.Id);
            if (sheet == null)
            {
                throw new ValidationError("برگه چک مورد نظر در سیستم وجود ندارد");
            }

            sheet.IsDeleted = true;
            sheet.ModifiedAt = DateTime.UtcNow;
            sheet.ModifiedById = _currentUserAccessor.GetId();

            _uow.Payables_ChequeBooksSheets.Update(sheet);
            var value = await _uow.SaveChangesAsync(cancellationToken);
            if (value <= 0)
            {
                throw new Exception("بروز خطا در حذف برگه چک");
            }
            return ServiceResult<Payables_ChequeBooksSheets>.Success(sheet);

        }
    }

}
