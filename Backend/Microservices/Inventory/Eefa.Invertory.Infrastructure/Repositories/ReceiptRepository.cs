using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Repositories
{
    public class ReceiptRepository : Repository<Receipt>, IReceiptRepository
    {
        
        public ReceiptRepository(IUnitOfWork unitOfWork, 
            ICurrentUserAccessor _currentUserAccessor, 
            IHierarchicalManager<Receipt> _hierarchicalManager, 
            IValidationErrorManager _validationErrorManager
           )
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

    }
}
