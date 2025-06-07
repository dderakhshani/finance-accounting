using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace Library.Utility
{
    public static class QueryUtility
    {
        public static string SqlParametersToQuey(ICollection<SqlParameter> sqlParameters)
        {
            var query = sqlParameters.Aggregate(string.Empty, (current, sqlParameter) => current + $"@{sqlParameter.ParameterName},");

            if (query.EndsWith(','))
            {
                query = query[..^1];
            }

            return query;
        }

        public static SqlParameter[] EntityToSqlParameters<TEntity>(this TEntity entity)
        {
            var sqlParameters = new List<SqlParameter>();

            if (entity != null)
            {
                foreach (var propertyInfo in entity.GetType().GetProperties())
                {
                    var parameter = new SqlParameter()
                    {
                        ParameterName = propertyInfo.Name,
                        Value = propertyInfo.GetValue(entity) ?? Convert.DBNull,
                    };

                    if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                    {
                        parameter.SqlDbType = System.Data.SqlDbType.Int;
                    }
                    else if (propertyInfo.PropertyType == typeof(string))
                    {
                        parameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    }
                    else if (propertyInfo.PropertyType == typeof(long) || propertyInfo.PropertyType == typeof(long?))
                    {
                        parameter.SqlDbType = System.Data.SqlDbType.BigInt;
                    }
                    else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                    {
                        parameter.SqlDbType = System.Data.SqlDbType.Bit;
                    }
                    else if (propertyInfo.PropertyType == typeof(DateTime) ||
                             propertyInfo.PropertyType == typeof(DateTime?))
                    {
                        parameter.SqlDbType = System.Data.SqlDbType.DateTime2;
                    }

                    sqlParameters.Add(parameter);
                }
            }

            return sqlParameters.ToArray();
        }
    }
}