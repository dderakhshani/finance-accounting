using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FileTransfer.WebApi.Persistance.Entities;
using Library.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FileTransfer.WebApi.Persistance.Sql
{
    public partial class FileTransferUnitOfWork : AuditableDbContext, IFileTransferUnitOfWork
    {
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }



        public DbContext DbContext()
        {
            return this;
        }

        public Task<List<TResult>> ExecuteSqlQueryAsync<TResult>(string query, object[] parameters, CancellationToken cancellationToken) where TResult : class
        {
            throw new NotImplementedException();
        }

        private FileTransferUnitOfWork _appDbContext;

        public FileTransferUnitOfWork(DbContextOptions<FileTransferUnitOfWork> options)
            : base(options)
        {
            _appDbContext = this;

            //this.ChangeTracker.LazyLoadingEnabled = false;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }


        public DbSet<Archive> Archives { get; set; }
        public DbSet<ArchiveAttachments> ArchiveAttachments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<BaseValue> BaseValues { get; set; }
        public DbSet<BaseValueType> BaseValueTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Language> Languages { get; set; }
        

        

        public FileTransferUnitOfWork Mock()
        {
            var dbContextOptions = new DbContextOptionsBuilder<FileTransferUnitOfWork>()
                .UseInMemoryDatabase("MockDB")
                .Options;
            _appDbContext = new FileTransferUnitOfWork(dbContextOptions);
            return _appDbContext;
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IBaseEntity).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }

            base.OnModelCreating(modelBuilder);
        }


      

    }

    public static class EFFilterExtensions
    {
        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
                .Invoke(null, new object[] { modelBuilder });
        }

        static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(EFFilterExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(t => t.IsGenericMethod);

        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, IBaseEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
        }
    }

    public class Cloner : ICloneable
    {
        public ChangeTracker ChangeTracker { get; set; }

        public Cloner(ChangeTracker changeTracker)
        {
            ChangeTracker = changeTracker;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}