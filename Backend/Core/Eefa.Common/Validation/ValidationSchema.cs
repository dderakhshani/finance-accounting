using System.Collections.Generic;

namespace Eefa.Common.Validation
{

    public class ValidationCriteria
    {
        public string FieldName { get; set; }
        public List<ValidationCondition> Conditions { get; set; }
        public List<ValidationFunction> Functions { get; set; }
        public List<NestedValidation> NestedValidations { get; set; }
    }

    public class ValidationCondition
    {
        public string Condition { get; set; }
        public ValidationMessage Message { get; set; }
    }

    public class ValidationFunction
    {
        public string Name { get; set; }
        public string ApiUrl { get; set; }
        public string[] Params { get; set; }
        public ValidationMessage Message { get; set; }
    }

    public class NestedValidation
    {
        public string ValidatorName { get; set; }
    }
    public class ValidationMessage
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }
    }
}

