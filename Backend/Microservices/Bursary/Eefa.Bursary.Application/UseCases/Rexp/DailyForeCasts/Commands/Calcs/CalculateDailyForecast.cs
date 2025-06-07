using Dapper;
using Eefa.Bursary.Application.UseCases.Rexp.Models;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Web;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Rexp.DailyForeCasts.Commands.Calcs
{
    public class CalculateDailyForecast : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int? YM { get; set; }
        public List<int>? MonthlyForecastIds { get; set; }
    }

    public class CalculateDailyForecastHandler : IRequestHandler<CalculateDailyForecast, ServiceResult>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IConfigurationAccessor _config;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public CalculateDailyForecastHandler(IBursaryUnitOfWork uow, IConfigurationAccessor config, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _config = config;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult> Handle(CalculateDailyForecast request, CancellationToken cancellationToken)
        {
            var ids = "";
            if (request.MonthlyForecastIds.Count > 0)
                ids = string.Join(",", request.MonthlyForecastIds);

            var parameters = new DynamicParameters();

            parameters.Add("UserId", _currentUserAccessor.GetId());
            parameters.Add("MonthlyForecastIds", ids);
            parameters.Add("YM", request.YM);
            parameters.Add("ResultId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("ResultText", dbType: DbType.String, direction: ParameterDirection.Output, size: 300);
            parameters.Add("RetVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using (var db = new SqlConnection(_config.GetConnectionString().DefaultString))
            {
                await db.ExecuteAsync(sql: "rexp.CalculateMonthForecast", commandType: CommandType.StoredProcedure, param: parameters, commandTimeout: 100);

                var resultId = Convert.ToInt32(parameters?.Get<int>("ResultId").ToString() ?? "0");
                var resultText = parameters?.Get<string>("ResultText")?.ToString();

                if (resultId == 0)
                {
                    return ServiceResult.Success();
                }

                List<string> exps = new List<string>();
                exps.Add(resultText);
                IDictionary<string, List<string>> errs = new Dictionary<string, List<string>>();
                errs.Add(resultText, exps);
                return ServiceResult.Failed(errs);
            }

            return null;
        }
    }
}
