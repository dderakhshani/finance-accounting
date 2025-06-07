using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using ServiceStack;

namespace Library.Common
{
    public abstract class AuditableDbContext : DbContext
    {
        public DbSet<Models.Audit> AuditLogs { get; set; }

        public new IModel Model()
        {
            return base.Model;
        }

        protected AuditableDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected AuditableDbContext()
            : base()
        {

        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }


        public EntityState State<TEntity>() where TEntity : class, IBaseEntity
        {
            return this.Entry(typeof(TEntity)).State;
        }

        public EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            return base.Entry(entity);
        }


        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await this.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveAsync(int menueId, CancellationToken cancellationToken)
        {
            return await this.SaveChangesAsync(cancellationToken);
        }
    }
}