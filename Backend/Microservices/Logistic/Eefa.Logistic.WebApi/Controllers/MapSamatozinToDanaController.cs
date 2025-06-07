using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Logistic.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Logistic.WebApi.Controllers { 
    public class MapSamatozinToDanaController : ApiControllerBase
    {
        IMapSamatozinToDanaQueries _MapSamatozinToDanaQueries;


        public MapSamatozinToDanaController(IMapSamatozinToDanaQueries MapSamatozinToDanaQueries
            )
        {
            _MapSamatozinToDanaQueries = MapSamatozinToDanaQueries ?? throw new ArgumentNullException(nameof(MapSamatozinToDanaQueries));
        }

        [HttpGet]
       
        public async Task<IActionResult> Get(int id)
        {

            var result = await _MapSamatozinToDanaQueries.GetById(id);
            return Ok(ServiceResult<MapSamatozinToDanaModel>.Success(result));
        }
        
        [HttpPost]

        public async Task<IActionResult> GetAll(
           
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _MapSamatozinToDanaQueries.GetAll( paginatedQuery);

            return Ok(ServiceResult<PagedList<MapSamatozinToDanaModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateMapSamatozinToDanaCommand model) => Ok(await Mediator.Send(model));


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            return Ok(await Mediator.Send(new DeleteMapSamatozinToDanaCommand() { Id = id }));
        }
        
        [HttpPost]
        public async Task<IActionResult> update([FromBody] UpdateMapSamatozinToDanaCommand model) => Ok(await Mediator.Send(model));

       
    }
}