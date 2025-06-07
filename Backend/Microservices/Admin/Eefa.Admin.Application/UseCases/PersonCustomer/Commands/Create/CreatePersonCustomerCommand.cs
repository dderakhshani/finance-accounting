using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;

namespace Eefa.Admin.Application.CommandQueries.PersonCustomer.Commands.Create
{
    public class CreatePersonCustomerCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Customer>, ICommand
    {
        public int PersonId { get; set; } = default!;

        public int CustomerTypeBaseId { get; set; } = default!;

        public string CustomerCode { get; set; } = default!;

        public int CurentExpertId { get; set; } = default!;
        public string? EconomicCode { get; set; }
        public string? Description { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePersonCustomerCommand, Data.Databases.Entities.Customer>();
        }
    }

    public class CreatePersonCustomerCommandHandler : IRequestHandler<CreatePersonCustomerCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreatePersonCustomerCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(CreatePersonCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.Databases.Entities.Customer>(request);

            _repository.Insert(entity);

            return await request.Save(_repository, entity, cancellationToken);
        }
    }
}
