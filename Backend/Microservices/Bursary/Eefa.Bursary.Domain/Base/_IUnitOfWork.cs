using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Eefa.Bursary.Domain.Base
{
  public  interface IUnitOfWork
  {
      Task<int> SaveChangesAsync(CancellationToken cancellationToken);

      Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken);
      DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity;
      public EntityState State<TEntity>() where TEntity : class, IBaseEntity;
      public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
    
   

    }
}
