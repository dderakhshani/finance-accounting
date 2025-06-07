using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Interfaces;

namespace Eefa.Bursary.Domain.Base
{
    public interface IRepository<TEntity> where TEntity : class, IBaseEntity

    {
    public IUnitOfWork UnitOfWork { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<int> SaveChangesAsync(int formId, CancellationToken cancellationToken = new CancellationToken());

    IQueryable<TEntity> GetQuery();

        // get all
        IQueryable<TEntity> GetAll(Action<IEntityCondition<TEntity>> config);

        // find by id
        IQueryable<TEntity> Find(Action<IEntityCondition<TEntity>>? config = null);


    // exist
    Task<bool> Exist(Action<IEntityCondition<TEntity>>? config = null);


    // Insert
    TEntity Insert(TEntity entity);


    // Update
    TEntity Update(TEntity entity);


    // Delete
    TEntity Delete(TEntity entity);

    // Count
    Task<int> GetCountAsync(Action<IEntityCondition<TEntity>>? config = null);


    }
}
