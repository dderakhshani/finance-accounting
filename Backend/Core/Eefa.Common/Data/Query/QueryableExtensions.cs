using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Common.Data.Query
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(IUnitOfWork unitOfWork, ICurrentUserAccessor currentUserAccessor) where TEntity : class, IBaseEntity
        {
            return unitOfWork.Set<TEntity>();
        }


        public static IQueryable<TEntity> QueryBuilder<TEntity>(this IQueryable<TEntity> queryable, Action<IEntityCondition<TEntity>>? config) where TEntity : class, IBaseEntity
        {
            if (config is null)
            {
                return queryable;
            }
            var condition = new EntityCondition<TEntity>();
            config.Invoke(condition);

            return queryable
                //.ObjectId(condition._objectId)
                .Condition(condition._condition)
                .Paginate(condition._pagination)
                .IncludeDeleted(condition._isDeletedIncluded)
                .Traking(condition._asNoTraking);
        }


        public static IQueryable<TEntity> Condition<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> conditionExpression)
            where TEntity : IBaseEntity
        {
            if (conditionExpression is null) { return query; }
            return query.Where(conditionExpression);
        }


        public static IQueryable<TEntity> IncludeDeleted<TEntity>(this IQueryable<TEntity> query, bool isDeletedIncluded) where TEntity : IBaseEntity
        {
            return isDeletedIncluded ? query : query.Where(x => x.IsDeleted != true);
        }


        public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query, Pagination pagination)
        {
            if (pagination is null) return query;
            return pagination.Take == 0 ? query : query.Skip(pagination.Skip).Take(pagination.Take);
        }


        public static IQueryable<TEntity> Traking<TEntity>(this IQueryable<TEntity> query, bool asNoTraking) where TEntity : class, IBaseEntity
        {

            return (IQueryable<TEntity>)(!asNoTraking ? query : query.AsNoTracking());
        }


        public static IOrderedQueryable<TEntity> OrderByMultipleColumns<TEntity>(this IQueryable<TEntity> query, string propertyNames = "Id")
        {
            var orderdQuery = query.OrderBy(propertyNames);
            return orderdQuery;
        }
        public static IQueryable<TEntity> FilterQuery<TEntity>(this IQueryable<TEntity> query, List<QueryCondition>? conditions)
        {
            if (conditions is null || conditions.Count == 0) return query;

            string queryString = "x => ";
            var uniquePropertyNamesGettingFiltered = conditions.Select(x => x.PropertyName).Distinct().ToList();
            var doesConditionsHaveMultipleQueryOnSameProperty = uniquePropertyNamesGettingFiltered.Any(x => conditions.Where(y => y.PropertyName == x).ToList().Count > 1);
            if (doesConditionsHaveMultipleQueryOnSameProperty)
            {
                foreach (var condition in conditions)
                {
                    if (conditions.Where(x => x.PropertyName == condition.PropertyName).ToList().Count == 1) condition.NextOperand = "and";
                }
            }
            foreach (var propertyName in uniquePropertyNamesGettingFiltered)
            {
                var propertyConditions = conditions.Where(x => x.PropertyName == propertyName).ToList();
                if (propertyConditions.Count > 1)
                {
                    if (queryString.Trim().EndsWith("or"))
                    {
                        queryString = queryString.Trim();
                        queryString = queryString.Remove(queryString.Length - 2, 2);
                        queryString += "and";
                    }
                    queryString += " (";
                }
                foreach (var condition in propertyConditions)
                {
                    ChangeConditionValuesToUTCIfPropertyIsOFTypeDateTime(condition, typeof(TEntity));
                    TrimValuesIfPropertyTypeIsOfTypeString(condition, typeof(TEntity));
                    SetPropertyNameToCamelCase(condition);


                    if (condition.Comparison == "between" && condition.Values?.Length > 0) queryString += ApplyBetweenCondition(condition);
                    else if (condition.Comparison == "startsWith" && condition.Values?.Length > 0) queryString += ApplyStartsWithCondition(condition);
                    else if (condition.Comparison == "endsWith" && condition.Values?.Length > 0) queryString += ApplyEndsWithCondition(condition);
                    else if (condition.Comparison == "in" && condition.Values?.Length > 0) queryString += ApplyInCondition(condition);
                    else if (condition.Comparison == "notIn" && condition.Values?.Length > 0) queryString += ApplyNotInCondition(condition);
                    else if (condition.Comparison == "inList" && condition.Values?.Length > 0) queryString += ApplyInListCondition(condition);
                    else if (condition.Comparison == "ofList" && condition.Values?.Length > 0) queryString += ApplyOfListCondition(condition);
                    else if (condition.Comparison == ".Contains" && condition.Values?.Length > 0) queryString += ApplyContainsCondition(condition);
                    else if (condition.Comparison == "notContains" && condition.Values?.Length > 0) queryString += ApplyNotContainsCondition(condition);
                    else if (condition.Values?.Length > 0)
                    {
                        ConvertConditionValueToStringIfNotBoolean(condition);
                        queryString += ApplyEqualsAndNotEqualsCondition(condition, typeof(TEntity));
                    }
                }
                if (propertyConditions.Count > 1 && (queryString.Trim().EndsWith("or") || queryString.Trim().EndsWith("and")))
                {
                    queryString = queryString.Trim();
                    var charsToRemove = queryString.EndsWith("or") ? 3 : 4;
                    queryString = queryString.Remove(queryString.Length - charsToRemove, charsToRemove);
                    queryString += ") and ";
                }
            }

            if (queryString.Trim().EndsWith("or") || queryString.Trim().EndsWith("and"))
            {
                queryString = queryString.Trim();
                var charsToRemove = queryString.EndsWith("or") ? 3 : 4;
                queryString = queryString.Remove(queryString.Length - charsToRemove, charsToRemove);
            }

            query = query.Where(queryString);

            return query;
        }

        private static void ChangeConditionValuesToUTCIfPropertyIsOFTypeDateTime(QueryCondition condition, Type entityType)
        {
            var propertyType = entityType.GetProperty(condition.PropertyName.Substring(0, 1).ToUpper() + condition.PropertyName.Substring(1, condition.PropertyName.Length - 1))?.PropertyType;
            if (propertyType != null && propertyType == typeof(DateTime))
            {
                condition.Values = condition.Values.Select(x => ((DateTime)x).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ")).ToArray();
            }
        }
        private static void TrimValuesIfPropertyTypeIsOfTypeString(QueryCondition condition, Type entityType)
        {
            var propertyType = entityType.GetProperty(condition.PropertyName.Substring(0, 1).ToUpper() + condition.PropertyName.Substring(1, condition.PropertyName.Length - 1))?.PropertyType;
            if (propertyType != null && propertyType == typeof(string))
            {
                condition.Values = condition.Values.Select(x => x?.ToString()?.Trim()).ToArray();
            }
        }
        private static void SetPropertyNameToCamelCase(QueryCondition condition)
        {
            condition.PropertyName = string.Concat(condition.PropertyName[0].ToString().ToUpper(),
                       condition.PropertyName.AsSpan(1));
        }
        private static string ApplyBetweenCondition(QueryCondition condition)
        {
            return $"(x.{condition.PropertyName} >= \"{condition.Values[0]}\" && x.{condition.PropertyName} <= \"{condition.Values[1]}\") {condition.NextOperand} ";
        }
        private static string ApplyStartsWithCondition(QueryCondition condition)
        {
            return $"x.{condition.PropertyName}.StartsWith(\"{condition.Values[0]}\")  {condition.NextOperand} ";
        }
        private static string ApplyEndsWithCondition(QueryCondition condition)
        {
            return $"x.{condition.PropertyName}.EndsWith(\"{condition.Values[0]}\"')  {condition.NextOperand} ";
        }
        private static string ApplyInCondition(QueryCondition condition)
        {
            var queryString = "";
            for (var i = 0; i < condition.Values.Length; i += 1)
            {
                if (i == 0)
                {
                    queryString += "(";
                }

                queryString += $"x.{condition.PropertyName} == {condition.Values[i]}" + (i + 1 < condition.Values.Length ? " or " : ")");

            }
            queryString += $" {condition.NextOperand} ";
            return queryString;
        }
        private static string ApplyInListCondition(QueryCondition condition)
        {
            var queryString = "";
            queryString += "(";
            for (var i = 0; i < condition.Values.Length; i += 1)
            {
                try
                {
                    queryString += $"x.{condition.PropertyName}.Contains({condition.Values[i]}) and ";
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            if (queryString.EndsWith("and "))
            {
                queryString = queryString.Remove(queryString.Length - 5, 5);
            }
            queryString += ")";
            queryString += $" {condition.NextOperand} ";

            return queryString;
        }
        private static string ApplyOfListCondition(QueryCondition condition)
        {
            var queryString = "";
            queryString += "(";
            for (var i = 0; i < condition.Values.Length; i += 1)
            {
                try
                {
                    queryString += $"x.{condition.PropertyName} = {condition.Values[i]} or ";
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            if (queryString.EndsWith(" or "))
            {
                queryString = queryString.Remove(queryString.Length - 4, 4);
            }
            queryString += ")";
            queryString += $" {condition.NextOperand} ";
            return queryString;
        }
        private static string ApplyContainsCondition(QueryCondition condition)
        {
            return $"x.{condition.PropertyName}{condition.Comparison}(\"{condition.Values[0]}\") {condition.NextOperand} ";
        }
        private static string ApplyNotContainsCondition(QueryCondition condition)
        {
            return $"!x.{condition.PropertyName}.Contains(\"{condition.Values[0]}\") {condition.NextOperand} ";
        }
        private static void ConvertConditionValueToStringIfNotBoolean(QueryCondition condition)
        {
            var isConditionValueBoolean = (condition.Values[0].ToString().ToLower().Equals("true") || condition.Values[0].ToString().ToLower().Equals("false"));
            if (!isConditionValueBoolean) condition.Values[0] = condition.Values[0] != null && condition.Values[0].ToString() != "null" ? $"\"{condition.Values[0]}\"" : $"{condition.Values[0]}";
        }
        private static string ApplyEqualsAndNotEqualsCondition(QueryCondition condition, Type entityType)
        {
            var queryString = "";
            var propertyType = entityType.GetProperty(condition.PropertyName.Substring(0, 1).ToUpper() + condition.PropertyName.Substring(1, condition.PropertyName.Length - 1))?.PropertyType;

            if (propertyType != null && propertyType == typeof(DateTime) && condition.Comparison == "==")
            {
                queryString += $"x.{condition.PropertyName} >= {condition.Values[0]}  &&  {condition.PropertyName} < \"{DateTime.Parse(condition.Values[0].ToString()).AddDays(1)}\" {condition.NextOperand} ";
            }
            else
            {
                queryString += $"x.{condition.PropertyName} {condition.Comparison} {condition.Values[0]} {condition.NextOperand} ";
            }

            return queryString;
        }
        private static string ApplyNotInCondition(QueryCondition condition)
        {
            var queryString = "";
            queryString += "(";
            for (var i = 0; i < condition.Values.Length; i += 1)
            {
                try
                {
                    queryString += $"x.{condition.PropertyName} != {condition.Values[i]} and ";
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            if (queryString.EndsWith(" and "))
            {
                queryString = queryString.Remove(queryString.Length - 5, 5);
            }
            queryString += ")";
            queryString += $" {condition.NextOperand} ";
            return queryString;
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
                string strPermision = "(";
                var getpermissions = $"select PermissionId from admin.RolePermissions where RoleId={_currentUserAccessor.GetRoleId()} and IsDeleted=0";
                using (var cmd = UnitOfWork.DbContex().Database.GetDbConnection().CreateCommand())
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


                //List<string> conditions = new List<string>();
                bool hasPermission = false;
                string conditions = "";
                using (var cmd = UnitOfWork.DbContex().Database.GetDbConnection().CreateCommand())
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

        //public static IQueryable<TEntity> FilterQuery<TEntity>(this IQueryable<TEntity> query, List<QueryCondition>? queries)
        //{
        //    if (queries is null)
        //    {
        //        return query;
        //    }

        //    foreach (var condition in queries)
        //    {
        //        condition.PropertyName = string.Concat(condition.PropertyName[0].ToString().ToUpper(),
        //            condition.PropertyName.AsSpan(1));

        //        if (condition.Comparison == "between")
        //        {
        //            query = query.Where($"{condition.PropertyName} >= @0 && {condition.PropertyName} <= @1",
        //                condition.Values);
        //        }

        //        else if (condition.Comparison == "startsWith")
        //        {
        //            var e = Expression.Parameter(typeof(TEntity), "e");
        //            var pi = typeof(TEntity).GetProperty(condition.PropertyName);
        //            var m = Expression.MakeMemberAccess(e, pi);
        //            var c = Expression.Constant(condition.Values[0], typeof(string));
        //            var mi = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        //            Expression call = Expression.Call(m, mi, c);

        //            var lambda = Expression.Lambda<Func<TEntity, bool>>(call, e);
        //            query = query.Where(lambda);
        //        }

        //        else if (condition.Comparison == "endsWith")
        //        {
        //            var e = Expression.Parameter(typeof(TEntity), "e");
        //            var pi = typeof(TEntity).GetProperty(condition.PropertyName);
        //            var m = Expression.MakeMemberAccess(e, pi);
        //            var c = Expression.Constant(condition.Values[0], typeof(string));
        //            var mi = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
        //            Expression call = Expression.Call(m, mi, c);

        //            var lambda = Expression.Lambda<Func<TEntity, bool>>(call, e);
        //            query = query.Where(lambda);
        //        }
        //        else if (condition.Comparison == "in")
        //        {
        //            Expression<Func<TEntity, bool>> expression = entity => false;

        //            for (var i = 0; i < condition.Values.Length; i += 1)
        //            {
        //                try
        //                {
        //                    expression = ExpressionUtils.BuildPredicate(expression, condition.PropertyName, "equal", condition.Values[i], "or");

        //                }
        //                catch (Exception e)
        //                {
        //                    // ignored
        //                }
        //            }

        //            query = query.Where(expression);

        //        }
        //        else if (condition.Comparison == "inList")
        //        {
        //            string where = "x => ";
        //            for (var i = 0; i < condition.Values.Length; i += 1)
        //            {
        //                try
        //                {
        //                    where += $"x.{condition.PropertyName}.Contains({condition.Values[i]}) and ";
        //                }
        //                catch (Exception e)
        //                {
        //                    // ignored
        //                }
        //            }

        //            if (where.EndsWith("and "))
        //            {
        //                where = where.Remove(where.Length - 5, 5);
        //            }

        //            query = query.Where(where);
        //        }
        //        else if (condition.Comparison == "ofList")
        //        {
        //            string where = "x => ";
        //            for (var i = 0; i < condition.Values.Length; i += 1)
        //            {
        //                try
        //                {
        //                    where += $"x.{condition.PropertyName}.Contains({condition.Values[i]}) or ";
        //                }
        //                catch (Exception e)
        //                {
        //                    // ignored
        //                }
        //            }

        //            if (where.EndsWith("or "))
        //            {
        //                where = where.Remove(where.Length - 4, 4);
        //            }

        //            query = query.Where(where);
        //        }
        //        else
        //        {
        //            query = query.Where($"{condition.PropertyName} {condition.Comparison}(@0)",
        //                condition.Values);
        //        }
        //    }

        //    return query;
        //}

        public static async Task<PagedList<TSource>> ToPagedList<TSource>(
       this IQueryable<TSource> query,
       PaginatedQueryModel queryParam)
        {
            if (query == null)
                throw new Exception("Query can't be null.");

            query = query.FilterQuery(queryParam.Conditions);
            query = query.OrderByMultipleColumns(queryParam.OrderByProperty);

            var result = await query
                .Paginate(queryParam.Paginator())
                .ToListAsync();

            int totalCount = queryParam.PageIndex <= 1
                ? await query.CountAsync()
                : 0;

            return new PagedList<TSource>
            {
                Data = result,
                TotalCount = totalCount
            };
        }

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AccessLimitControlAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Schema { get; set; }

        public AccessLimitControlAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AccessPermissionControlAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Schema { get; set; }

        public AccessPermissionControlAttribute(string name)
        {
            Name = name;
        }
    }
}
