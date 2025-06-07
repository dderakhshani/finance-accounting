using Eefa.Bursary.Domain.Entities;
using System;
using System.Threading.Tasks;
using Eefa.Common.Data;
using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using System.Threading;
using Eefa.Common;
using Eefa.Common.Exceptions;

namespace Eefa.Bursary.Infrastructure.Repositories
{
    public class FinancialRequestDocumentHeadRepositoryRepository :Repository<FinancialRequestDocuments>, IFinancialRequestDocumentHeadRepository
    {

        public FinancialRequestDocumentHeadRepositoryRepository(IUnitOfWork unitOfWork, ICurrentUserAccessor _currentUserAccessor, IHierarchicalManager<FinancialRequestDocuments> _hierarchicalManager, IValidationErrorManager _validationErrorManager)
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
