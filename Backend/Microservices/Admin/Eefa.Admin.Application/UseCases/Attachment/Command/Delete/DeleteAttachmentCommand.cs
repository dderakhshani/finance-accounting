using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Attachment.Command.Delete
{
    public class DeleteAttachmentCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteAttachmentCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Attachment>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            var deleted = _repository.Delete(entity);
            return await request.Save(_repository, deleted.Entity, cancellationToken);

        }
    }
}
