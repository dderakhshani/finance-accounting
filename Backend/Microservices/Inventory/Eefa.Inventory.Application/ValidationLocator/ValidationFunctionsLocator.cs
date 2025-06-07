using System.Collections.Generic;
using Eefa.Common.Validation;

namespace Eefa.Inventory.Application.ValidationLocator
{
    public static class ValidationFunctionsLocator
    {
        public static Dictionary<string, ValidatorFunctionDelegate> ValidatorFunctions { get; private set; }

        public static void Init()
        {
            Eefa.Common.Validation.ValidationFunctionsLocator.Init(ValidatorFunctions);
        }
        static ValidationFunctionsLocator()
        {
            ValidatorFunctions = new Dictionary<string, ValidatorFunctionDelegate>
            {
                 //["IsBaseValueIdInUse"] = new(BaseValueValidation.IsBaseValueIdInUse),
            };
        }
    }
}