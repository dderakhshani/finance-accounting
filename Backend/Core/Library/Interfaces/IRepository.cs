#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Library.Interfaces
{
    public interface IRepository
    {

        #region Methods
        public IModel Model { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(int formId,CancellationToken cancellationToken = new CancellationToken());

        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IBaseEntity;

        // get all
        IQueryable<TEntity> GetAll<TEntity>(Action<IEntityCondition<TEntity>>? config = null) where TEntity : class, IBaseEntity;

        // find by id
        IQueryable<TEntity> Find<TEntity>(Action<IEntityCondition<TEntity>>? config = null) where TEntity : class, IBaseEntity;
     

        // exist
        Task<bool> Exist<TEntity>(Action<IEntityCondition<TEntity>>? config = null) where TEntity : class, IBaseEntity;


        // Insert
        EntityEntry<TEntity> Insert<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;


        // Update
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;


        // Delete
        EntityEntry<TEntity> Delete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

        IQueryable<TEntity> GetAllWithPermission<TEntity>(bool checkOwner, bool checkPermision) where TEntity : class, IBaseEntity;
        Task<IQueryable<TEntity>> WithPermissionAsync<TEntity>(bool checkOwner, bool checkPermision) where TEntity : class, IBaseEntity;
        #endregion
    }
}