using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Library.Exceptions
{
    public class PermissionExtention
    {
        public string GetPermissions<TEntity>(IUnitOfWork UnitOfWork, ICurrentUserAccessor _currentUserAccessor) where TEntity : class, IBaseEntity
        {
            var entity = UnitOfWork.Model().FindEntityType(typeof(TEntity).FullName);
            var schema = entity.GetSchema();
            var tableName = entity.GetTableName();
            string strPermision = "(";
            var getpermissions = $"select PermissionId from admin.RolePermissions where RoleId={_currentUserAccessor.GetRoleId()} and IsDeleted=0";
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
            string conditions = CreatConditions(UnitOfWork, strPermision, schema, tableName);

            return conditions;
        }

        private string CreatConditions(IUnitOfWork UnitOfWork, string strPermision, string schema, string tableName)
        {

            var permission = "select distinct pc.Id, pc.Condition from admin.Permissions p join " +
                $"admin.PermissionConditions pc on p.Id = pc.PermissionId and pc.TableName = '{schema}.{tableName}'  " +
                $" where p.Id in {strPermision}";


            //List<string> conditions = new List<string>();
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
                        var ff = con.Replace("t.", "");

                        conditions += ff + " or ";
                    }
                }
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            conditions = conditions.EndsWith("or ") ? conditions[..^4] : conditions;
            conditions = conditions == "" ? conditions += "1<>1" : conditions;
            return conditions;
        }
    }
}
