using System.Collections.Generic;

namespace Eefa.Common.Validation
{
    public static class ValidationFunctionsLocator
    {
      
        public static Dictionary<string, ValidatorFunctionDelegate> ValidatorFunctions { get; private set; }

        public static void Init(Dictionary<string, ValidatorFunctionDelegate> keyValuePairs)
        {
            ValidatorFunctions = keyValuePairs;
        }
    }
}