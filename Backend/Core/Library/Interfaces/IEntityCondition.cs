using System;
using System.Linq.Expressions;
using Library.Common;
using Library.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Library.Interfaces
{
    public interface IEntityCondition<TEntity> where TEntity : IBaseEntity
    {
        IEntityCondition<TEntity> ConditionExpression(Expression<Func<TEntity, bool>> conditionExpression);
        IEntityCondition<TEntity> Paginate(Pagination pagination);
        IEntityCondition<TEntity> IsDeletedIncluded(bool isDeletedIncluded);
        IEntityCondition<TEntity> AsNoTraking(bool asNoTraking);
        IEntityCondition<TEntity> ObjectId(object objectId, IModel entityType = default);

    }
}