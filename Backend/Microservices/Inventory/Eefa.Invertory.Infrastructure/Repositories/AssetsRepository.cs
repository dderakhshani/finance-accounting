using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Repositories
{
    public class AssetsRepository : Repository<Assets>, IAssetsRepository
    {
        
        public AssetsRepository(IUnitOfWork unitOfWork, 
            ICurrentUserAccessor _currentUserAccessor, 
            IHierarchicalManager<Assets> _hierarchicalManager, 
            IValidationErrorManager _validationErrorManager
            )
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

    }
}
