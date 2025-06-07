using Library.Interfaces;
using MediatR;
using System.Threading.Tasks;
using Library.Models;
using Library.Common;
using AutoMapper;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.UseCases.Help.Command.Delete
{
    public class DeleteHelpCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteHelpCommandHandler : IRequestHandler<DeleteHelpCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mappaer;

        public DeleteHelpCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mappaer = mapper;
        }

        public async Task<ServiceResult> Handle(DeleteHelpCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Help>(c =>
                c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);

            var deletedEntity = _repository.Delete(entity);
            return await request.Save(_repository, cancellationToken);
        }
    }
}
