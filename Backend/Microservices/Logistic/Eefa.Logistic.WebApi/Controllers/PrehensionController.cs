using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Logistic.Application;
using Eefa.Logistic.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Logistic.WebApi.Controllers
{
    public class PrehensionController : ApiControllerBase
    {
        IPrehensionQueries _PrehensionQueries;


        public PrehensionController(IPrehensionQueries PrehensionQueries
            )
        {
            _PrehensionQueries = PrehensionQueries ?? throw new ArgumentNullException(nameof(PrehensionQueries));
        }

        [HttpGet]
       
        public async Task<IActionResult> Get(int id)
        {

            var result = await _PrehensionQueries.GetById(id);
            return Ok(ServiceResult<Prehension>.Success(result));
        }
        
        [HttpPost]

        public async Task<IActionResult> GetAll(
           
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _PrehensionQueries.GetAll( paginatedQuery);

            return Ok(ServiceResult<PagedList<Prehension>>.Success(result));
        }
        [HttpPost]

        public async Task<IActionResult> GetGroupByCode(

            PaginatedQueryModel paginatedQuery)
        {
            var result = await _PrehensionQueries.GetGroupByCode(paginatedQuery);

            return Ok(ServiceResult<PagedList<string>>.Success(result));
        }
        


    }
}