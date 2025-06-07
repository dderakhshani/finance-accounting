using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.Position.Command.Delete
{
    public class DeletePositionCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public DeletePositionCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Position>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);


            var deletedEntity = _repository.Delete(entity);
            return await request.Save(_repository, cancellationToken);

        }
    }
}
