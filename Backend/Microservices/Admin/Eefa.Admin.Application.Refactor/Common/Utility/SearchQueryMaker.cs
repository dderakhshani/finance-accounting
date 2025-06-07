using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Linq.Dynamic.Core;

public static class SearchQueryMaker
{
    //public static IQueryable<TEntity> WhereQueryMaker<TEntity>(this IQueryable<TEntity> query,List<SearchQueryItem>? queries)
    //{
    //    if (queries is null)
    //    {
    //        return query;
    //    }
    //    foreach (var searchQueryItem in queries)
    //    {
    //        foreach (var part in searchQueryItem.Parts)
    //        {
    //            foreach (var DynamicFilter in part.Value)
    //            {
    //                if (DynamicFilter.Comparison == "between")
    //                {
    //                    query = query.Where($"{DynamicFilter.PropertyName} >= @0 && {DynamicFilter.PropertyName} <= @1",
    //                        DynamicFilter.Values);
    //                }
    //                else
    //                {
    //                    query = query.Where($"{DynamicFilter.PropertyName} {DynamicFilter.Comparison}(@0)",
    //                        DynamicFilter.Values);
    //                }
    //            }
    //        }
    //    }

    //    return query;
    //}


    public static IQueryable<TEntity> WhereQueryMaker<TEntity>(this IQueryable<TEntity> query, List<DynamicFilter>? conditions)
    {
        if (conditions is null || conditions.Count == 0) return query;

        string queryString = "x => ";
        var uniquePropertyNamesGettingFiltered = conditions.Select(x => x.PropertyName).Distinct().ToList();
        var doesConditionsHaveMultipleQueryOnSameProperty = uniquePropertyNamesGettingFiltered.Any(x => conditions.Where(y => y.PropertyName == x).ToList().Count > 1);
        if (doesConditionsHaveMultipleQueryOnSameProperty)
        {
            foreach (var condition in conditions)
            {
                if (conditions.Where(x => x.PropertyName == condition.PropertyName).ToList().Count == 1) condition.NextOperand = "and";
            }
        }
        foreach (var propertyName in uniquePropertyNamesGettingFiltered)
        {
            var propertyConditions = conditions.Where(x => x.PropertyName == propertyName).ToList();
            if (propertyConditions.Count > 1)
            {
                if (queryString.Trim().EndsWith("or"))
                {
                    queryString = queryString.Trim();
                    queryString = queryString.Remove(queryString.Length - 2, 2);
                    queryString += "and";
                }
                queryString += " (";
            }
            foreach (var condition in propertyConditions)
            {
                ChangeConditionValuesToUTCIfPropertyIsOFTypeDateTime(condition, typeof(TEntity));
                SetPropertyNameToCamelCase(condition);

                if (condition.Comparison == "between") queryString += ApplyBetweenCondition(condition);
                else if (condition.Comparison == "startsWith") queryString += ApplyStartsWithCondition(condition);
                else if (condition.Comparison == "endsWith") queryString += ApplyEndsWithCondition(condition);
                else if (condition.Comparison == "in") queryString += ApplyInCondition(condition);
                else if (condition.Comparison == "inList") queryString += ApplyInListCondition(condition);
                else if (condition.Comparison == "ofList") queryString += ApplyOfListCondition(condition);
                else if (condition.Comparison == ".Contains") queryString += ApplyContainsCondition(condition);
                else if (condition.Comparison == "notContains") queryString += ApplyNotContainsCondition(condition);
                else
                {
                    ConvertConditionValueToStringIfNotBoolean(condition);
                    queryString += ApplyEqualsAndNotEqualsCondition(condition, typeof(TEntity));
                }
            }
            if (propertyConditions.Count > 1 && (queryString.Trim().EndsWith("or") || queryString.Trim().EndsWith("and")))
            {
                queryString = queryString.Trim();
                var charsToRemove = queryString.EndsWith("or") ? 3 : 4;
                queryString = queryString.Remove(queryString.Length - charsToRemove, charsToRemove);
                queryString += ") and ";
            }
        }

        if (queryString.Trim().EndsWith("or") || queryString.Trim().EndsWith("and"))
        {
            queryString = queryString.Trim();
            var charsToRemove = queryString.EndsWith("or") ? 3 : 4;
            queryString = queryString.Remove(queryString.Length - charsToRemove, charsToRemove);
        }

        query = query.Where(queryString);

        return query;
    }



