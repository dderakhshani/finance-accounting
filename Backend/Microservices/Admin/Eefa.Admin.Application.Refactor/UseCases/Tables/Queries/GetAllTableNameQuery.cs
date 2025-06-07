using MediatR;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetAllTableNameQuery : IRequest<ServiceResult<object>>
{
    public string TableName { get; set; }
}
public class GetAllTableNameQueryHandler : IRequestHandler<GetAllTableNameQuery, ServiceResult<object>>
{
    public readonly IUnitOfWork _unitOfWork;

    public GetAllTableNameQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork; ;
    }

    public async Task<ServiceResult<object>> Handle(GetAllTableNameQuery request, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>();
        if (string.IsNullOrEmpty(request.TableName))
        {
            var response = await _unitOfWork.ExecuteSqlCommandAsync<TableModel>($"EXEC [dbo].[SPGetColumnsDescription]  {QueryUtility.SqlParametersToQuey(parameters)}",
                    parameters.ToArray(),
                    cancellationToken);
            var result = response.Select(x => x.schema + "." + x.Table).Distinct().ToList();

            return ServiceResult.Success<object>(result);
        }
        else
        {
            var response = await _unitOfWork.ExecuteSqlCommandAsync<TableModel>($"EXEC [dbo].[SPGetColumnsDescription]  {QueryUtility.SqlParametersToQuey(parameters)}",
                    parameters.ToArray(),
                    cancellationToken);
            var result = response.Where(a => a.Table == request.TableName.Split('.')[1]).Select(x => new { x.Type, x.Description, x.Column }).Distinct().ToList();

            return ServiceResult.Success<object>(result);
        }
    }
}