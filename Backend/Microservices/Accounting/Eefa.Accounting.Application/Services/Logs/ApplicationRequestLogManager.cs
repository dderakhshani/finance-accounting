using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Eefa.Accounting.Data.Logs;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Exceptions;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Services.Logs
{
    public class ApplicationRequestLogManager : IApplicationRequestLogManager
    {
        private readonly IAccountingUnitOfWork unitOfWork;
        private readonly ICurrentUserAccessor currentUser;
        private readonly IConfigurationProvider configurationProvider;

        public ApplicationRequestLogManager(IAccountingUnitOfWork unitOfWork, ICurrentUserAccessor currentUser, IConfigurationProvider configurationProvider)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
            this.configurationProvider = configurationProvider;
        }

        public async Task CommitLog(object request, object? error = null, int status = 0, CancellationToken cancellationToken = default)
        {
            this.unitOfWork.DbContext().ChangeTracker.Clear();
            var requestLog = new ApplicationRequestLog
            {
                Id = Guid.NewGuid(),
                RequestJSON = JsonConvert.SerializeObject(request),
                RequestType = request.GetType().Name,
                ResponseJSON = JsonConvert.SerializeObject(error),
                Status = status,
                CreatedAt = DateTime.UtcNow,
                ProjectTitle = "Accounting",
                CreatedById = currentUser.GetId(),
            };
            this.unitOfWork.DbContext().Add(requestLog);
            try
            {

            await unitOfWork.DbContext().SaveChangesAsync(cancellationToken);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }



        public async Task<ServiceResult> GetAll(GetLogsQuery query)
        {
            var queryable = this.unitOfWork.ApplicationRequestLogs
                .ProjectTo<ApplicationRequestLogViewModel>(configurationProvider)
                .WhereQueryMaker(query.Conditions)
                .OrderByMultipleColumns(query.OrderByProperty);

            return ServiceResult.Success(new
            {
                Data = await queryable.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = await queryable.CountAsync()
            });
        }

    }
}
