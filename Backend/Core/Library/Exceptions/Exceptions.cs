using Library.Exceptions.Interfaces;

namespace Library.Exceptions
{
    public class AlreadyLoggedIn : System.Exception, IHandledException
    {
        public AlreadyLoggedIn(string message)
            : base(message)
        {
        }
    }

    public class Authorization : System.Exception, IHandledException, I401Exception
    {
        public Authorization(string message)
            : base(message)
        {
        }
    }

    public class InvalidToken : System.Exception, IHandledException, I401Exception
    {
        public InvalidToken()
            : base()
        {
        }
    }

    public class BadRequest : System.Exception, IHandledException
    {
        public BadRequest(string message)
            : base(message)
        {
        }

    }

    public class NotAllowedDate : System.Exception, IHandledException
    {
        public NotAllowedDate(string message)
            : base(message)
        {
        }

    }

    public class UnbalancedVoucherRegisteration : System.Exception, IHandledException
    {
        public UnbalancedVoucherRegisteration(string message)
            : base(message)
        {
        }

    }

    public class InvalidConfirmPassword : System.Exception, IHandledException
    {
        public InvalidConfirmPassword(string message)
            : base(message)
        {
        }
    }

    public class InvalidFromat : System.Exception, IHandledException
    {
        public InvalidFromat(string message)
            : base(message)
        {
        }

    }


    public class EndVoucherIsAlreadyExists : System.Exception, IHandledException
    {
        public EndVoucherIsAlreadyExists(string message)
            : base(message)
        {
        }

    }


    public class NoResult : System.Exception, IHandledException
    {
        public NoResult(string message)
            : base(message)
        {
        }

    }

    public class BalancingVoucherIsAlreadyExists : System.Exception, IHandledException
    {
        public BalancingVoucherIsAlreadyExists(string message)
            : base(message)
        {
        }

    }

    public class CloseVoucherIsAlreadyExists : System.Exception, IHandledException
    {
        public CloseVoucherIsAlreadyExists(string message)
            : base(message)
        {
        }

    }

    public class PassworHasExpired : System.Exception, IHandledException
    {
        public PassworHasExpired(string message)
            : base(message)
        {
        }

    }

    public class UsernameOrPasswordIncorrect : System.Exception, IHandledException
    {
        public UsernameOrPasswordIncorrect(string message)
            : base(message)
        {
        }

    }

    public class UsernameHasBeenBlocked : System.Exception, IHandledException
    {
        public UsernameHasBeenBlocked(string message)
            : base(message)
        {
        }

    }

    public class NotFound : System.Exception, IHandledException
    {
        public NotFound(string message)
            : base(message)
        {
        }
    }

    public class UniqueKeyViolation : System.Exception, IHandledException
    {
        public UniqueKeyViolation(string? message = "UniqueKeyViolation")
            : base(message)
        {
        }
    }

    public class RefreshTokenHasBeenChanged : System.Exception, I401Exception
    {
        public RefreshTokenHasBeenChanged(string message)
            : base(message)
        {
        }
    }

    public class RefreshTokenHasBeenExpired : System.Exception, I401Exception
    {
        public RefreshTokenHasBeenExpired(string message)
            : base(message)
        {
        }

    }

    public class RefreshTokenHasNotExpiredYet : System.Exception, IHandledException
    {
        public RefreshTokenHasNotExpiredYet(string message)
            : base(message)
        {
        }

    }
}