using Eefa.Common.Validation;
using Eefa.Common.Validation.Resources;
using System;
using System.Collections.Generic;

namespace Eefa.Common.Exceptions
{
    public class ValidationErrorManager : IValidationErrorManager
    {
        private readonly IValidationFactory _validationFactory;

        public ValidationErrorManager(IValidationFactory validationFactory)
        {
            _validationFactory = validationFactory;
        }

        public void Throw<T>(List<string> value = null) where T : System.Exception, IValidationException
        {
            var message = Utility.ValidationMessageBuilder(_validationFactory, new ValidationMessage()
            {
                Key = typeof(T).Name,
                Values = value
            });

            var someResult = (T)Activator.CreateInstance(
                typeof(T)
                , new object[] { message }
            );
            throw someResult ?? throw new Exception(message);
        }
    }
}