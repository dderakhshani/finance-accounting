using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Eefa.Common.Data.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage;
using EFCore.BulkExtensions;

namespace Eefa.Common.Data
{
    public abstract class AuditableDbContext : DbContext, IUnitOfWork
    {

        public DbSet<Audit> AuditLogs { get; set; }

        public new IModel Model()
        {
            return base.Model;
        }

        public DbContext DbContex()
        {
            return this;
        }

        public IConfiguration _configuration;
        public ICurrentUserAccessor _currentUserAccessor;

        protected AuditableDbContext(DbContextOptions options, ICurrentUserAccessor currentUserAccessor, IConfiguration configuration = default)
            : base(options)
        {
            _currentUserAccessor = currentUserAccessor;
            _configuration = configuration;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public async Task<List<TResult>> ExecuteSqlQueryAsync<TResult>(string query, object[] parameters) where TResult : class
        {
            return await this.SqlQueryAsync<TResult>(query, parameters, CancellationToken.None);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            var types = modelBuilder.Model.GetEntityTypes().Select(t => t.ClrType).ToHashSet();

            modelBuilder.ApplyConfigurationsFromAssembly(
                    Assembly.GetExecutingAssembly(),
                    t => t.GetInterfaces().Any(i => i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                            && types.Contains(i.GenericTypeArguments[0]))
                );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                //Automatically remove logical deleted record by Adding Default Query
                if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var isDeletedProperty = entityType.FindProperty("IsDeleted");
                    var parameter = Expression.Parameter(entityType.ClrType, "p");
                    var expression = Expression.Equal(Expression.Property(parameter, isDeletedProperty.PropertyInfo), Expression.Constant(false));
                    var filter = Expression.Lambda(expression, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }

        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            return base.Entry(entity);
        }

        public async Task<List<TResult>> ExecuteSqlQueryAsync<TResult>(string query, object[] parameters, CancellationToken cancellationToken) where TResult : class
        {
            return await this.SqlQueryAsync<TResult>(query, parameters, cancellationToken);
        }

        // for commit and roleback transactions
        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await this._SaveChangesAsync(0, cancellationToken);
            //return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(int menuId, CancellationToken cancellationToken = default)
        {
            return await this._SaveChangesAsync(menuId, cancellationToken);
            //return base.SaveChangesAsync(cancellationToken);
        }

        private async Task<int> _SaveChangesAsync(int menuId, CancellationToken cancellationToken)
        {
            //Add Auditable Codes
            foreach (var entity in ChangeTracker.Entries())
            {
                var baseEntity = entity.Entity as IBaseEntity;
                if (entity.State == EntityState.Added)
                {
                    baseEntity.ModifiedAt = DateTime.UtcNow;
                    baseEntity.CreatedAt = DateTime.UtcNow;
                    baseEntity.CreatedById = _currentUserAccessor.GetId();
                    baseEntity.IsDeleted = false;
                    baseEntity.OwnerRoleId = _currentUserAccessor.GetRoleId();
                }
                else if (entity.State == EntityState.Modified)
                {
                    baseEntity.ModifiedAt = DateTime.UtcNow;
                }
            }

            return await _SaveAuditChanges(menuId, cancellationToken);

        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Database.BeginTransactionAsync(cancellationToken);
        }


        #region Audit 

        public async Task<int> _SaveAuditChanges(int menueId, CancellationToken cancellationToken)
        {
            try

            {
                var logs = await this.CreateAuditEntries(menueId);

                foreach (var auditEntry in logs)
                {
                    if (auditEntry.Changes.Count == 0 || auditEntry.AuditType == AuditType.Create)
                    {
                        continue;
                    }
                    AuditLogs.Add(auditEntry.ToAudit());
                }

                //----------------------------Main Save All Entites----------------------------
                var res = await base.SaveChangesAsync(cancellationToken);

                //Adding Second Level Audit??????
                foreach (var auditEntry in logs)
                {
                    if (auditEntry.AuditType == AuditType.Create)
                    {
                        var entity = auditEntry.Entry.Entity;
                        auditEntry.PrimaryKey = (int)entity.GetType().GetProperty("Id").GetValue(entity, null);// int.Parse(auditEntry.Entry.Entity.GetId().ToString() ?? "0");
                        AuditLogs.Add(auditEntry.ToAudit());
                    }
                }

                await base.SaveChangesAsync(cancellationToken);


                return res;

            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                //if (e is DbUpdateException { InnerException: SqlException sqlException })
                //{
                //    switch (sqlException.Number)
                //    {
                //        case 2627:  // Unique constraint error
                //        case 547:   // Constraint check violation
                //        case 2601:  // Duplicated key row error
                //            throw new UniqueKeyViolation();
                //        default:
                //            throw;
                //    }
                //}

                throw;

            }
        }

        public async Task<List<AuditEntry>> CreateAuditEntries(int menueId)
        {
            //Create TransactionId to Group all audit entry in single Save
            var transactionId = Guid.NewGuid();

            this.ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            //Find All the entity changes in DbContext which are IAuditable
            foreach (var entry in this.ChangeTracker.Entries().Where(x => x.Entity is IAuditable))
            {
                //Do not Track Changes in Audit Table itself
                if (entry.Entity is Audit ||
                    entry.State is EntityState.Detached or EntityState.Unchanged)
                    continue;


                var auditEntry = new AuditEntry
                {
                    TransactionId = transactionId,
                    //Find Name of Mapped Table via TableAttribute on the Entity class
                    TableName = (entry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false)
                        .FirstOrDefault() as TableAttribute)?.Name ?? throw new Exception("Audit: TableAttribute not found on Entity"),
                    //SubSystem = subSystem,
                    MenueId = menueId,
                    //Find PrimaryKey of the Entity
                    PrimaryKey = int.Parse(entry.Properties.FirstOrDefault(x => x.Metadata.IsPrimaryKey())?.CurrentValue
                        .ToString() ?? string.Empty),

                    //Find PrimaryKey of the Entity
                    OwnerRoleId = int.Parse(entry.Properties.FirstOrDefault(x => x.Metadata.Name == "OwnerRoleId")
                        ?.CurrentValue?.ToString() ?? string.Empty)
                };

                switch (entry.State)
                {

                    case EntityState.Added:

                        auditEntry.Entry = entry;
                        auditEntry.AuditType = AuditType.Create;


                        auditEntry.Changes = await GetEntityAuditChanges(entry, auditEntry.TableName);
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;

                        break;

                    case EntityState.Modified:

                        //Get Changes of Audit Entity
                        auditEntry.Changes = await GetEntityAuditChanges(entry, auditEntry.TableName);




                        auditEntry.AuditType = entry.Properties.Any(x =>
                            x.Metadata.Name == nameof(IBaseEntity.IsDeleted) &&
                            (bool)entry.Properties.Single(t =>
                                    t.Metadata.Name == nameof(IBaseEntity.IsDeleted))
                                .CurrentValue == true)
                            ? AuditType.Delete
                            : AuditType.Update;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    default:
                        break;
                }
                //if(auditEntry.UserId>0)
                auditEntry.UserId = _currentUserAccessor.GetId();
                auditEntries.Add(auditEntry);
            }

            return auditEntries;
        }
        private async Task<List<AuditChangeValue>> GetEntityAuditChanges(EntityEntry entry, string TableName)
        {
            var audits = new List<AuditChangeValue>();

            //Custom Audit Rules have higher priority

            audits = await AddSepecificEntityAuditRule(entry);

            foreach (var property in entry.Properties)
            {
                //Ignore all Properties exist in IBaseEntity



                if (typeof(IBaseEntity).GetProperties()
                    .Any(x => x.Name == property.Metadata.Name && x.Name != "Id"))
                {
                    continue;
                }

                //Ignore all which was already overrided by IAuditable.Audit()
                if (audits.Exists(x => x.Title == property.Metadata.Name))
                {
                    continue;
                }

                //Ignore if current value is same as old one
                if (property.OriginalValue?.ToString() == property.CurrentValue?.ToString() && entry.State == EntityState.Modified)
                {
                    continue;
                }

                if (entry.State == EntityState.Added)
                {
                    string ColumnName = await GetColumnsName(TableName, property);
                    audits.Add(new AuditChangeValue(property.Metadata.Name, null, property.CurrentValue?.ToString(), ColumnName));

                }
                else if (entry.State == EntityState.Modified)
                {
                    string ColumnName = await GetColumnsName(TableName, property);
                    audits.Add(new AuditChangeValue(property.Metadata.Name, property.OriginalValue?.ToString(), property.CurrentValue?.ToString(), ColumnName));
                }
            }

            return audits;

        }

