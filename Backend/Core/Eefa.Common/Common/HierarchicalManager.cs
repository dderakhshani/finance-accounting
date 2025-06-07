using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Common
{
    public class HierarchicalManager<TEntity> : IHierarchicalManager<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly IUnitOfWork _unitOfWork;
        private string LastSideLevelCode { get; set; }
        public HierarchicalManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TEntity> SetLevelCode(TEntity entity)
        {
            if (typeof(IHierarchical).IsAssignableFrom(typeof(TEntity)))
            {
                if (/*_unitOfWork.State<TEntity>() == EntityState.Added*/ true)
                {
                    if (((IHierarchical)entity).ParentId != default)
                    {
                        var parentLevelCode = (await _unitOfWork.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == ((IHierarchical)entity).ParentId) as IHierarchical)?.LevelCode;
                        var previouseLevelCode = (await _unitOfWork.Set<TEntity>().AsNoTracking().OrderBy(x => x.Id)
                            .LastOrDefaultAsync(x => (x as IHierarchical).ParentId == ((IHierarchical)entity).ParentId) as IHierarchical)?.LevelCode;

                        if (previouseLevelCode != default)
                        {

                            if (LastSideLevelCode != default && !LastSideLevelCode.Substring(0, LastSideLevelCode.Length - 4)
                                .Equals(parentLevelCode))
                            {
                                LastSideLevelCode = default;
                            }


                            if (LastSideLevelCode == default)
                            {
                                LastSideLevelCode = previouseLevelCode?.Substring(parentLevelCode.Length,
                                    previouseLevelCode.Length - parentLevelCode.Length);
                            }

                            var i = LastSideLevelCode.Length > 4 ? int.Parse(LastSideLevelCode.Substring(LastSideLevelCode.Length - 4, 4)) : int.Parse(LastSideLevelCode);
                            LastSideLevelCode = $"{parentLevelCode}{++i:0000}";
                            ((IHierarchical)entity).LevelCode = LastSideLevelCode;
                        }
                        else
                        {
                            ((IHierarchical)entity).LevelCode = $"{parentLevelCode}0001";
                        }
                    }
                    else if ((entity as IHierarchical).LevelCode.Length > 4)
                    {
                        var previouseLevelCode = (await _unitOfWork.Set<TEntity>().AsNoTracking().OrderBy(x => x.Id)
                            .LastOrDefaultAsync(x => (x as IHierarchical).ParentId == null || (x as IHierarchical).ParentId == 0) as IHierarchical)?.LevelCode;
                        var i = int.Parse(previouseLevelCode);
                        var newLevelCode = $"{++i:0000}";

                        ((IHierarchical)entity).LevelCode = newLevelCode;
                    }
                }
            }

            return entity;
        }

        public async Task<TEntity> UpdateLevelCode(TEntity entity)
        {
            if (typeof(IHierarchical).IsAssignableFrom(typeof(TEntity)))
            {
                var aparentIdHasChanged = ((IHierarchical)entity).ParentId != (await _unitOfWork.Set<TEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == entity.Id) as IHierarchical)?.ParentId;

                var lastLevelCode = ((IHierarchical)entity).LevelCode;
                if (aparentIdHasChanged)
                {
                    var res = await SetLevelCode(entity);
                    await _unitOfWork.SaveChangesAsync(new CancellationToken());
                    foreach (var child in _unitOfWork.Set<TEntity>()
                        .Where(x => ((IHierarchical)x).LevelCode.StartsWith(lastLevelCode)))
                    {
                        var childLevelCode = ((IHierarchical)child).LevelCode;
                        var newLevelCode = ((IHierarchical)res).LevelCode +
                                           childLevelCode.Remove(0, ((IHierarchical)res).LevelCode.Length);
                        ((IHierarchical)child).LevelCode = newLevelCode;
                    }

                    return res;
                }
                else
                {
                    return entity;
                }
            }
            else
            {
                return entity;
            }
        }

        public TEntity DeleteChildren(TEntity entity)
        {
            try
            {
                var list = _unitOfWork.Set<TEntity>().Where(x => ((IHierarchical)x)

                                    .LevelCode.StartsWith(((IHierarchical)entity).LevelCode));
                foreach (var child in list)
                {
                    child.IsDeleted = true;
                }
            }
            catch(Exception e)
            {

            }
            

            return entity;
        }

        public TEntity DeleteUniqueProperty(TEntity entity)
        {
            var entityType = typeof(TEntity);
            if (entityType.IsDefined(typeof(HasUniqueIndex), false))
            {
                var uniqueIndexedProperties = typeof(TEntity).GetProperties().Where(x => x.GetCustomAttributes(false).Any(x => x.GetType() == typeof(UniqueIndex)));
                foreach (var propertyInfo in uniqueIndexedProperties)
                {
                    var findEntityType = _unitOfWork.Model().FindEntityType(entityType);
                    var maxChar = 0;
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        maxChar = findEntityType.GetProperty(propertyInfo.Name).GetMaxLength() - (propertyInfo.GetValue(entity) as string)?.Length ?? 0;
                    }

                    propertyInfo.SetValue(entity, propertyInfo.GetValue(entity) + RandomMaker.GenerateRandomString(maxChar));
                }
            }

            return entity;
        }
    }

    public class HierarchicalConcurrencyModel
    {
        public HierarchicalConcurrencyModel(string parentLevelCode)
        {
            this.ParentLevelCode = parentLevelCode;
        }
        public HierarchicalConcurrencyModel(string parentLevelCode, string lastSideLevelCode)
        {
            this.ParentLevelCode = parentLevelCode;
            this.LastSideLevelCode = lastSideLevelCode;
        }
        public string ParentLevelCode { get; set; }
        public string LastSideLevelCode { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HasUniqueIndex : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UniqueIndex : Attribute
    {

    }
}
