using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;

namespace Eefa.Bursary.Infrastructure.Repositories
{
    public class FinancialRequestRepository : Eefa.Common.Data.Repository<FinancialRequest>, IFinancialRequestRepository
    {

        public FinancialRequestRepository(IUnitOfWork unitOfWork, ICurrentUserAccessor _currentUserAccessor, IHierarchicalManager<FinancialRequest> _hierarchicalManager, IValidationErrorManager _validationErrorManager)
    : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in this.UnitOfWork.Set<FinancialRequest>().Local)
            {
                entity.ModifiedAt = DateTime.UtcNow;
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedById = _currentUserAccessor.GetId();
                entity.IsDeleted = false;
                entity.OwnerRoleId = _currentUserAccessor.GetRoleId();
            }
            var res = await this.UnitOfWork.SaveChangesAsync(cancellationToken);
            return res;
        }


    }
}
