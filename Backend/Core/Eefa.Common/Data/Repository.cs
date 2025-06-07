using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Common.Exceptions;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;

namespace Eefa.Common.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        //protected readonly IUnitOfWork UnitOfWork;
        protected readonly ICurrentUserAccessor _currentUserAccessor;
        protected readonly IHierarchicalManager<TEntity> _hierarchicalManager;
        //private readonly ADOConnection<TEntity> _AdoConnection;
        protected readonly IValidationErrorManager _validationErrorManager;


        public IUnitOfWork UnitOfWork { get; set; }

        public Repository(IUnitOfWork unitOfWork,
            ICurrentUserAccessor currentUserAccessor, IHierarchicalManager<TEntity> hierarchicalManager, IValidationErrorManager validationErrorManager)
        {

            _currentUserAccessor = currentUserAccessor;
            _hierarchicalManager = hierarchicalManager;
            _validationErrorManager = validationErrorManager;
            UnitOfWork = unitOfWork;
            //_AdoConnection = adoConnection;
        }


        public virtual TEntity Delete(TEntity entity)
        {
            if (entity is null) _validationErrorManager.Throw<TEntityIsEmpity>();
            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedById = _currentUserAccessor.GetId();

            //If a property has UniqueIndex: add random string at end of its value to be reusable 
            entity = _hierarchicalManager.DeleteUniqueProperty(entity);
            _hierarchicalManager.DeleteChildren(entity);

            return UnitOfWork.Set<TEntity>().Update(entity).Entity;
        }


        public async Task<bool> Exist(Action<IEntityCondition<TEntity>> config = null)
        {
            return await AsQueryable().QueryBuilder(config).AnyAsync();
        }

        public async Task<TEntity> Find(int Id)
        {
            return await UnitOfWork.Set<TEntity>().FindAsync(Id);
        }

        public IQueryable<TEntity> GetAll(Action<IEntityCondition<TEntity>> config)
        {
            return AsQueryable().QueryBuilder(config);
            //return AsQueryable().QueryBuilder(config);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return UnitOfWork.Set<TEntity>();
        }

        public virtual TEntity Insert(TEntity entity)
        {
            if (entity is null)
                _validationErrorManager.Throw<TEntityIsEmpity>();
            entity.ModifiedAt = DateTime.UtcNow;
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedById = _currentUserAccessor.GetId();
            entity.IsDeleted = false;
            entity.OwnerRoleId = _currentUserAccessor.GetRoleId();


            if (typeof(IHierarchical).IsAssignableFrom(typeof(TEntity)))
            {
                ((IHierarchical)entity).LevelCode = "0";
            }

            return UnitOfWork.Set<TEntity>().Add(entity).Entity;
        }



        public virtual TEntity InsertBackgroundTransaction(TEntity entity)
        {
            if (entity is null)
                _validationErrorManager.Throw<TEntityIsEmpity>();
            entity.ModifiedAt = DateTime.UtcNow;
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedById =1;
            entity.IsDeleted = false;
            entity.OwnerRoleId = 1;


            if (typeof(IHierarchical).IsAssignableFrom(typeof(TEntity)))
            {
                ((IHierarchical)entity).LevelCode = "0";
            }

            return UnitOfWork.Set<TEntity>().Add(entity).Entity;
        }



        public virtual void AddRange(List<TEntity> entites)
        {
            if (entites is null)
                _validationErrorManager.Throw<TEntityIsEmpity>();
            foreach (var entity in entites)
            {
                entity.ModifiedAt = DateTime.UtcNow;
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedById = _currentUserAccessor.GetId();
                entity.IsDeleted = false;
                entity.OwnerRoleId = _currentUserAccessor.GetRoleId();
                if (typeof(IHierarchical).IsAssignableFrom(typeof(TEntity)))
                {
                    ((IHierarchical)entity).LevelCode = "0";
                }

            }
            UnitOfWork.Set<TEntity>().AddRange(entites);


        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            
                var res = await UnitOfWork.SaveChangesAsync(cancellationToken);
                return res;
           
        }

        public virtual async Task<int> SaveChangesAsync(int formId, CancellationToken cancellationToken = default)
        {
            var res = await UnitOfWork.SaveChangesAsync(cancellationToken);
            return res;
        }

        
        public virtual async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            var res = await UnitOfWork.SaveAsync(cancellationToken);
            return res;
        }
        
        public virtual TEntity Update(TEntity entity)
        {
            entity.ModifiedById = _currentUserAccessor.GetId();
            entity.ModifiedAt = DateTime.UtcNow;
            //Update Level if entity is type of IHierarchical
            entity = _hierarchicalManager.UpdateLevelCode(entity).GetAwaiter().GetResult();

            return UnitOfWork.Set<TEntity>().Update(entity).Entity;
        }


        public virtual IQueryable<TEntity> GetAllWithPermission(bool checkOwner, bool checkPermision)
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
                strPermision.Remove(strPermision.Length - 1);
                strPermision += ")";
                query += $" t.PermissionId in {strPermision} or ";
                query = query.EndsWith("or ") ? query[..^4] : query;
            }

            query = $"select t.* from {schema}[{tableName}] t " + query;

            return UnitOfWork.Set<TEntity>().FromSqlRaw(query);

        }


        public virtual async Task<IQueryable<TEntity>> WithPermissionAsync(bool checkOwner, bool checkPermision)
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



                var condition = await UnitOfWork.Set<TEntity>().FromSqlRaw(permission).ToListAsync();

                query += $" t.PermissionId in {strPermision} or ";
                query = query.EndsWith("or ") ? query[..^4] : query;
            }

            query = $"select t.* from {schema}[{tableName}] t " + query;

            return UnitOfWork.Set<TEntity>().FromSqlRaw(query);

        }



        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await UnitOfWork.BeginTransactionAsync(cancellationToken);
        }

        public void CommitTransaction(IDbContextTransaction transaction)
        {
            transaction.Commit();
        }

        public void RollbackTransaction(IDbContextTransaction transaction)
        {
            transaction.Rollback();
        }

    }
}
