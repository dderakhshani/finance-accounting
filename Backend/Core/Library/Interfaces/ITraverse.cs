using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Library.Common;

namespace Library.Interfaces
{
    public interface ITraverse
    {
        //Task<ICollection<T>> FindAllParents<T>(ICollection<T> allEnities, int? id, Expression<Func<T, bool>>? conditionExpression) where T : class, IHierarchical;

        Task<ICollection<TOut>> FindAllParents2<TIn, TOut>(ICollection<TOut> allEnities, int? id,
            Expression<Func<TOut, bool>>? conditionExpression) where TIn : class, IHierarchicalBaseEntity,IBaseEntity
            where TOut : class, IHierarchicalBaseEntity;
    }

    public interface ITest<in TIn, out TOut>
    {
        TOut Nest(TIn @in);
    }

}