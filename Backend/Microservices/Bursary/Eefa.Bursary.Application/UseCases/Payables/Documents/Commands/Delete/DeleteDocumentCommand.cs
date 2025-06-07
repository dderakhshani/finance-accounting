using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Delete
{
    public class DeleteDocumentCommand : CommandBase, IRequest<ServiceResult<Payables_Documents>>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, ServiceResult<Payables_Documents>>
    {
        private readonly IBursaryUnitOfWork _uow;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public DeleteDocumentCommandHandler(IBursaryUnitOfWork uow, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<Payables_Documents>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = await _uow.Payables_Documents.AsNoTracking().AsQueryable().FirstOrDefaultAsync(w => w.Id == request.Id);
            if (doc == null)
            {
                throw new ValidationError("سند مورد نظر در سیستم وجود ندارد");
            }

            doc.IsDeleted = true;
            doc.ModifiedAt = DateTime.UtcNow;
            doc.ModifiedById = _currentUserAccessor.GetId();
            _uow.Payables_Documents.Update(doc);

            var r = await _uow.Payables_DocumentsAccounts.AsQueryable().Where(w => w.DocumentId == request.Id).ToListAsync();
            if (r != null && r.Count > 0)
            {
                foreach (var item in r)
                {
                    item.IsDeleted = true;
                    item.ModifiedAt = DateTime.UtcNow;
                    item.ModifiedById = _currentUserAccessor.GetId();
                    _uow.Payables_DocumentsAccounts.Update(item);
                }
            }

            var o = await _uow.Payables_DocumentsOperations.AsQueryable().Where(w => w.DocumentId == request.Id).ToListAsync();
            if (o != null && r.Count > 0)
            {
                foreach (var item in o)
                {
                    item.IsDeleted = true;
                    item.ModifiedAt = DateTime.UtcNow;
                    item.ModifiedById = _currentUserAccessor.GetId();
                    _uow.Payables_DocumentsOperations.Update(item);
                }
            }

            var p = await _uow.Payables_DocumentsPayOrders.AsQueryable().Where(w => w.DocumentId == request.Id).ToListAsync();
            if (p != null && r.Count > 0)
            {
                foreach (var item in p)
                {
                    item.IsDeleted = true;
                    item.ModifiedAt = DateTime.UtcNow;
                    item.ModifiedById = _currentUserAccessor.GetId();
                    _uow.Payables_DocumentsPayOrders.Update(item);
                }
            }

            var value = await _uow.SaveChangesAsync(cancellationToken);
            if (value <= 0)
            {
                throw new ValidationError("بروز خطا در حذف دسته چک");
            }
            return ServiceResult<Payables_Documents>.Success(doc);
        }
    }
}
