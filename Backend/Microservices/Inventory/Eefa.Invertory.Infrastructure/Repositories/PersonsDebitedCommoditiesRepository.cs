using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Repositories
{
    public class PersonsDebitedCommoditiesRepository : Repository<PersonsDebitedCommodities>, IPersonsDebitedCommoditiesRepository
    {

        public PersonsDebitedCommoditiesRepository(IUnitOfWork unitOfWork,
            ICurrentUserAccessor _currentUserAccessor,
            IHierarchicalManager<PersonsDebitedCommodities> _hierarchicalManager,
            IValidationErrorManager _validationErrorManager
            
            )
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

    }
}
