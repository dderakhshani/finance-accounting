using System;
using System.Linq.Expressions;
using Eefa.Common.Data.Query;

namespace Eefa.Common.Data
{
    public interface IEntityCondition<TEntity> where TEntity : IBaseEntity
    {
        IEntityCondition<TEntity> ConditionExpression(Expression<Func<TEntity, bool>> conditionExpression);
        IEntityCondition<TEntity> Paginate(Pagination pagination);
        IEntityCondition<TEntity> IsDeletedIncluded(bool isDeletedIncluded);
        IEntityCondition<TEntity> AsNoTraking(bool asNoTraking);
        //IEntityCondition<TEntity> ObjectId(object objectId, IModel entityType = default);

    }
}