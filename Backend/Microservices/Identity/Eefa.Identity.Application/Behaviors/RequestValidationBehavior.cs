using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data.SqlServer;
using Infrastructure.Interfaces;
using MediatR;

namespace Eefa.Identity.Application.Behaviors
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse> ,ICommand
    {
        public RequestValidationBehavior()
        {
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        { 
           // await request.Validate(request, _repository);
            return await next();
        }
    }
}