using System.Collections.Generic;

namespace Eefa.Common.Data.Query
{

    public interface SearchableQuery
    {
        List<QueryCondition> Conditions { get; set; }
    }


    public class QueryCondition
    {
        public string PropertyName { get; set; }
        public string Comparison
        {
            get => _comparison ?? "=";
            set
            {
                switch (value)
                {
                    case "equal":
                    case "=":
                        _comparison = "==";
                        break;
                    case "notEqual":
                    case "!=":
                        _comparison = "!=";
                        break;
                    case "contains":
                        _comparison = ".Contains";
                        break;
                    case "notContains":
                        _comparison = "notContains";
                        break;
                    case "in":
                        _comparison = "in";
                        break;
                    case "notIn":
                        _comparison = "notIn";
                        break;
                    case "inList":
                        _comparison = "inList";
                        break;
                    case "ofList":
                        _comparison = "ofList";
                        break;
                    case "between":
                        _comparison = "between";
                        break;
                    case "greaterThan":
                    case ">=":
                        _comparison = ">=";
                        break;
                    case "lessThan":
                    case "<=":
                        _comparison = "<=";
                        break;
                    default:
                        _comparison = "=";
                        break;
                }
            }
        }

        private string _comparison { get; set; }


        public object[] Values { get; set; }


        public string NextOperand
        {
            get
            {
                if (string.IsNullOrEmpty(_nextOperand))
                {
                    return "or";
                }
                else
                {
                    return _nextOperand;
                }
            }
            set => _nextOperand = value;
        }

        private string _nextOperand { get; set; }

    }
}