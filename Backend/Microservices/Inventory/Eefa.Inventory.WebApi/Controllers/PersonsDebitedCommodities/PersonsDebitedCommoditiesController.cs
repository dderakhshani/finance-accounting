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
    public class PersonsDebitedCommoditiesController : ApiControllerBase
    {
        IPersonsDebitedCommoditiesQueries _Queries;

        public PersonsDebitedCommoditiesController(

           IPersonsDebitedCommoditiesQueries Queries
          )
        {

            _Queries = Queries ?? throw new ArgumentNullException(nameof(Queries));
            
        }
        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _Queries.GetById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll(string FromDate,
            string ToDate, PaginatedQueryModel paginatedQuery)
        {

            var result = await _Queries.GetAll(FromDate, ToDate, paginatedQuery);
            return Ok(ServiceResult<PagedList<PersonsDebitedCommoditiesModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetByDocumentId(int DocumentHeadId, int CommodityId, PaginatedQueryModel paginatedQuery)
        {

            var result = await _Queries.GetByDocumentId(DocumentHeadId, CommodityId, paginatedQuery);
            return Ok(ServiceResult<PagedList<PersonsDebitedCommoditiesModel>>.Success(result));
        }

        //[HttpPut]
        //public async Task<IActionResult> update([FromBody] UpdatePersonsDebitedCommoditiesCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateNewPersonsDebited([FromBody] UpdateNewPersonsDebitedCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateAssetsAttachment([FromBody] UpdateAssetsAttachmentCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> UpdateReturnToWarehouseDebited([FromBody] UpdateReturnToWarehouseDebitedCommand model) => Ok(await Mediator.Send(model));


        [HttpPost]
        public async Task<IActionResult> UpdateAssetSerialPersonsDebited([FromBody] UpdateAssetSerialPersonsDebitedCommand model) => Ok(await Mediator.Send(model));


        
    }
}