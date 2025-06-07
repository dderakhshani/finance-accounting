using System.Collections.Generic;

namespace Library.Models
{
    public class Validator
    {
        public List<Properties> Properties { get; set; }
    }

    public class Properties
    {
        public string Title { get; set; }
        public List<Expression> Expressions { get; set; }
        public List<Function> Functions { get; set; }
        public List<NestedValidation> NestedValidations { get; set; }
    }

    public class Expression
    {
        public string Query { get; set; }
        public Message Message { get; set; }
    }

    public class Function
    {
        public string Name { get; set; }
        public string ApiUrl { get; set; }
        public string[] Params { get; set; }
        public Message Message { get; set; }
    }

    public class NestedValidation
    {
        public string ValidatorName { get; set; }
    }
    public class Message
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }
        public string MessageValue { get; set; }
        public string TranslatedPropertyName { get; set; }
    }
}

