using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Common;
using Library.Interfaces;
using Library.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Eefa.Persistence.Data.SqlServer
{
    public class Repository : IRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IHierarchicalController _hierarchicalController;

        public IModel Model { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public Repository(IUnitOfWork unitOfWork,
            ICurrentUserAccessor currentUserAccessor, IHierarchicalController hierarchicalController)
        {
            _unitOfWork = unitOfWork;
            _currentUserAccessor = currentUserAccessor;
            _hierarchicalController = hierarchicalController;
            this.Model = _unitOfWork.Model();
            UnitOfWork = _unitOfWork;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var res = await _unitOfWork.SaveAsync(cancellationToken);
            return res;
        }

        public async Task<int> SaveChangesAsync(int formId, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await _unitOfWork.SaveAsync(formId, cancellationToken);
            return res;
        }

        public virtual IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IBaseEntity
        {
            return _unitOfWork.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll<TEntity>(Action<IEntityCondition<TEntity>> config) where TEntity : class, IBaseEntity
        {
            return GetQuery<TEntity>().QueryBuilder(config, _unitOfWork);
        }


        public IQueryable<TEntity> Find<TEntity>(Action<IEntityCondition<TEntity>> config)
            where TEntity : class, IBaseEntity
        {
            return GetQuery<TEntity>().QueryBuilder(config, _unitOfWork);
        }


        public async Task<bool> Exist<TEntity>(Action<IEntityCondition<TEntity>> config) where TEntity : class, IBaseEntity
        {
            return await GetQuery<TEntity>().QueryBuilder(config, _unitOfWork).AnyAsync();
        }

        public EntityEntry<TEntity> Insert<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
   
            entity.ModifiedAt = DateTime.UtcNow;
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedById = _currentUserAccessor.GetId();
            entity.IsDeleted = false;
            entity.OwnerRoleId = _currentUserAccessor.GetRoleId();

            if (typeof(IHierarchicalBaseEntity).IsAssignableFrom(typeof(TEntity)))
            {
                ((IHierarchicalBaseEntity)entity).LevelCode = "0";
            }

            return _unitOfWork.Set<TEntity>().Add(entity);
        }

        public EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            entity.ModifiedById = _currentUserAccessor.GetId();
            entity.ModifiedAt = DateTime.UtcNow;

            entity = _hierarchicalController.UpdateLevelCode(entity).GetAwaiter().GetResult();

            return _unitOfWork.Set<TEntity>().Update(entity);
        }

        public EntityEntry<TEntity> Delete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedById = _currentUserAccessor.GetId();


            entity = _hierarchicalController.DeleteUniqueProperty(entity);
            _hierarchicalController.DeleteChildren(entity);


            return _unitOfWork.Set<TEntity>().Update(entity);
        }

        public virtual IQueryable<TEntity> GetAllWithPermission<TEntity>(bool checkOwner, bool checkPermision) where TEntity : class, IBaseEntity
        {
            var entity = Model.FindEntityType(typeof(TEntity).FullName);
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

                Type myType = typeof(TEntity);
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                var PermissionProp = props.Where(a => a.Name == "PermissionId").FirstOrDefault();
                if (PermissionProp == null)
                {
                    throw new Exception("جدول مربوطه فیلد PermissionId را ندارد.");
                }
                query += checkOwner ? " or " : " where ";
                var permisions = _currentUserAccessor.GetPermissions();
                if (permisions == null || permisions.Count == 0)
                {
                    throw new Exception("شما هیچ دسترسی ندارید.");
                }
                string strPermision = "(";
                foreach (var permision in permisions)
                {
                    strPermision += permision + ",";
                }
                strPermision = strPermision.Remove(strPermision.Length - 1);
                strPermision += ")";
                query += $" t.PermissionId is null or t.PermissionId in {strPermision} or ";
                query = query.EndsWith("or ") ? query[..^4] : query;
            }

            query = $"select t.* from {schema}.[{tableName}] t " + query;

            return _unitOfWork.Set<TEntity>().FromSqlRaw(query);

        }

        public virtual async Task<IQueryable<TEntity>> WithPermissionAsync<TEntity>(bool checkOwner, bool checkPermision) where TEntity : class, IBaseEntity
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
                var permisions = _currentUserAccessor.GetPermissions();
                if (permisions == null || permisions.Count == 0)
                {
                    throw new Exception("شما هیچ دسترسی ندارید.");
                }

                string strPermision = "(";
                foreach (var permision in permisions)
                {
                    strPermision += permision + ",";
                }
                strPermision = strPermision.Remove(strPermision.Length - 1);
                strPermision += ")";

                var permission = "select distinct r.Condition   from accounting.AccountHead a left join " +
                    "(select pc.Condition,pc.IsDeleted from admin.Permissions p" +
                    "  left join  admin.PermissionConditions pc on p.Id = pc.PermissionId" +
                    "  left join admin.Tables t on pc.TableId=t.Id and t.NameEn='accounting.AccountHead' where p.Id in (1044,1043)) as r on a.Id!=r.IsDeleted";
                var lklkj = UnitOfWork.Set<TEntity>().FromSqlRaw(permission);


                var condition = await UnitOfWork.Set<TEntity>().FromSqlRaw(permission).ToListAsync();

                query += $" t.PermissionId in {strPermision} or ";
                query = query.EndsWith("or ") ? query[..^4] : query;
            }

            query = $"select t.* from {schema}[{tableName}] t " + query;

            return UnitOfWork.Set<TEntity>().FromSqlRaw(query);

        }
    }

}