using Eefa.Accounting.Application.Services.EventManager;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Eefa.Accounting.Data.Events.Abstraction;
using Library.Exceptions.Interfaces;
using Library.Exceptions;
using Library.Interfaces;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Accounting.Application.Services.Logs;

namespace Eefa.Accounting.Application.Common.Behaviors
{
    public class ApplicationRequestsLogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly IAccountingUnitOfWork unitOfWork;
        private readonly IApplicationRequestLogManager logManager;

        public ApplicationRequestsLogBehavior(IApplicationRequestLogManager logManager)
        {
            this.logManager = logManager;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception error)
            {
                if (error is not I401Exception) await logManager.CommitLog(request, error, 500);
                throw;
            }

        }

    }
}
