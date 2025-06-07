using FluentValidation;
using Library.Exceptions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Common.Behaviors
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
           where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var failures = _validators
                    .Select(validator => validator.ValidateAsync(context, cancellationToken))
                    .SelectMany(validationResult => validationResult.GetAwaiter().GetResult().Errors)
                    .Where(validationFailure => validationFailure != null)
                    .ToList();

                if (failures.Count != 0)
                    throw new ApplicationValidationException(failures.Select(x => new ApplicationErrorModel { Source = x.PropertyName, PropertyName = x.PropertyName, Message = x.ErrorMessage}).ToList());
            }
            return await next();
        }
    }
}
