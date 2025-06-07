using System;
using System.Linq.Expressions;

namespace Eefa.Common.Data
{
    public class AuditMapRule<TSource, TDestination> : AuditMapRule
    {
        public Expression<Func<TSource, object>> Source { get; set; }
        public Expression<Func<TDestination, string>> Destination { get; set; }
        public Expression<Func<TDestination, object>> ComparerProperty { get; set; }

        public AuditMapRule(Expression<Func<TSource, object>> source,
            Expression<Func<TDestination, string>> destination,
            Expression<Func<TDestination, object>> comparerProperty)
        {
            Source = source;
            Destination = destination;
            ComparerProperty = comparerProperty;
        }
    }

    //Unkown functionality
    public interface IChange
    {

    }

    public class AuditMapRule : IChange
    {
    }
}