        private async Task<string> GetColumnsName(string TableName, PropertyEntry property)
        {
            var model = new SpGetColumnsDescriptionParam() { TableName = TableName, ColumnName = property.Metadata.Name };
            var parameters = model.EntityToSqlParameters();
            var DescriptionColumnName = await ExecuteSqlQueryAsync<SpGetColumnsDescription>($"EXEC [dbo].[SPGetColumnsDescription] {Eefa.Common.Data.Query.QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            var ColumnNames = DescriptionColumnName.Select(a => a.Description).FirstOrDefault();
            var ColumnName = ColumnNames != null ? ColumnNames : property.CurrentValue?.ToString();
            return ColumnName;
        }

        private async Task<List<AuditChangeValue>> AddSepecificEntityAuditRule(EntityEntry entityEntry)
        {
            var audits = new List<AuditChangeValue>();

            if (entityEntry.Entity is IAuditable auditableEntity)
            {
                foreach (var change in auditableEntity.Audit() ?? new List<AuditMapRule>())
                {
                    var genericArgs = change.GetType().GenericTypeArguments;

                    var o = new object[]{
                        entityEntry, change
                    };
                    var method = GetType().GetMethod("Map")
                        ?.MakeGenericMethod(new Type[] { genericArgs[0], genericArgs[1] });
                    var ruleResult = await (((Task<AuditChangeValue?>)method?.Invoke(this, o)!)!);

                    if (ruleResult != null)
                    {
                        audits.Add(ruleResult);
                    }
                }

            }
            return audits;
        }

        private string SimplifyExpressionQuery(string query)
        {
            query = query.Replace("convert", " ");
            query = query.Replace("{", " ");
            query = query.Replace("}", " ");
            query = query.Replace("$", " ");
            query = query.Replace("Object", " ");
            query = query.Replace(",", " ");
            query = query.Replace(")", " ");
            query = query.Replace("(", " ");
            query = query.Replace('\"', ' ').Trim();

            query = query.Remove(0, query.IndexOf('.') + 1);

            return query;
        }

        public async Task<AuditChangeValue?> Map<TSource, TDestination>(EntityEntry entityEntry,
            AuditMapRule option)
            where TDestination : class, IBaseEntity
        {
            if (option is not AuditMapRule<TSource, TDestination> changeOption) return null;

            var destinationProperty = SimplifyExpressionQuery(changeOption.Source.Body.ToString());
            foreach (var property in entityEntry.Properties)
            {
                if (property.Metadata.Name != destinationProperty) continue;
                var audit = new AuditChangeValue(destinationProperty);
                var whereParameter = SimplifyExpressionQuery(changeOption.ComparerProperty.Body.ToString());
                if (property.OriginalValue == null || property.CurrentValue == null)
                {
                    continue;
                }
                var oldValue = int.Parse(property.OriginalValue?.ToString() ?? string.Empty);
                var newValue = int.Parse(property.CurrentValue?.ToString() ?? string.Empty);
                if (oldValue == newValue && entityEntry.State == EntityState.Modified) continue;

                if (oldValue != default && entityEntry.State == EntityState.Modified)
                {
                    var oldWhereCondition = $"x=> x.{whereParameter} == {oldValue}";
                    audit.Old = await Set<TDestination>().Where(oldWhereCondition)
                        .Select(changeOption.Destination).FirstOrDefaultAsync();
                }

                if (newValue != default)
                {
                    var newWhereCondition = $"x=> x.{whereParameter} == {newValue}";
                    audit.New = await Set<TDestination>().Where(newWhereCondition)
                        .Select(changeOption.Destination).FirstOrDefaultAsync();
                }

                return audit;
            }
            return null;
        }
        public async Task BulkInsertAsync<TEntity>(List<TEntity> entities, BulkConfig bulkConfig)
        {
            await this.BulkInsertAsync(entities, bulkConfig);
        }

        #endregion

    }
}