using Eefa.Bursary.Domain.Aggregates.AccountReferenceAggregate;
using Eefa.Bursary.Domain.Entities;


using System;

using System.Threading;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;

namespace Eefa.Bursary.Infrastructure.Repositories
{
    public class AccountReferenceRepository : Eefa.Common.Data.Repository<AccountReferences>, IAccountReferenceRepository
    {

        public AccountReferenceRepository( IUnitOfWork unitOfWork,  ICurrentUserAccessor _currentUserAccessor, IHierarchicalManager<AccountReferences> _hierarchicalManager, IValidationErrorManager _validationErrorManager)
            : base(unitOfWork, _currentUserAccessor, _hierarchicalManager, _validationErrorManager)
        {

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in this.UnitOfWork.Set<AccountReferences>().Local)
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
