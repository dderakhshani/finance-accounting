using System.Linq;
using Eefa.Accounting.Data.Entities;
using Library.Interfaces;
using Library.Utility;

namespace Eefa.Accounting.Data.Databases.Reposiroty
{
    public class Repository : Persistence.Data.SqlServer.Repository
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public Repository(IUnitOfWork unitOfWork, ICurrentUserAccessor currentUserAccessor, IHierarchicalController hierarchicalController) : base(unitOfWork, currentUserAccessor, hierarchicalController)
        {
            _currentUserAccessor = currentUserAccessor;
        }

        public override IQueryable<TEntity> GetQuery<TEntity>()
        {
            if (typeof(TEntity) == typeof(VouchersHead))
            {
                return (IQueryable<TEntity>) base.GetQuery<VouchersHead>().Where(x => x.YearId == _currentUserAccessor.GetYearId());
            }
            else
            {
                return base.GetQuery<TEntity>();
            }
        }
    }
}