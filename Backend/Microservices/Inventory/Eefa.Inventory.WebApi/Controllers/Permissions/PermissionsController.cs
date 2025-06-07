using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class PermissionsController : ApiControllerBase
    {
        IPermissionsQueries _Queries;
       

        public PermissionsController(IPermissionsQueries baseValueQueries)
        {
            _Queries = baseValueQueries ?? throw new ArgumentNullException(nameof(baseValueQueries));

        }
        
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {

            var result = await _Queries.GetById(id);
            return Ok(ServiceResult<PermissionsModel>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GeALL(PaginatedQueryModel paginatedQuery)
        {
            var result = await _Queries.GetAll(paginatedQuery);
            return Ok(ServiceResult<PagedList<PermissionsModel>>.Success(result));

        }
    }
}