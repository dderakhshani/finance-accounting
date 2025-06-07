using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Library.Interfaces
{
    public interface IUnitOfWork
    {
        IModel Model();
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity;
        Task<int> SaveAsync(CancellationToken cancellationToken);
        Task<int> SaveAsync(int formId, CancellationToken cancellationToken);
        public EntityState State<TEntity>() where TEntity : class, IBaseEntity;
        public EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        public DbContext DbContext();
        public DbSet<Models.Audit> AuditLogs { get; set; }

        Task<List<TResult>> ExecuteSqlQueryAsync<TResult>(string query, object[] parameters,
             CancellationToken cancellationToken) where TResult : class;
    }
}