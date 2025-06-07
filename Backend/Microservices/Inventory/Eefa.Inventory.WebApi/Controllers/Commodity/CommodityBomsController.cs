using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Web;
using Eefa.Inventory.Application.Models;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class CommodityBomsController : ApiControllerBase
    {
        IBomsQueries _BomsQueries;

        public CommodityBomsController(
           
           IBomsQueries BomsQueries
          )
        {
            
            _BomsQueries = BomsQueries ?? throw new ArgumentNullException(nameof(BomsQueries));
            
        }

        
        //[HttpGet]
        //public async Task<IActionResult> Get(int id) => Ok(await _BomsQueries.GetById(id));

        //[HttpPost]
        //public async Task<IActionResult> GetAll(string FromDate,
        //    string ToDate, PaginatedQueryModel paginatedQuery) {

        //   var result= await _BomsQueries.GetAll(FromDate, ToDate,paginatedQuery);
        //   return Ok(ServiceResult<PagedList<CommodityBomsModel>>.Success(result));
        //}

        [HttpGet]
        public async Task<IActionResult> GetByCommodityId(int CommodityId)
        {

            var result = await _BomsQueries.GetByCommodityId(CommodityId);
            return Ok(ServiceResult<PagedList<CommodityBomsModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBomCommand model) => Ok(await Mediator.Send(model));


    }
}