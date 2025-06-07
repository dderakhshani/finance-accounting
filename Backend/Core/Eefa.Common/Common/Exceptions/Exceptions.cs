using System;
using System.Collections.Generic;

namespace Eefa.Common.Exceptions
{
    public interface IValidationException
    {
    }

    public interface IValidationErrorManager
    {
        public void Throw<T>(List<string> value = null) where T : Exception, IValidationException;
    }

    public interface IUnAuthorizedException : IValidationException
    {
    }

    public abstract class ValidationExceptionBase : System.Exception, IValidationException
    {
        public ValidationExceptionBase(string message)
             : base(message)
        {
        }

        public ValidationExceptionBase()
            : base()
        {
        }
    }

    public class AuthorizationException : ValidationExceptionBase, IUnAuthorizedException
    {
        public AuthorizationException(string message)
            : base(message)
        {
        }
    }

    public class InvalidToken : ValidationExceptionBase, IUnAuthorizedException
    {
        public InvalidToken()
            : base()
        {
        }
    }

    public class AlreadyLoggedInException : ValidationExceptionBase
    {
        public AlreadyLoggedInException(string message)
            : base(message)
        {
        }
    }

    public class BadRequestException : ValidationExceptionBase
    {
        public BadRequestException(string message)
            : base(message)
        {
        }

    }

    public class InvalidConfirmPassword : ValidationExceptionBase
    {
        public InvalidConfirmPassword(string message)
            : base(message)
        {
        }
    }

    public class InvalidFromat : ValidationExceptionBase
    {
        public InvalidFromat(string message)
            : base(message)
        {
        }
    }

    public class NotFountException : ValidationExceptionBase
    {
        public NotFountException(string message)
            : base(message)
        {
        }
    }
    public class ValidationError : ValidationExceptionBase
    {
        public ValidationError(string message= "There is no sent request number")
            : base(message)
        {
        }
    }
    public class UniqueKeyViolation :  ValidationExceptionBase
    {
        public UniqueKeyViolation(string? message = "UniqueKeyViolation")
            : base(message)
        {
        }
    }

    public class TEntityIsEmpity : ValidationExceptionBase
    {
        public TEntityIsEmpity(string message)
            : base(message)
        {
        }
    }

    public class RefreshTokenHasBeenChanged : AuthorizationException
    {
        public RefreshTokenHasBeenChanged(string message)
            : base(message)
        {
        }
    }

    public class RefreshTokenHasBeenExpired : AuthorizationException
    {
        public RefreshTokenHasBeenExpired(string message)
            : base(message)
        {
        }

    }

    public class RefreshTokenHasNotExpiredYet : ValidationExceptionBase
    {
        public RefreshTokenHasNotExpiredYet(string message)
            : base(message)
        {
        }

    }


}
