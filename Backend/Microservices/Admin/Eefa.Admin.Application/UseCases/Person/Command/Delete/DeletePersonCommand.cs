using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Person.Model;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Person.Command.Delete
{
    public class DeletePersonCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeletePersonCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Person>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            var deletedEntity = _repository.Delete(entity);
            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<PersonModel>(deletedEntity.Entity));
            }
            return ServiceResult.Failure();
        }
    }
}
