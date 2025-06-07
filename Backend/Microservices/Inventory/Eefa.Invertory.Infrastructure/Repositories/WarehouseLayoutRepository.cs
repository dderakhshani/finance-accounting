using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Invertory.Infrastructure.Context;

namespace Eefa.Invertory.Infrastructure.Repositories
{
    public class WarehouseLayoutRepository : Repository<WarehouseLayout>, IWarehouseLayoutRepository
    {
        private readonly InvertoryContext _contex;

        public WarehouseLayoutRepository(
             InvertoryContext contex
            , IUnitOfWork unitOfWork
            , ICurrentUserAccessor _currentUserAccessor
            , IHierarchicalManager<WarehouseLayout> _hierarchicalManager,
           
        IValidationErrorManager _validationErrorManager)
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {
            _contex = contex;

        }

        
       
    }
}
