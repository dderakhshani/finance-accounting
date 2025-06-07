using System.Threading.Tasks;
using Eefa.Common.Data;

namespace Eefa.Common
{
    public interface IHierarchical
    {
        public string LevelCode { get; set; }
        public int? ParentId { get; set; }
    }

    public interface IHierarchicalManager<TEntity> where TEntity : class, IBaseEntity
    {
        TEntity DeleteUniqueProperty(TEntity entity);
        Task<TEntity> SetLevelCode(TEntity entity) ;
        Task<TEntity> UpdateLevelCode(TEntity entity) ;
        TEntity DeleteChildren(TEntity entity);
    }

   
}