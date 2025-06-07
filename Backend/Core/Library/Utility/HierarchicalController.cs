using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Attributes;
using Library.Common;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Library.Utility
{
    public class HierarchicalController : IHierarchicalController
    {
        private readonly IUnitOfWork _unitOfWork;
        public IModel Model { get; set; }
        private string LastSideLevelCode { get; set; }
        public HierarchicalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this.Model = _unitOfWork.Model();
        }

        public async Task<TEntity> SetLevelCode<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (typeof(IHierarchicalBaseEntity).IsAssignableFrom(typeof(TEntity)))
            {
                if (/*_unitOfWork.State<TEntity>() == EntityState.Added*/ true)
                {
                    if (((IHierarchicalBaseEntity)entity).ParentId != default)
                    {
                        var parentLevelCode = (await _unitOfWork.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == ((IHierarchicalBaseEntity)entity).ParentId) as IHierarchicalBaseEntity)?.LevelCode;
                        var previouseLevelCode = (await _unitOfWork.Set<TEntity>().AsNoTracking().OrderBy(x => x.Id)
                            .LastOrDefaultAsync(x => (x as IHierarchicalBaseEntity).ParentId == ((IHierarchicalBaseEntity)entity).ParentId) as IHierarchicalBaseEntity)?.LevelCode;

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
                            ((IHierarchicalBaseEntity)entity).LevelCode = LastSideLevelCode;
                        }
                        else
                        {
                            ((IHierarchicalBaseEntity)entity).LevelCode = $"{parentLevelCode}0001";
                        }
                    }
                    else if ((entity as IHierarchicalBaseEntity).LevelCode.Length > 4)
                    {
                        var previouseLevelCode = (await _unitOfWork.Set<TEntity>().AsNoTracking().OrderBy(x => x.Id)
                            .LastOrDefaultAsync(x => (x as IHierarchicalBaseEntity).ParentId == null || (x as IHierarchicalBaseEntity).ParentId == 0) as IHierarchicalBaseEntity)?.LevelCode;
                        var i = int.Parse(previouseLevelCode);
                        var newLevelCode = $"{++i:0000}";

                        ((IHierarchicalBaseEntity)entity).LevelCode = newLevelCode;
                    }
                }
            }

            return entity;
        }

        public async Task<TEntity> UpdateLevelCode<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (typeof(IHierarchicalBaseEntity).IsAssignableFrom(typeof(TEntity)))
            {
                var aparentIdHasChanged = ((IHierarchicalBaseEntity)entity).ParentId != (await _unitOfWork.Set<TEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == entity.Id) as IHierarchicalBaseEntity)?.ParentId;

                var lastLevelCode = ((IHierarchicalBaseEntity)entity).LevelCode;
                if (aparentIdHasChanged)
                {
                    var res = await SetLevelCode(entity);
                    await _unitOfWork.SaveAsync(new CancellationToken());
                    foreach (var child in _unitOfWork.Set<TEntity>()
                        .Where(x => ((IHierarchicalBaseEntity)x).LevelCode.StartsWith(lastLevelCode)))
                    {
                        var childLevelCode = ((IHierarchicalBaseEntity)child).LevelCode;
                        var newLevelCode = ((IHierarchicalBaseEntity)res).LevelCode +
                                           childLevelCode.Remove(0, ((IHierarchicalBaseEntity)res).LevelCode.Length);
                        ((IHierarchicalBaseEntity)child).LevelCode = newLevelCode;
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

        public TEntity DeleteChildren<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (typeof(IHierarchicalBaseEntity).IsAssignableFrom(typeof(TEntity)))
            {
                foreach (var child in _unitOfWork.Set<TEntity>().Where(x =>
                             ((IHierarchicalBaseEntity)x).LevelCode.StartsWith(((IHierarchicalBaseEntity)entity)
                                 .LevelCode)))
                {
                    DeleteUniqueProperty(child);
                }
            }

            return entity;
        }

        public TEntity DeleteUniqueProperty<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (typeof(TEntity).GetCustomAttributes(false).Any(x => x.GetType() == typeof(HasUniqueIndex)))
            {
                foreach (var propertyInfo in typeof(TEntity).GetProperties().Where(x => x.GetCustomAttributes(false).Any(x => x.GetType() == typeof(UniqueIndex))))
                {
                    var findEntityType = Model.FindEntityType(typeof(TEntity));
                    var maxChar = 0;
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        maxChar = findEntityType.GetProperty(propertyInfo.Name).GetMaxLength() - (propertyInfo.GetValue(entity) as string)?.Length ?? 0;
                    }

                    propertyInfo.SetValue(entity, propertyInfo.GetValue(entity) + RandomMaker.GenerateRandomStringWithDash(maxChar));
                }
            }

            return entity;
        }
    }

    public interface IHierarchicalController
    {
        public TEntity DeleteChildren<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        TEntity DeleteUniqueProperty<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        Task<TEntity> SetLevelCode<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        Task<TEntity> UpdateLevelCode<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
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
}