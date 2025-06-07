using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;

namespace Eefa.Admin.Application.CommandQueries.PersonPhones.Commands.Create
{
    public class CreatePersonPhoneCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.PersonPhone>, ICommand
    {
        public int PersonId { get; set; } = default!;
        public int PhoneTypeBaseId { get; set; } = default!;
        public string PhoneNumber { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePersonPhoneCommand, Data.Databases.Entities.PersonPhone>();
        }
    }

    public class CreatePersonPhoneCommandHandler : IRequestHandler<CreatePersonPhoneCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreatePersonPhoneCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(CreatePersonPhoneCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.Databases.Entities.PersonPhone>(request);

            _repository.Insert(entity);

            return await request.Save(_repository, entity, cancellationToken);
        }
    }
}
