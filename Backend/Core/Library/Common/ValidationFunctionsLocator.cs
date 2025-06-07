using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Library.Interfaces;

namespace Library.Common
{
    public static class ValidationFunctionsLocator
    {
        public delegate Task<bool> ValidatorFunctionDelegate(ICommand command, IRepository repository,IMapper mapper);
        public static Dictionary<string, ValidatorFunctionDelegate> ValidatorFunctions { get; private set; }

        public static void Set(Dictionary<string, ValidatorFunctionDelegate> keyValuePairs)
        {
            ValidatorFunctions = keyValuePairs;
        }
    }
}