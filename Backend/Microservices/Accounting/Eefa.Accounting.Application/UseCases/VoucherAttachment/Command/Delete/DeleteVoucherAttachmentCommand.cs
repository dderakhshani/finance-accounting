using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VoucherAttachment.Command.Delete
{
    public class DeleteVoucherAttachmentCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteVoucherAttachmentCommandHandler : IRequestHandler<DeleteVoucherAttachmentCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteVoucherAttachmentCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteVoucherAttachmentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.VoucherAttachment>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            _repository.Delete(entity);

            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    return ServiceResult.Success(entity);
                }
            }
            else
            {
                return ServiceResult.Success(entity);
            }

            return ServiceResult.Failure();
        }
    }
}
