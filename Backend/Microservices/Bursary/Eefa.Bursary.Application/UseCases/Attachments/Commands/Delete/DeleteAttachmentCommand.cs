using Eefa.Bursary.Domain.Entities;
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

namespace Eefa.Bursary.Application.UseCases.Attachments.Commands.Delete
{
    public class DeleteAttachmentCommand : CommandBase, IRequest<ServiceResult<Attachment>>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, ServiceResult<Attachment>>
    {
        private readonly IBursaryUnitOfWork _uow;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public DeleteAttachmentCommandHandler(IBursaryUnitOfWork uow, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<Attachment>> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            var atch = await _uow.Attachment.AsNoTracking().AsQueryable().FirstOrDefaultAsync(w => w.Id == request.Id);
            if (atch == null)
            {
                throw new ValidationError("فایل مورد نظر در سیستم وجود ندارد");
            }

            atch.IsDeleted = true;
            atch.ModifiedAt = DateTime.UtcNow;
            atch.ModifiedById = _currentUserAccessor.GetId();
            _uow.Attachment.Update(atch);

            var value = await _uow.SaveChangesAsync(cancellationToken);
            if (value <= 0)
            {
                throw new ValidationError("بروز خطا در حذف دسته چک");
            }
            return ServiceResult<Attachment>.Success(atch);
        }
    }

}
