using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Repositories
{
    public class AssetsRepositoryRepository : Repository<AccessToWarehouse>, IAccessToWarehouseRepository
    {
        
        public AssetsRepositoryRepository(IUnitOfWork unitOfWork, 
            ICurrentUserAccessor _currentUserAccessor, 
            IHierarchicalManager<AccessToWarehouse> _hierarchicalManager, 
            IValidationErrorManager _validationErrorManager
            )
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

    }
}
