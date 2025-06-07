using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

public class Specification<T> where T : AuditableEntity
{
    [SwaggerExclude]
    public List<Expression<Func<T, bool>>> ApplicationConditions { get; set; } = new List<Expression<Func<T, bool>>>();
    [SwaggerExclude]
    public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; }
    [SwaggerExclude]
    public List<string> IncludeStrings { get; } = new List<string>();
    [SwaggerExclude]
    public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }


    public List<DynamicFilter> Conditions { get; set; } = new List<DynamicFilter>();
    public string OrderByProperty { get; set; } = "id ASC";
    public int PageIndex { get; set; }
    public int PageSize { get; set; }


    public Specification<T> Where(Expression<Func<T, bool>> condition)
    {
        this.ApplicationConditions.Add(condition);
        return this;
    }
}
