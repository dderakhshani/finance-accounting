using Eefa.Bursary.Infrastructure.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Behaviors
{
    public class RequestResponseLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IBursaryUnitOfWork _unitOfWork;
        private readonly IApplicationLogs _logManager;

        public RequestResponseLoggingBehavior(IBursaryUnitOfWork unitOfWork, IApplicationLogs logManager)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception error)
            {
                 await _logManager.CommitLog(request, error,500);
                throw;
            }

        }

    }
}
