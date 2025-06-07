using System;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Bursary.Domain.Aggregates.DocumentHeadAggregate;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;


namespace Eefa.Bursary.Infrastructure.Repositories
{
    public class DocumentHeadRepository : Eefa.Common.Data.Repository<DocumentHeads>, IDocumentHeadRepository
    {
        public DocumentHeadRepository(IUnitOfWork unitOfWork, ICurrentUserAccessor _currentUserAccessor, IHierarchicalManager<DocumentHeads> _hierarchicalManager, IValidationErrorManager _validationErrorManager)
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
 
