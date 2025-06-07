using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.Unit.Command.Delete
{
    public class DeleteUnitCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteUnitCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Unit>(c =>
             c.ObjectId(request.Id)).Include(x => x.UnitPositions)
             .FirstOrDefaultAsync(cancellationToken);

            foreach (var unitPosition in entity.UnitPositions)
            {
                _repository.Delete<Data.Databases.Entities.UnitPosition>(unitPosition);
            }


            var deletedEntity = _repository.Delete(entity);
            return await request.Save(_repository, cancellationToken);

        }
    }
}