    private static void ChangeConditionValuesToUTCIfPropertyIsOFTypeDateTime(DynamicFilter condition, Type entityType)
    {
        var propertyType = entityType.GetProperty(condition.PropertyName.Substring(0, 1).ToUpper() + condition.PropertyName.Substring(1, condition.PropertyName.Length - 1))?.PropertyType;
        if (propertyType != null && propertyType == typeof(DateTime))
        {
            condition.Values = condition.Values.Select(x => ((DateTime)x).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ")).ToArray();
        }
    }
    private static void SetPropertyNameToCamelCase(DynamicFilter condition)
    {
        condition.PropertyName = string.Concat(condition.PropertyName[0].ToString().ToUpper(),
                   condition.PropertyName.AsSpan(1));
    }
    private static string ApplyBetweenCondition(DynamicFilter condition)
    {
        return $"(x.{condition.PropertyName} >= \"{condition.Values[0]}\" && x.{condition.PropertyName} <= \"{condition.Values[1]}\") {condition.NextOperand} ";
    }
    private static string ApplyStartsWithCondition(DynamicFilter condition)
    {
        return $"x.{condition.PropertyName}.StartsWith(\"{condition.Values[0]}\")  {condition.NextOperand} ";
    }
    private static string ApplyEndsWithCondition(DynamicFilter condition)
    {
        return $"x.{condition.PropertyName}.EndsWith(\"{condition.Values[0]}\"')  {condition.NextOperand} ";
    }
    private static string ApplyInCondition(DynamicFilter condition)
    {
        var queryString = "";
        for (var i = 0; i < condition.Values.Length; i += 1)
        {
            if (i == 0)
            {
                queryString += "(";
            }

            queryString += $"x.{condition.PropertyName} == {condition.Values[i]}" + (i + 1 < condition.Values.Length ? " or " : ")");

        }
        queryString += $" {condition.NextOperand} ";
        return queryString;
    }
    private static string ApplyInListCondition(DynamicFilter condition)
    {
        var queryString = "";
        queryString += "(";
        for (var i = 0; i < condition.Values.Length; i += 1)
        {
            try
            {
                queryString += $"x.{condition.PropertyName}.Contains({condition.Values[i]}) and ";
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        if (queryString.EndsWith("and "))
        {
            queryString = queryString.Remove(queryString.Length - 5, 5);
        }
        queryString += ")";
        queryString += $" {condition.NextOperand} ";

        return queryString;
    }
    private static string ApplyOfListCondition(DynamicFilter condition)
    {
        var queryString = "";
        queryString += "(";
        for (var i = 0; i < condition.Values.Length; i += 1)
        {
            try
            {
                queryString += $"x.{condition.PropertyName} = {condition.Values[i]} or ";
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        if (queryString.EndsWith(" or "))
        {
            queryString = queryString.Remove(queryString.Length - 4, 4);
        }
        queryString += ")";
        queryString += $" {condition.NextOperand} ";
        return queryString;
    }
    private static string ApplyContainsCondition(DynamicFilter condition)
    {
        return $"x.{condition.PropertyName}{condition.Comparison}(\"{condition.Values[0]}\") {condition.NextOperand} ";
    }
    private static string ApplyNotContainsCondition(DynamicFilter condition)
    {
        return $"!x.{condition.PropertyName}.Contains(\"{condition.Values[0]}\") {condition.NextOperand} ";
    }
    private static void ConvertConditionValueToStringIfNotBoolean(DynamicFilter condition)
    {
        var isConditionValueBoolean = (condition.Values[0].ToString().ToLower().Equals("true") || condition.Values[0].ToString().ToLower().Equals("false"));
        if (!isConditionValueBoolean) condition.Values[0] = condition.Values[0] != null && condition.Values[0].ToString() != "null" ? $"\"{condition.Values[0]}\"" : $"{condition.Values[0]}";
    }
    private static string ApplyEqualsAndNotEqualsCondition(DynamicFilter condition, Type entityType)
    {
        var queryString = "";
        var propertyType = entityType.GetProperty(condition.PropertyName.Substring(0, 1).ToUpper() + condition.PropertyName.Substring(1, condition.PropertyName.Length - 1))?.PropertyType;

        if (propertyType != null && propertyType == typeof(DateTime) && condition.Comparison == "==")
        {
            queryString += $"x.{condition.PropertyName} >= {condition.Values[0]}  &&  {condition.PropertyName} < \"{DateTime.Parse(condition.Values[0].ToString()).AddDays(1)}\" {condition.NextOperand} ";
        }
        else
        {
            queryString += $"x.{condition.PropertyName} {condition.Comparison} {condition.Values[0]} {condition.NextOperand} ";
        }

        return queryString;
    }


    public static Expression<Func<TEntity, bool>> MakeSearchQuery<TEntity>(List<DynamicFilter> queries)
    {
        Expression<Func<TEntity, bool>> exp = null;
        //foreach (var query in queries)
        //{
        //    Expression<Func<TEntity, bool>> partExp = null;
        //    foreach (var queryPart in query.Parts)
        //    {
        //        foreach (var DynamicFilter in queryPart.Value)
        //        {
        //            if (partExp == null)
        //            {
        //                partExp = ExpressionUtils.BuildPredicate<TEntity>(DynamicFilter.PropertyName,
        //                    DynamicFilter.Comparison, DynamicFilter.Values);
        //            }
        //            else
        //            {

        //                partExp = ExpressionUtils.BuildPredicate<TEntity>(partExp,
        //                    DynamicFilter.PropertyName, DynamicFilter.Comparison, DynamicFilter.Values, DynamicFilter.Operand);
        //            }
        //        }

        //        partExp = ExpressionUtils.BuildPredicate<TEntity>(partExp, queryPart.Key);
        //    }

        //    if (exp == null)
        //    {
        //        exp = partExp;
        //    }
        //    else
        //    {
        //        exp.BuildPredicate<TEntity>(partExp, query.NextOperand);
        //    }
        //}

        return exp;
    }
}