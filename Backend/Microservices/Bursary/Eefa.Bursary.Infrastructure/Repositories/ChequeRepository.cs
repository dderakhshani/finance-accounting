
using Eefa.Bursary.Domain.Aggregates.ChequeAggregate;
using Eefa.Common;

using System;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;


namespace Eefa.Bursary.Infrastructure.Repositories
{
    public class ChequeRepository : Eefa.Common.Data.Repository<PayCheque>, IChequeRepository
    {
        public ChequeRepository(IUnitOfWork unitOfWork, ICurrentUserAccessor _currentUserAccessor, IHierarchicalManager<PayCheque> _hierarchicalManager, IValidationErrorManager _validationErrorManager)
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in this.UnitOfWork.Set<PayCheque>().Local)
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
