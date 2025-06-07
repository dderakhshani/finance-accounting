using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Admin.Application.CommandQueries.MenuItem.Command.Delete
{
    public class DeleteMenuItemCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteMenuItemCommandHandler : IRequestHandler<DeleteMenuItemCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteMenuItemCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.MenuItem>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            var deletedEntity = _repository.Delete(entity);
            return await request.Save(_repository, cancellationToken);

        }
    }
}
