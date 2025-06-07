using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Common.Data
{
    public interface IRepository<TEntity> where TEntity : class, IBaseEntity

    {
        public IUnitOfWork UnitOfWork { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(int formId, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
        IQueryable<TEntity> GetAll(Action<IEntityCondition<TEntity>> config = null);
        Task<TEntity> Find(int Id);
        Task<bool> Exist(Action<IEntityCondition<TEntity>> config = null);
        TEntity Insert(TEntity entity);
        TEntity InsertBackgroundTransaction(TEntity entity);
        void AddRange(List<TEntity> entites);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
        IQueryable<TEntity> AsQueryable();
        IQueryable<TEntity> GetAllWithPermission(bool checkOwner, bool checkPermision);
        Task<IQueryable<TEntity>> WithPermissionAsync(bool checkOwner, bool checkPermision);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        void CommitTransaction(IDbContextTransaction transaction);
        void RollbackTransaction(IDbContextTransaction transaction);
    }
}
