using System;
using System.Collections.Generic;
using Library.Interfaces;

namespace Library.Exceptions.Interfaces
{
    public interface IHandledErrorManager : IService
    {
        public T Throw<T>(List<string> value = null) where T : Exception, IHandledException;
    }
}