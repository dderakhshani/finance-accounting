using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Sale.Application.Common.Interfaces;
using Eefa.Sale.Application.Queries.Customers;
using Eefa.Sale.Domain.Aggregates;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Sale.Application.Commands.Customer.Create
{
    public class CreateCustomerCommand : CommandBase, IRequest<ServiceResult<CustomerModel>>, IMapFrom<Domain.Aggregates.CustomerAggregate.Customer>, ICommand
    {

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
            profile.CreateMap<CreateCustomerCommand, Domain.Aggregates.CustomerAggregate.Customer>();
        }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ServiceResult<CustomerModel>>
    {
        private readonly ICustomerRepository _repository;
        private readonly IRepository<Person> _personRepository;
        private readonly IMapper _mapper;
        private readonly ICrmServices _crmServices;
        private readonly IRepository<AccountReferencesGroups> _accountReferenceGroupRepository;
        private readonly IRepository<AccountReferences> _accountReferencesRepository;
        private readonly IRepository<Domain.Aggregates.CustomerAggregate.Customer> customerRepository;
        private readonly IRepository<BaseValues> _baseValueRepository;

        public CreateCustomerCommandHandler(ICustomerRepository repository,
            IMapper mapper,
            IRepository<Person> personRepository,
            ICrmServices crmServices,
            IRepository<AccountReferencesGroups> accountReferenceGroupRepository,
            IRepository<AccountReferences> accountReferencesRepository,
            IRepository<Domain.Aggregates.CustomerAggregate.Customer> customerRepository,
            IRepository<BaseValues> baseValueRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _personRepository = personRepository;
            _crmServices = crmServices;
            _accountReferenceGroupRepository = accountReferenceGroupRepository;
            _accountReferencesRepository = accountReferencesRepository;
            this.customerRepository = customerRepository;
            _baseValueRepository = baseValueRepository;
        }
        public async Task<ServiceResult<CustomerModel>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {




            var person = await _personRepository.GetAll().Where(x => x.Id == request.PersonId).Include(x => x.Customers).Include(x => x.AccountReference).ThenInclude(x => x.AccountReferencesRelReferencesGroups).FirstOrDefaultAsync();

            if (!person.AccountReference.AccountReferencesRelReferencesGroups.Any(x => x.ReferenceGroupId == request.AccountReferenceGroupId))
            {
                person.AccountReference.AccountReferencesRelReferencesGroups.Add(new AccountReferencesRelReferencesGroups()
                {
                    ReferenceGroupId = request.AccountReferenceGroupId,
                    Reference = person.AccountReference
                });
            }



            var customer = new Domain.Aggregates.CustomerAggregate.Customer()
            {
                CurrentAgentId = request.CurrentAgentId,
                AccountReferenceGroupId = request.AccountReferenceGroupId,
                CustomerCode = request.CustomerCode,
                Description = request.Description,
                IsActive = request.IsActive,
                EconomicCode = request.EconomicCode,
                CustomerTypeBaseId = request.CustomerTypeBaseId
            };

            if (string.IsNullOrEmpty(customer.CustomerCode))
            {
                var lastCustomerCode = await customerRepository.GetAll().Where(x => x.CustomerCode.Length == 8).OrderByDescending(x => x.CustomerCode).Select(x => x.CustomerCode).FirstOrDefaultAsync();
                customer.CustomerCode = (Convert.ToInt64(lastCustomerCode) + 1).ToString();
            }

            person.Customers.Add(customer);
            await _personRepository.SaveChangesAsync(cancellationToken);

            var customerModel = _mapper.Map<CustomerModel>(customer);
            return ServiceResult<CustomerModel>.Success(customerModel);
        }
    }
}
