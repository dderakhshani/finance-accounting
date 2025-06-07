namespace Library.Models
{
    public class Condition
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
                        _comparison = "=";
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
                    case "between":
                        _comparison = "between";
                        break;
                    case "greaterOrEqualThan":
                    case ">=":
                        _comparison = ">=";
                        break;
                    case "lessOrEqualThan":
                    case "<=":
                        _comparison = "<=";
                        break;
                    case "greaterThan":
                    case ">":
                        _comparison = ">";
                        break;
                    case "lessThan":
                    case "<":
                        _comparison = "<";
                        break;
                    case "startsWith":
                        _comparison = "startsWith";
                        break;
                    case "endsWith":
                        _comparison = "endsWith";
                        break;                  
                    case "inList":
                        _comparison = "inList";
                        break;                 
                    case "ofList":
                        _comparison = "ofList";
                        break;
                    default:
                        _comparison = "=";
                        break;
                }
            }
        }

        public string NextOperand
        {
            get
            {
                if(string.IsNullOrEmpty(_nextOperand))
                {
                    return "or";
                }else
                {
                    return _nextOperand;
                }
            }
            set => _nextOperand = value;
        }

        private string _comparison { get; set; }
        private string _nextOperand { get; set; }


        public object[] Values { get; set; }

    }
}