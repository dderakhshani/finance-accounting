using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Eefa.Persistence.Data.Mock.SqlServer
{
    public class MockUnitOfWork : DbContext, IUnitOfWork
    {

        public new IModel Model()
        {
            return base.Model;
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _appDbContext.SaveAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task<int> SaveAsync(int formId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public EntityState State<TEntity>() where TEntity : class, IBaseEntity
        {
            throw new NotImplementedException();
        }

        public EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            throw new NotImplementedException();
        }

        public DbContext DbContext()
        {
            throw new NotImplementedException();
        }

        public DbSet<Library.Models.Audit> AuditLogs { get; set; }

        public Task<List<TEntity>> ExecuteSqlQueryAsync<TEntity>(string query, object[] parameters, CancellationToken cancellationToken) where TEntity : class
        {
            throw new NotImplementedException();
        }

        private readonly IUnitOfWork _appDbContext;

        public MockUnitOfWork(IUnitOfWork unitOfWork)
        {
            _appDbContext = unitOfWork;
        }

      


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(AppDomain.CurrentDomain.GetAssemblies()
                .First(x => x.FullName.Contains("Eefa.Common")));

            base.OnModelCreating(modelBuilder);
        }


    }

    public interface IMockUnitOfWork : IUnitOfWork
    {

    }
}