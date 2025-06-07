using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.UseCases.Tables.Query;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class TableController : AdminBaseController
    {
        [HttpGet]
        public async Task<ServiceResult> GetAllTableName(CancellationToken cancellationToken)
        {
            GetAllTableNameQuery model = new();
            return await Mediator.Send(model);
        }
        [HttpGet]
        public async Task<ServiceResult> GetAllTableFieldsByName(string tablename, CancellationToken cancellationToken)
        {
            GetAllTableNameQuery model = new();
            model.TableName = tablename;
            return await Mediator.Send(model);
        }
    }
}
