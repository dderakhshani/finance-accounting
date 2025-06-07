using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;

namespace Eefa.Sale.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>,ICustomerRepository
    {
        public CustomerRepository(IUnitOfWork unitOfWork, ICurrentUserAccessor _currentUserAccessor, IHierarchicalManager<Customer> _hierarchicalManager, IValidationErrorManager _validationErrorManager)
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }
    }
}
