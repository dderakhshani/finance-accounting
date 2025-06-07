using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;
using System.Linq.Dynamic.Core;
using System.Text;
using Library.Resources;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Library.Common
{
    public interface IEntityUtill
    {
        List<ChangedProperty> Differentiate<T>(T oldObj, T newObj) where T : class;
        ChangedProperty TranslatedChangedProperties(ChangedProperty changedProperty);
        string ChangeProperyToString(ChangedProperty changedProperty);
    }

    public class EntityUtill : IEntityUtill
    {
        private readonly IResourceFactory _resourceFactory;

        public EntityUtill(IResourceFactory resourceFactory)
        {
            _resourceFactory = resourceFactory;
        }

        public List<ChangedProperty> Differentiate<T>(T oldObj, T newObj) where T : class
        {
            var list = new List<ChangedProperty>();
            foreach (var propertyInfo in typeof(T).GetProperties()
                         .Where(x =>
                             typeof(IBaseEntity).GetProperties().Select(s => s.Name).Contains(x.Name) == false
                             && typeof(IBaseEntity).IsAssignableFrom(x.PropertyType) == false
                         )
                    )
            {

                string? oldValue = oldObj.GetType().GetProperty(propertyInfo.Name)?.GetValue(oldObj)?.ToString();
                string? newValue = newObj.GetType().GetProperty(propertyInfo.Name)?.GetValue(newObj)?.ToString();
                if (oldValue != newValue)
                {
                    list.Add(new ChangedProperty(propertyInfo.Name, oldValue, newValue));
                }
            }

            return list;
        }

        public ChangedProperty TranslatedChangedProperties(ChangedProperty changedProperty)
        {
            changedProperty.Title = _resourceFactory.GetTranslatedMetaData(changedProperty.Title);
            return changedProperty;
        }

        public string ChangeProperyToString(ChangedProperty changedProperty)
        {
            changedProperty = TranslatedChangedProperties(changedProperty);
            var text = new StringBuilder();
            text.Append(changedProperty.Title);
            text.Append("از");
            text.Append(" ");
            text.Append("مقدار");
            text.Append(changedProperty.Old);
            text.Append("به");
            text.Append(" ");
            text.Append(changedProperty.New);
            text.Append("تغییر یافت");
            return text.ToString();
        }
    }
    public class ChangeTrakerUtill
    {

        public async Task<List<AuditEntry>> GetChanges(DbContext dbContext,int menueId)
        {
            //Create TransactionId to Group all audit entry in single Save
            var transactionId = Guid.NewGuid();

            dbContext.ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            //Find All the entity changes in DbContext which are IAuditable
            foreach (var entry in dbContext.ChangeTracker.Entries().Where(x => x.Entity is IAuditable))
            {
                //Do not Track Changes in Audit Table itself
                if (entry.Entity is Models.Audit ||
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
                        auditEntry.UserId =
                            int.Parse(entry.Properties.FirstOrDefault(x => x.Metadata.Name == "CreatedById")
                                ?.CurrentValue?.ToString() ?? string.Empty);

                        //Get Changes of Audit Entity
                        auditEntry.Changes = await GetEntityAuditChanges(dbContext,entry);

                        break;
                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.UserId =
                            int.Parse(entry.Properties.FirstOrDefault(x => x.Metadata.Name == "ModifiedById")
                                ?.CurrentValue?.ToString() ?? string.Empty);
                        break;
                    case EntityState.Modified:

                        //Get Changes of Audit Entity
                        auditEntry.Changes = await GetEntityAuditChanges(dbContext, entry);

                        auditEntry.UserId =
                            int.Parse(entry.Properties.FirstOrDefault(x => x.Metadata.Name == "ModifiedById")
                                ?.CurrentValue?.ToString() ?? string.Empty);
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

                auditEntries.Add(auditEntry);
            }

            return auditEntries;
        }

        private async Task<List<ChangedProperty>> GetEntityAuditChanges(DbContext dbContext,EntityEntry entry)
        {
            var audits = new List<ChangedProperty>();

            //Custom Audit Rules have higher priority
            audits = await AddSepecificEntityAuditRule(dbContext,entry);

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
                    audits.Add(new ChangedProperty(property.Metadata.Name, null,
                        property.CurrentValue?.ToString()));
                }
                else if (entry.State == EntityState.Modified)
                {
                    audits.Add(new ChangedProperty(property.Metadata.Name, property.OriginalValue?.ToString(),
                        property.CurrentValue?.ToString()));
                }
            }

            return audits;

        }

        private async Task<List<ChangedProperty>> AddSepecificEntityAuditRule(DbContext dbContext,EntityEntry entityEntry)
        {
            var audits = new List<ChangedProperty>();

            if (entityEntry.Entity is IAuditable auditableEntity)
            {
                foreach (var change in auditableEntity.Audit() ?? new List<AuditMapRule>())
                {
                    var genericArgs = change.GetType().GenericTypeArguments;

                    var o = new object[]{
                        dbContext,entityEntry, change
                    };
                    var method = GetType().GetMethod("Map")
                        ?.MakeGenericMethod(new Type[] { genericArgs[0], genericArgs[1] });
                    var ruleResult = await (((Task<ChangedProperty?>)method?.Invoke(this, o)!)!);

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

        public async Task<ChangedProperty?> Map<TSource, TDestination>(DbContext dbContext,EntityEntry entityEntry,
            AuditMapRule option)
            where TDestination : class, IBaseEntity
        {
            if (option is not AuditMapRule<TSource, TDestination> changeOption) return null;

            var destinationProperty = SimplifyExpressionQuery(changeOption.Source.Body.ToString());
            foreach (var property in entityEntry.Properties)
            {
                if (property.Metadata.Name != destinationProperty) continue;
                var audit = new ChangedProperty(destinationProperty);
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
                    audit.Old = await dbContext.Set<TDestination>().Where(oldWhereCondition)
                        .Select(changeOption.Destination).FirstOrDefaultAsync();
                }

                if (newValue != default)
                {
                    var newWhereCondition = $"x=> x.{whereParameter} == {newValue}";
                    audit.New = await dbContext.Set<TDestination>().Where(newWhereCondition)
                        .Select(changeOption.Destination).FirstOrDefaultAsync();
                }

                return audit;
            }
            return null;
        }
    }
}