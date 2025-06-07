using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Repositories
{
    public class WarehousRepository : Repository<Warehouse>, IWarehouseRepository
    {

        public WarehousRepository(IUnitOfWork unitOfWork,
            ICurrentUserAccessor _currentUserAccessor,
            IHierarchicalManager<Warehouse> _hierarchicalManager,
            IValidationErrorManager _validationErrorManager
            
            )
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

    }
}
