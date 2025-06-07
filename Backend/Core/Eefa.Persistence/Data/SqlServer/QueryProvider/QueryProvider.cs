using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Dynamic.Core;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Expression = System.Linq.Expressions.Expression;
using System.Data;

namespace Eefa.Persistence.Data.SqlServer.QueryProvider
{
    public static class QueryProvider
    {
        //public static IQueryable<TEntity> QueryBuilder<TEntity>(this Action<IEntityCondition<TEntity>>? config, IUnitOfWork unitOfWork, ICurrentUserAccessor currentUserAccessor) where TEntity : class, IBaseEntity
        //{
        //    if (config is null)
        //    {
        //        return GetQuery<TEntity>(unitOfWork, currentUserAccessor);
        //    }
        //    var condition = new EntityConfition<TEntity>();
        //    config.Invoke(condition);

        //    return GetQuery<TEntity>(unitOfWork, currentUserAccessor)
        //        .ObjectId(condition._objectId, unitOfWork.Model())
        //        .Condition(condition._condition)
        //        .Paginate(condition._pagination)
        //        .IncludeDeleted(condition._isDeletedIncluded)
        //        .Traking(condition._asNoTraking);
        //}


        public static IQueryable<TEntity> QueryBuilder<TEntity>(this IQueryable<TEntity> queryable, Action<IEntityCondition<TEntity>>? config, IUnitOfWork unitOfWork) where TEntity : class, IBaseEntity
        {
            if (config is null)
            {
                return queryable;
            }
            var condition = new EntityConfition<TEntity>();
            config.Invoke(condition);

            return queryable
                .ObjectId(condition._objectId, unitOfWork.Model())
                .Condition(condition._condition)
                .Paginate(condition._pagination)
                .IncludeDeleted(condition._isDeletedIncluded)
                .Traking(condition._asNoTraking);
        }


        public static IQueryable<TEntity> Condition<TEntity>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> conditionExpression)
            where TEntity : IBaseEntity
        {
            if (conditionExpression is null) { return query; }
            return query.Where(conditionExpression);
        }


        public static IQueryable<TEntity> IncludeDeleted<TEntity>(this IQueryable<TEntity> query,
            bool isDeletedIncluded)
        where TEntity : IBaseEntity
        {
            return isDeletedIncluded ? query : query.Where(x => x.IsDeleted != true);
        }


        public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query,
            Pagination pagination)
        {
            if (pagination is null) return query;
            return pagination.Take == 0 ? query : query.Skip(pagination.Skip).Take(pagination.Take);
        }


        public static IQueryable<TEntity> Traking<TEntity>(this IQueryable<TEntity> query,
            bool asNoTraking)   where TEntity : class, IBaseEntity
        {
            return (IQueryable<TEntity>)(!asNoTraking ? query : query.AsNoTracking());
        }


        public static IQueryable<TEntity> ObjectId<TEntity>(this IQueryable<TEntity> query,
            object id, IModel model)
            where TEntity : IBaseEntity
        {

            if (id is null || model is null)
            {
                return query;
            }

            var entityType = model.FindEntityType(typeof(TEntity));
            var primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();
            var primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primayKeyValue = null;

            try
            {
                primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }


            var pe = Expression.Parameter(typeof(TEntity), "entity");
            var expressionTree = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(Expression.Property(pe, primaryKeyName),
                    Expression.Constant(primayKeyValue, primaryKeyType)), new[] { pe });

            return query.Condition(expressionTree);

        }


        //public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, string propertyName, string sortDirection) where TEntity : class
        //{
        //    return query.OrderByMultipleColumns(propertyName);
        //    //if (string.IsNullOrEmpty(propertyName))
        //    //{
        //    //    return query;
        //    //}
        //    //var methodName = sortDirection == "Asc" ? "OrderBy" : "OrderByDescending";
        //    //var type = typeof(TEntity);
        //    //var property = type.GetProperty(propertyName);
        //    //var parameter = Expression.Parameter(type, "p");
        //    //var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        //    //var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        //    //var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType },
        //    //    query.Expression, Expression.Quote(orderByExpression));
        //    //return query.Provider.CreateQuery<TEntity>(resultExpression);
        //}

        public static IOrderedQueryable<TEntity> OrderByMultipleColumns<TEntity>(this IQueryable<TEntity> query, string propertyNames = "Id")
        {
            var orderdQuery = query.OrderBy(propertyNames);
            return orderdQuery;
        }


        public static IQueryable<TEntity> ApplyPermission<TEntity>(this IQueryable<TEntity> query1, IUnitOfWork UnitOfWork, ICurrentUserAccessor _currentUserAccessor, bool checkOwner, bool checkPermision) where TEntity : class, IBaseEntity
        {
            var entity = UnitOfWork.Model().FindEntityType(typeof(TEntity).FullName);
            var schema = entity.GetSchema();
            var tableName = entity.GetTableName();

            string query = string.Empty;

            if (checkOwner)
            {
                query = $" join " +
                        $"[admin].[Roles] r on t.OwnerRoleId = r.Id " +
                        $"where r.LevelCode like '{_currentUserAccessor.GetRoleLevelCode()}%' ";
            }

            if (checkPermision)
            {
                query += checkOwner ? " or " : " where ";
                var getpermissions = $"select PermissionId from admin.RolePermissions where RoleId={_currentUserAccessor.GetRoleId()} and IsDeleted=0";

                string strPermision = "(";
                using (var cmd = UnitOfWork.DbContext().Database.GetDbConnection().CreateCommand())
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText = getpermissions;
                    cmd.CommandType = CommandType.Text;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string? per = reader["PermissionId"].ToString();

                        if (!string.IsNullOrEmpty(per))
                        {
                            strPermision += per + ",";
                        }
                    }
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
                strPermision = strPermision.Remove(strPermision.Length - 1);

                strPermision += ")";
                if (strPermision == ")")
                {
                    throw new Exception("شما هیچ دسترسی ندارید.");
                }
                var permission = "select distinct pc.Id, pc.Condition from admin.Permissions p join " +
                    $"admin.PermissionConditions pc on p.Id = pc.PermissionId and pc.TableName = '{schema}.{tableName}'  " +
                    $" where p.Id in {strPermision}";

                bool hasPermission = false;
                string conditions = "";
                using (var cmd = UnitOfWork.DbContext().Database.GetDbConnection().CreateCommand())
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText = permission;
                    cmd.CommandType = CommandType.Text;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string? con = reader["Condition"].ToString();
                        string? id = reader["Id"].ToString();
                        if (!string.IsNullOrEmpty(id))
                        {
                            hasPermission = true;
                        }
                        if (!string.IsNullOrEmpty(con))
                        {
                            conditions += con + " or ";
                        }
                    }
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }

                query += $"{conditions}";
                query = query.EndsWith("or ") ? query[..^4] : query;
                query = query.EndsWith("where ") ? query + "1<>1" : query;
                query = $"select t.* from {schema}.[{tableName}] t " + query;

                return UnitOfWork.Set<TEntity>().FromSqlRaw(query);
            }
            else
            {
                query = $"select t.* from {schema}.[{tableName}] t " + query;
                return UnitOfWork.Set<TEntity>().FromSqlRaw(query);
            }

        }


    }
}