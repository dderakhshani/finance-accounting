using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Eefa.Common.Common.Exceptions
{
    public static class PermissionForListExtention
    {
        public static List<T> ApplyPermission<T, TEntity>(this List<T> input, IUnitOfWork UnitOfWork, ICurrentUserAccessor _currentUserAccessor,
            bool checkOwner, bool checkPermision) where TEntity : class, IBaseEntity where T : PermissionForListModel
        {
            var entity = UnitOfWork.Model().FindEntityType(typeof(TEntity).FullName);
            var schema = entity.GetSchema();
            var tableName = entity.GetTableName();

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



            string query = string.Empty;
            List<int> rolesId = new();
            List<T> result = new();
            if (checkOwner)
            {
                query = $"select r.* from admin.Roles r left join {schema}.{tableName} a on a.Id=r.Title " +
                                           $"where r.LevelCode like '{_currentUserAccessor.GetRoleLevelCode()}%' ";

                rolesId = UnitOfWork.Set<TEntity>().FromSqlRaw(query).Select(a => a.Id).ToList();
                if (checkPermision)
                {

                    string conditions = CreatConditions(UnitOfWork, strPermision, schema, tableName);


                    result = input.Join(rolesId, i => i.OwnerRoleId, r => r, (i, r) => new { i, r }).AsQueryable()
                     .Where(conditions)
                     .Select(a => a.i).ToList();
                    input = result;
                }
                else
                {

                    result = input.Join(rolesId, i => i.OwnerRoleId, r => r, (i, r) => new { i, r })
                     .Select(a => a.i).ToList();
                    input = result;
                }
            }

            else if (checkPermision)
            {
                string conditions = CreatConditions(UnitOfWork, strPermision, schema, tableName);
                result = input.AsQueryable().Where(conditions).ToList();
                input = result;

            }

            return input;
        }



        private static string CreatConditions(IUnitOfWork UnitOfWork, string strPermision, string schema, string tableName)
        {

            var permission = "select distinct pc.Id, pc.Condition from admin.Permissions p join " +
                $"admin.PermissionConditions pc on p.Id = pc.PermissionId and pc.TableName = '{schema}.{tableName}'  " +
                $" where p.Id in {strPermision}";


            //List<string> conditions = new List<string>();
            bool hasPermission = false;
            string conditions = "t => ";
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
                    conditions += reader["Condition"].ToString()?.Replace(" <> ", " != ").Replace(" = ", " == ")
                        .Replace(" is null", " == null ").Replace(" is null ", " == null ").Replace(" and ", " && ").Replace(" or ", " || ") + " || ";
                    string id = reader["Id"].ToString();
                }
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            conditions = conditions.EndsWith("|| ") ? conditions[..^4] : conditions;
            conditions = conditions.EndsWith("t => ") ? conditions += "false" : conditions;
            return conditions;
        }
    }
}
