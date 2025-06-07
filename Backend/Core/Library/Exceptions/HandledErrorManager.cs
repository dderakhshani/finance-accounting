using System;
using System.Collections.Generic;
using Library.Common;
using Library.Exceptions.Interfaces;
using Library.Models;
using Library.Resources;

namespace Library.Exceptions
{
    public class HandledErrorManager : IHandledErrorManager
    {
        private readonly IResourceFactory _validationFactory;

        public HandledErrorManager(IResourceFactory validationFactory)
        {
            _validationFactory = validationFactory;
        }

        public T Throw<T>(List<string> value = null) where T : System.Exception, IHandledException
        {
            var message = CommandBase.ValidationMessageBuilder(_validationFactory, new Message()
            {
                Key = typeof(T).Name,
                Values = value
            });

            var someResult = (T)Activator.CreateInstance(
                typeof(T)
                , new object[] { message }
            )!;
            throw someResult ?? throw new Exception(message);
        }
    }
}