using Eefa.Accounting.Application.Services.EventManager;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Common.Behaviors
{
    public class ApplicationEventsPersistenceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly IApplicationEventsManager applicationEventsManager;

        public ApplicationEventsPersistenceBehavior(IApplicationEventsManager applicationEventsManager)
        {
            this.applicationEventsManager = applicationEventsManager;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            try
            {
                await applicationEventsManager.CommitEvents(cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

    }
}
