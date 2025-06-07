using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Repositories
{
    public class DocumentHeadExtraCostRepository : Repository<DocumentHeadExtraCost>, IDocumentHeadExtraCostRepository
    {

        public DocumentHeadExtraCostRepository(IUnitOfWork unitOfWork,
            ICurrentUserAccessor _currentUserAccessor,
            IHierarchicalManager<DocumentHeadExtraCost> _hierarchicalManager,
            IValidationErrorManager _validationErrorManager
            
            )
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

    }
}
