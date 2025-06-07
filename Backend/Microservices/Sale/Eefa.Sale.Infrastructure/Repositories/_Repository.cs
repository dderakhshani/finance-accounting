using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Common.Data;
using Eefa.Sale.Domain.Aggregates.SalePriceListAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
 

namespace Eefa.Sale.Infrastructure.Repositories
{
    public class Repository<TEntity> :IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly Infrastructure.Repositories.ICurrentUserAccessor _currentUserAccessor;

        public  IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        public TEntity Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exist(Action<global::Infrastructure.Interfaces.IEntityCondition<TEntity>> config = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Find(Action<global::Infrastructure.Interfaces.IEntityCondition<TEntity>> config = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll(Action<global::Infrastructure.Interfaces.IEntityCondition<TEntity>> config)
        {
            return GetQuery().QueryBuilder(config, _unitOfWork);
        }

        public Task<int> GetCountAsync(Action<global::Infrastructure.Interfaces.IEntityCondition<TEntity>> config = null)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TEntity> GetQuery()
        {
            return QueryProvider.GetQuery<TEntity>(_unitOfWork);
        }

        public TEntity Insert(TEntity entity)
        {

            return _unitOfWork.Set<TEntity>().Add(entity).Entity;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return res;
        }

        public Task<int> SaveChangesAsync(int formId, CancellationToken cancellationToken = default)
        {
            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public TEntity Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
