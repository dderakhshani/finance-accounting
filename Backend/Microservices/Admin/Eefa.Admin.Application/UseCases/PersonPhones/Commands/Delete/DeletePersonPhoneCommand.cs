using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonPhones.Commands.Delete
{
    public class DeletePersonPhoneCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.PersonPhone>, ICommand
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeletePersonPhoneCommand, Data.Databases.Entities.PersonPhone>();
        }
    }

    public class DeletePersonPhoneCommandHandler : IRequestHandler<DeletePersonPhoneCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeletePersonPhoneCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(DeletePersonPhoneCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.PersonPhone>(c =>
                    c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);

            var deletedEntity = _repository.Delete(entity);
            return await request.Save(_repository, cancellationToken);
        }
    }
}
