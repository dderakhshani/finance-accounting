using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Sale.Application.Common.Interfaces;
using Eefa.Sale.Application.Queries.Customers;
using Eefa.Sale.Domain.Aggregates;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Sale.Application.Commands.Customer.Update
{

    public class UpdateCustomerCommand : CommandBase, IRequest<ServiceResult<CustomerModel>>,
        IMapFrom<Domain.Aggregates.CustomerAggregate.Customer>, ICommand
    {
      
        public int Id { get; set; }
        public int PersonId { get; set; } = default!;
        public int AccountReferenceGroupId { get; set; }
        public int CustomerTypeBaseId { get; set; } = default!;
        public string CustomerCode { get; set; } = default!;
        public int CurrentAgentId { get; set; } = default!;
        public string? EconomicCode { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCustomerCommand, Domain.Aggregates.CustomerAggregate.Customer>()
                .IgnoreAllNonExisting();
        }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ServiceResult<CustomerModel>>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICrmServices _crmServices;

        public UpdateCustomerCommandHandler(ICustomerRepository repository, IMapper mapper, ICrmServices crmServices)
        {
            _repository = repository;
            _mapper = mapper;
            _crmServices = crmServices;
        }

        public async Task<ServiceResult<CustomerModel>> Handle(UpdateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await _repository
                .AsQueryable()
                .Where(x => x.Id == request.Id)
                .Include(x => x.Person)
                .ThenInclude(x => x.AccountReference)
                .ThenInclude(x => x.AccountReferencesRelReferencesGroups)
                .FirstOrDefaultAsync(cancellationToken);


            if (!customer.Person.AccountReference.AccountReferencesRelReferencesGroups.Any(x => x.ReferenceGroupId == request.AccountReferenceGroupId))
            {
                //var relReferenceGroupToDelete =
                //    customer.Person.AccountReference.AccountReferencesRelReferencesGroups.FirstOrDefault(x =>
                //        x.ReferenceGroupId == customer.AccountReferenceGroupId);

                //customer.Person.AccountReference.AccountReferencesRelReferencesGroups.Remove(relReferenceGroupToDelete);

                customer.Person.AccountReference.AccountReferencesRelReferencesGroups.Add(new AccountReferencesRelReferencesGroups()
                {
                    Reference = customer.Person.AccountReference,
                    ReferenceGroupId = request.AccountReferenceGroupId
                });
            }

            _mapper.Map(request, customer);




            var updatedEntity = _repository.Update(customer);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult<CustomerModel>.Success(_mapper.Map<CustomerModel>(updatedEntity));

        }
    }
}
