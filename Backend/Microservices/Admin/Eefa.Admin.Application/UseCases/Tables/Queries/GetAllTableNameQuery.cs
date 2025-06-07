using Eefa.Admin.Application.UseCases.Tables.Model;
using Eefa.Admin.Data.Databases.SqlServer.Context;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Eefa.Admin.Application.UseCases.Tables.Query
{
    public class GetAllTableNameQuery : IRequest<ServiceResult>
    {
        public string TableName { get; set; }
    }
    public class GetAllTableNameQueryHandler : IRequestHandler<GetAllTableNameQuery, ServiceResult>
    {
        public readonly IUnitOfWork _unitOfWork;

        public GetAllTableNameQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(GetAllTableNameQuery request, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(request.TableName))
            {
                var response = await _unitOfWork.ExecuteSqlQueryAsync<TableModel>($"EXEC [dbo].[SPGetColumnsDescription]  {QueryUtility.SqlParametersToQuey(parameters)}",
                        parameters.ToArray(),
                        cancellationToken);
                var result = response.Select(x => x.schema + "." + x.Table).Distinct().ToList();

                return ServiceResult.Success(result);
            }
            else
            {
                var response = await _unitOfWork.ExecuteSqlQueryAsync<TableModel>($"EXEC [dbo].[SPGetColumnsDescription]  {QueryUtility.SqlParametersToQuey(parameters)}",
                        parameters.ToArray(),
                        cancellationToken);
                var result = response.Where(a => a.Table == request.TableName.Split('.')[1]).Select(x => new { x.Type, x.Description, x.Column }).Distinct().ToList();

                return ServiceResult.Success(result);
            }
        }
    }
}
