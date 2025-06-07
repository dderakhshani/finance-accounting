using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

internal static class SpecificationEvaluator
{
    public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, Specification<T> specification, bool ignorePagination = false)
        where T : AuditableEntity
    {
        if (inputQuery == null) throw new ArgumentNullException(nameof(inputQuery));

        if (specification == null) throw new ArgumentNullException(nameof(specification));

        IQueryable<T> query = inputQuery;

        // Applying application conditions
        if (specification.ApplicationConditions != null && specification.ApplicationConditions.Any())
        {
            foreach (Expression<Func<T, bool>> specificationCondition in specification.ApplicationConditions)
            {
                query = query.Where(specificationCondition);
            }
        }

        // Includes all expression-based includes
        if (specification.Includes != null)
        {
            query = specification.Includes(query);
        }

        // Include any string-based include statements
        if (specification.IncludeStrings != null && specification.IncludeStrings.Any())
        {
            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
        }


        // Apply ordering if expressions are set
        if (specification.OrderBy != null)
        {
            query = specification.OrderBy(query);
        }

        // Apply paging if enabled
        if (specification.PageIndex != 0 && specification.PageSize != 0 && !ignorePagination)
        {
            query = query.Skip((specification.PageIndex - 1) * specification.PageSize).Take(specification.PageSize);
        }


        return query;
    }
    public static IQueryable<TProjectedType> GetSpecifiedQuery<T, TProjectedType>(this IQueryable<T> inputQuery, Specification<T> specification, AutoMapper.IConfigurationProvider mapperConfigurations = null, Expression<Func<T, TProjectedType>> selectExpression = null, bool ignorePagination = false)
    where T : AuditableEntity
    {
        if (inputQuery == null) throw new ArgumentNullException(nameof(inputQuery));

        if (specification == null) throw new ArgumentNullException(nameof(specification));

        IQueryable<T> query = inputQuery;

        // Applying application conditions
        if (specification.ApplicationConditions != null && specification.ApplicationConditions.Any())
        {
            foreach (Expression<Func<T, bool>> specificationCondition in specification.ApplicationConditions)
            {
                query = query.Where(specificationCondition);
            }
        }

        // Includes all expression-based includes
        if (specification.Includes != null)
        {
            query = specification.Includes(query);
        }

        // Include any string-based include statements
        if (specification.IncludeStrings != null && specification.IncludeStrings.Any())
        {
            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
        }
        // Apply ordering if expressions are set
        if (specification.OrderBy != null)
        {
            query = specification.OrderBy(query);
        }
        IQueryable<TProjectedType> projectedQuery = null;
        if (selectExpression != null)
        {
            projectedQuery = query.Select(selectExpression);
        }
        else
        {
            // Apply Projection
            projectedQuery = query.ProjectTo<TProjectedType>(mapperConfigurations);
        }

        // Apply dynamic filters
        if (specification.Conditions != null && specification.Conditions.Any())
        {
            projectedQuery = projectedQuery.ApplyDynamicFilters(specification.Conditions);
        }

        // Apply dynamic orderBy
        else if (!(string.IsNullOrWhiteSpace(specification.OrderByProperty) || string.IsNullOrWhiteSpace(specification.OrderByProperty)))
        {
            projectedQuery = projectedQuery.OrderBy(specification.OrderByProperty);
        }

        // Apply paging if enabled
        if (specification.PageIndex != 0 && specification.PageSize != 0 && !ignorePagination)
        {
            projectedQuery = projectedQuery.Skip((specification.PageIndex - 1) * specification.PageSize).Take(specification.PageSize);
        }


        return projectedQuery;
    }

    private static IQueryable<T> ApplyDynamicFilters<T>(this IQueryable<T> query, List<DynamicFilter> conditions)
    {
        string @where = "x => ";

        foreach (var condition in conditions)
        {
            Type type = typeof(T);
            var propertyType = type.GetProperty(condition.PropertyName.Substring(0, 1).ToUpper() + condition.PropertyName.Substring(1, condition.PropertyName.Length - 1))?.PropertyType;
            if (propertyType != null && propertyType == typeof(DateTime))
            {
                condition.Values = condition.Values.Select(x => ((DateTime)x).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ")).ToArray();
            }
            condition.PropertyName = string.Concat(condition.PropertyName[0].ToString().ToUpper(),
                condition.PropertyName.AsSpan(1));

            if (condition.Comparison == "between")
            {
                @where +=
                    $"(x.{condition.PropertyName} >= \"{condition.Values[0]}\" && x.{condition.PropertyName} <= \"{condition.Values[1]}\") {condition.NextOperand} ";
            }
            else if (condition.Comparison == "startsWith")
            {
                @where += $"x.{condition.PropertyName}.StartsWith(\"{condition.Values[0]}\")  {condition.NextOperand} ";
            }
            else if (condition.Comparison == "endsWith")
            {
                @where += $"x.{condition.PropertyName}.EndsWith(\"{condition.Values[0]}\"')  {condition.NextOperand} ";
            }
            else if (condition.Comparison == "in")
            {
                for (var i = 0; i < condition.Values.Length; i += 1)
                {
                    try
                    {
                        if (i == 0)
                        {
                            @where += "(";
                        }

                        @where += $"x.{condition.PropertyName} == {condition.Values[i]}" + (i + 1 < condition.Values.Length ? " or " : ")");

                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }
                @where += $" {condition.NextOperand} ";

            }
            else if (condition.Comparison == "inList")
            {
                @where += "(";
                for (var i = 0; i < condition.Values.Length; i += 1)
                {
                    try
                    {
                        @where += $"x.{condition.PropertyName}.Contains({condition.Values[i]}) and ";
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }

                if (@where.EndsWith("and "))
                {
                    @where = @where.Remove(@where.Length - 5, 5);
                }
                @where += ")";
                @where += $" {condition.NextOperand} ";
            }
            else if (condition.Comparison == "ofList")
            {
                @where += "(";
                for (var i = 0; i < condition.Values.Length; i += 1)
                {
                    try
                    {
                        @where += $"x.{condition.PropertyName} = {condition.Values[i]} or ";
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }

                if (@where.EndsWith(" or "))
                {
                    @where = @where.Remove(@where.Length - 4, 4);
                }
                @where += ")";
                @where += $" {condition.NextOperand} ";

            }
            else if (condition.Comparison == ".Contains")
            {
                @where +=
                   $"x.{condition.PropertyName}{condition.Comparison}(\"{condition.Values[0]}\") {condition.NextOperand} ";
            }

            else if (condition.Comparison == "notContains")
            {
                @where += $"!x.{condition.PropertyName}.Contains(\"{condition.Values[0]}\") {condition.NextOperand} ";
            }

            else
            {
                var isConditionValueBoolean = (condition.Values[0].ToString().ToLower().Equals("true") || condition.Values[0].ToString().ToLower().Equals("false"));
                object conditionValue;
                if (!isConditionValueBoolean)
                {
                    conditionValue = condition.Values[0] != null && condition.Values[0].ToString() != "null" ? $"\"{condition.Values[0]}\"" : $"{condition.Values[0]}";
                }
                else
                {
                    conditionValue = condition.Values[0];
                }

                if (propertyType != null)
                {
                    if (propertyType == typeof(DateTime) && condition.Comparison == "==")
                    {
                        @where +=
                          $"x.{condition.PropertyName} >= {conditionValue}  &&  {condition.PropertyName} < \"{DateTime.Parse(condition.Values[0].ToString()).AddDays(1)}\" {condition.NextOperand} ";
                    }
                    else
                    {
                        @where +=
                            $"x.{condition.PropertyName} {condition.Comparison} {conditionValue} {condition.NextOperand} ";
                    }
                }
                else
                {
                    @where +=
                            $"x.{condition.PropertyName} {condition.Comparison} {conditionValue} {condition.NextOperand} ";
                }

            }
        }
        @where = @where.Trim();

        if (@where.EndsWith("or") || @where.EndsWith("and"))
        {
            var charsToRemove = @where.EndsWith("or") ? 3 : 4;
            @where = @where.Remove(@where.Length - charsToRemove, charsToRemove);
        }

        query = query.Where(@where);

        return query;
    }
}
