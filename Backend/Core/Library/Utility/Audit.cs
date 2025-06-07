//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Linq.Expressions;
//using System.Threading.Tasks;
//using Infrastructure.Common;
//using Infrastructure.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using ServiceStack;

//namespace Infrastructure.Utility
//{
//    public class Audit
//    {
//        public static DbContext DbContext { get; set; }

//        public class AuditMapper<TSource, TDestination> where TSource : class, IBaseEntity
//            where TDestination : class, IBaseEntity
//        {
//            public class Change
//            {
//                public Change(Expression<Func<TSource, string>> source,
//                    Expression<Func<TDestination, string>> destination,
//                    Expression<Func<TDestination, string>> comparerProperty)
//                {
//                    Source = source;
//                    Destination = destination;
//                    ComparerProperty = comparerProperty;
//                }

//                public Expression<Func<TSource, string>> Source { get; set; }
//                public Expression<Func<TDestination, string>> Destination { get; set; }
//                public Expression<Func<TDestination, string>> ComparerProperty { get; set; }
//            }


//            public async Task<List<BaseAudit>> Map(EntityEntry entityEntry,
//                List<Change> options)
//            {
//                var audits = new List<BaseAudit>();

//                foreach (var expression in options)
//                {
//                    var destinationProperty = expression.Source.Body.ToString().Replace('\"', ' ').Trim();
//                    foreach (var property in entityEntry.Properties)
//                    {
//                        if (property.Metadata.Name == destinationProperty)
//                        {
//                            var audit = new BaseAudit(destinationProperty);
//                            var whereParameter = expression.ComparerProperty.ToString().Replace('\"', ' ').Trim();
//                            var oldValue = int.Parse(property.OriginalValue.ToString() ?? string.Empty);
//                            var newValue = int.Parse(property.CurrentValue.ToString() ?? string.Empty);
//                            var oldWhereCondition = $"x=> x.{whereParameter} == {oldValue}";
//                            var newWhereCondition = $"x=> x.{whereParameter} == {newValue}";

//                            audit.Old = await DbContext.Set<TDestination>().Where(oldWhereCondition)
//                                .Select(expression.Destination).FirstAsync();
//                            audit.New = await DbContext.Set<TDestination>().Where(newWhereCondition)
//                                .Select(expression.Destination).FirstAsync();

//                            audits.Add(audit);
//                        }
//                    }
//                }

//                return audits;
//            }
//        }
//    }
//}