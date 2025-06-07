using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Repositories
{
    public class UnitCommodityQuotaRepository : Repository<UnitCommodityQuota>, IUnitCommodityQuotaRepository
    {
        
        public UnitCommodityQuotaRepository(IUnitOfWork unitOfWork, 
            ICurrentUserAccessor _currentUserAccessor, 
            IHierarchicalManager<UnitCommodityQuota> _hierarchicalManager, 
            IValidationErrorManager _validationErrorManager
            )
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

    }
}
