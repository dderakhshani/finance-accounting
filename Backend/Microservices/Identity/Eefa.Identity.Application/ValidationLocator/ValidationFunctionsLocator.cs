using System.Collections.Generic;

namespace Eefa.Identity.Application.ValidationLocator
{
    public static class ValidationFunctionsLocator
    {
        public static Dictionary<string, Infrastructure.Common.ValidationFunctionsLocator.ValidatorFunctionDelegate> ValidatorFunctions { get; private set; }

        public static void Set()
        {
            Infrastructure.Common.ValidationFunctionsLocator.Set(ValidatorFunctions);
        }
        static ValidationFunctionsLocator()
        {
            ValidatorFunctions = new Dictionary<string, Infrastructure.Common.ValidationFunctionsLocator.ValidatorFunctionDelegate>
            {
             //   ["DoesBaseValueExist"] = new(BaseValueValidation.Iswfwqegqwgee),
            };
        }
    }
}