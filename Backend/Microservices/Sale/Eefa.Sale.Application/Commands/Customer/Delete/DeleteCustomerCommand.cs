using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;
using MediatR;

namespace Eefa.Sale.Application.Commands.Customer.Delete
{

    public class DeleteCustomerCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Domain.Aggregates.CustomerAggregate.Customer>, ICommand
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteCustomerCommand, Domain.Aggregates.CustomerAggregate.Customer>();
        }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ServiceResult>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public DeleteCustomerCommandHandler(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find(request.Id);

            var deletedEntity = _repository.Delete(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
