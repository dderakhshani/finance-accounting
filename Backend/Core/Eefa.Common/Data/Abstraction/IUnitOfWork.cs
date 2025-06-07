using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Eefa.Common.Data
{
    public interface IUnitOfWork
    {
        IModel Model();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveAsync(CancellationToken cancellationToken);
        Task BulkInsertAsync<TEntity>(List<TEntity> list,BulkConfig bulkConfig);
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity;
        //public EntityState State<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

        Task<List<TResult>> ExecuteSqlQueryAsync<TResult>(string query, object[] parameters, CancellationToken cancellationToken) where TResult : class;

        public DbContext DbContex();
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    }
}
