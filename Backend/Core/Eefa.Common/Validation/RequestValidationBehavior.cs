using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using Eefa.Common.Validation.Resources;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Common.Validation
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
          where TRequest : IRequest<TResponse>, ICommand
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IValidationFactory _validationFactory;
        public RequestValidationBehavior(ICurrentUserAccessor currentUserAccessor, IWebHostEnvironment hostingEnvironment, IValidationFactory validationFactory)
        {
            _currentUserAccessor = currentUserAccessor;
            _hostingEnvironment = hostingEnvironment;
            _validationFactory = validationFactory;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var res = await Utility.Validate<TRequest>(request, _currentUserAccessor, _hostingEnvironment, _validationFactory);
            if (res!=null && res.Count > 0)
            {
                throw new ValidationException(res);
            }
            return await next();
        }
    }
}
