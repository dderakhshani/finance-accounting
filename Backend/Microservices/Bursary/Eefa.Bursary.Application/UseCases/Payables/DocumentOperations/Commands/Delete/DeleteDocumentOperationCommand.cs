using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Commands.Delete
{
    public class DeleteDocumentOperationCommand:CommandBase,IRequest<ServiceResult<Payables_DocumentsOperations>>,ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteDocumentOperationCommandHandler : IRequestHandler<DeleteDocumentOperationCommand, ServiceResult<Payables_DocumentsOperations>>
    {
        private readonly IBursaryUnitOfWork _uow;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public DeleteDocumentOperationCommandHandler(IBursaryUnitOfWork uow, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<Payables_DocumentsOperations>> Handle(DeleteDocumentOperationCommand request, CancellationToken cancellationToken)
        {
            var opt = await _uow.Payables_DocumentsOperations.FirstOrDefaultAsync(w => w.Id == request.Id);
            if (opt == null)
            {
                throw new ValidationError("اطلاعات مورد نظر در سیستم وجود ندارد");
            }

            opt.IsDeleted = true;
            opt.ModifiedAt = DateTime.UtcNow;
            opt.ModifiedById = _currentUserAccessor.GetId();

            _uow.Payables_DocumentsOperations.Update(opt);
            var value = await _uow.SaveChangesAsync(cancellationToken);
            if (value <= 0)
            {
                throw new Exception("بروز خطا در حذف عملیات چک");
            }
            return ServiceResult<Payables_DocumentsOperations>.Success(opt);

        }
    }

}
