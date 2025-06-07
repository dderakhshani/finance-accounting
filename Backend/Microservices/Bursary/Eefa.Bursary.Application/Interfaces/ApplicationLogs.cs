using AutoMapper;
using Eefa.Bursary.Application;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure
{
    public class ApplicationLogs :IApplicationLogs
    {
        private readonly IBursaryUnitOfWork _unitOfWork;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IConfigurationProvider _configurationProvider;
      

        public ApplicationLogs(IBursaryUnitOfWork unitOfWork, ICurrentUserAccessor currentUser, IConfigurationProvider configurationProvider )
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _configurationProvider = configurationProvider;
        
        }

        public async Task CommitLog(object request, object? error = null, int status = 0, CancellationToken cancellationToken = default)
        {
           _unitOfWork.DbContex().ChangeTracker.Clear();
            var requestLog = new FinancialRequestLogs  
            {
                RequestJSON = JsonConvert.SerializeObject(request),
                RequestType = request.GetType().Name,
                ResponseJSON =error == null ? "200": JsonConvert.SerializeObject(error),
                Status = status,
            
                ProjectTitle = "Bursary",
            };
            _unitOfWork.DbContex().Add(requestLog);
            await _unitOfWork.DbContex().SaveChangesAsync(cancellationToken);
        }
    }
}
