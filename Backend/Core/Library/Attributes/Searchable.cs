using System;
using System.Linq;

namespace Library.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class Searchable : Attribute
    {
        public string TableName { get; set; }
        public string Schema { get; set; }
        public SearchTypes[] SearchType;
        public enum SearchTypes
        {
            Equal,
            Between,
            NotEqual,
            GreaterThan,
            GreaterThanOrEqual,
            LessThan,
            LessThanOrEqual,
            Contains,
            StartsWith,
            EndsWith
        }
        public Searchable(string tabelName,string schema,SearchTypes[] searchType)
        {
            TableName = tabelName;
            Schema = schema;
            SearchType = searchType;
        }

        public override string ToString()
        {
            var res = SearchType.Aggregate(string.Empty, (current, v) => current + (v + ","));

            if (res.EndsWith(','))
            {
                res = res.Remove(res.Length - 1, 1);
            }
            return res;
        }
    }
}