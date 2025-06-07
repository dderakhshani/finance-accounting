using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Inventory.Application;

using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class AssetsController : ApiControllerBase
    {
        IAssetsQueries _AssetsQueries;
        public AssetsController(

           IAssetsQueries assetsQueries
          )
        {
            _AssetsQueries = assetsQueries ?? throw new ArgumentNullException(nameof(assetsQueries));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _AssetsQueries.GetById(id));

        [HttpPost]
        public async Task<IActionResult> GetAll(string FromDate,string ToDate, PaginatedQueryModel paginatedQuery) {
            

           var result= await _AssetsQueries.GetAll(FromDate, ToDate,paginatedQuery);
           return Ok(ServiceResult<PagedList<AssetsModel>>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetByDocumentId(int DocumentHeadId, int CommodityId) => Ok(await _AssetsQueries.GetByDocumentId(DocumentHeadId, CommodityId));
        
        [HttpGet]
        public async Task<IActionResult> GetLastNumber(int AssetGroupId)
        {

            var result =await _AssetsQueries.GetLastNumber(AssetGroupId);
            
            return Ok(ServiceResult<string>.Success(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> GetDuplicateAssets([FromBody] AssetsserialDuplicate AssetsSerial)
        {

            var result = await _AssetsQueries.GetDuplicateAssets(AssetsSerial.AssetsSerial);
            return Ok(ServiceResult<AssetsSerialModel>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetAttachmentsIdByPersonsDebitedCommoditiesId(int AssetId, int PersonsDebitedCommoditiesId)
        {

            var result = await _AssetsQueries.GetAssetAttachmentsIdByPersonsDebitedCommoditiesId(AssetId, PersonsDebitedCommoditiesId);
            return Ok(ServiceResult<int[]>.Success(result));
        }
        [HttpPut]
        public async Task<IActionResult> update([FromBody] UpdateAssetsCommand model) => Ok(await Mediator.Send(model));

    }
}