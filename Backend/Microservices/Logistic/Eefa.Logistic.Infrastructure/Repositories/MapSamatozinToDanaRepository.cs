using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Logistic.Domain;

namespace Eefa.Logistic.Infrastructure
{
    public class MapSamatozinToDanaRepository : Repository<MapSamatozinToDana>, IMapSamatozinToDanaRepository
    {
        
        public MapSamatozinToDanaRepository(IUnitOfWork unitOfWork, 
            ICurrentUserAccessor _currentUserAccessor, 
            IHierarchicalManager<MapSamatozinToDana> _hierarchicalManager, 
            IValidationErrorManager _validationErrorManager
            )
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

    }
}
