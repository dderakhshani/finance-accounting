using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;

namespace Eefa.Purchase.Infrastructure.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        
        public InvoiceRepository(IUnitOfWork unitOfWork, 
            ICurrentUserAccessor _currentUserAccessor, 
            IHierarchicalManager<Invoice> _hierarchicalManager, 
            IValidationErrorManager _validationErrorManager
            )
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

    }
}
