using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Eefa.Inventory.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class AuditController : ApiControllerBase
    {
        IAuditQueries _AuditQueries;
        public AuditController(

           IAuditQueries AuditQueries
          )
        {
            _AuditQueries = AuditQueries ?? throw new ArgumentNullException(nameof(AuditQueries));
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditById(int PrimaryId, string TableName)
        {
            var result = await _AuditQueries.GetAuditById(PrimaryId, TableName);
            return Ok(ServiceResult<PagedList<spGetLogTable>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(string FromDate,string ToDate, PaginatedQueryModel paginatedQuery) {

            var result = await _AuditQueries.GetAll(FromDate, ToDate, paginatedQuery);
            return Ok(ServiceResult<PagedList<SpGetAudit>>.Success(result));

        }

       

    }
}