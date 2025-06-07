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
    public class UnitCommodityQuotaController : ApiControllerBase
    {
        IUnitCommodityQuotaQueries _Queries;

        public UnitCommodityQuotaController(

           IUnitCommodityQuotaQueries Queries
          )
        {

            _Queries = Queries ?? throw new ArgumentNullException(nameof(Queries));
            
        }
        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _Queries.GetById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery) {

           var result= await _Queries.GetAll(paginatedQuery);
           return Ok(ServiceResult<PagedList<UnitCommodityQuotaModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetAllUnits(PaginatedQueryModel paginatedQuery)
        {

            var result = await _Queries.GetAllUnits(paginatedQuery);
            return Ok(ServiceResult<PagedList<UnitsModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetAllQuotaGroup(PaginatedQueryModel paginatedQuery)
        {

            var result = await _Queries.GetAllQuotaGroup(paginatedQuery);
            return Ok(ServiceResult<PagedList<QuotaGroupModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> add([FromBody] CreateUnitCommodityQuotaCommand model) => Ok(await Mediator.Send(model));
        

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            
            return Ok(await Mediator.Send(new DeleteUnitCommodityQuotaCommand(){ Id= id }));
        }
        [HttpPut]
        public async Task<IActionResult> update([FromBody] UpdateUnitCommodityQuotaCommand model) => Ok(await Mediator.Send(model));

    }
}