using Eefa.Common;
using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Eefa.Sale.Application.Common.Interfaces;
using Eefa.Sale.Domain.Aggregates;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;

namespace Eefa.Sale.Infrastructure.Context
{
    public partial class SalesUnitOfWork : AuditableDbContext, ISalesUnitOfWork
    {
        public virtual DbSet<AccountReferences> AccountReferences { get; set; } = default!;
        public virtual DbSet<AccountReferencesGroups> AccountReferencesGroups { get; set; } = default!;
        public virtual DbSet<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual DbSet<BaseValueTypes> BaseValueTypes { get; set; } = default!;
        public virtual DbSet<BaseValues> BaseValues { get; set; } = default!;
        public virtual DbSet<Customer> Customers { get; set; } = default!;
        public virtual DbSet<PersonPhones> PersonPhones { get; set; } = default!;
        public virtual DbSet<Person> Persons { get; set; } = default!;
        public virtual DbSet<SalesAgents> SalesAgents { get; set; } = default!;


        public SalesUnitOfWork(DbContextOptions<SalesUnitOfWork> options, ICurrentUserAccessor currentUserAccessor)
            : base(options, currentUserAccessor)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}