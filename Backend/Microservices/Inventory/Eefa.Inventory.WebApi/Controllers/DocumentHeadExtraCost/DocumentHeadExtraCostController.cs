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
    public class DocumentHeadExtraCostController : ApiControllerBase
    {
        IDocumentHeadExtraCostQueries _DocumentHeadExtraCostQueries;

        public DocumentHeadExtraCostController(
           
           IDocumentHeadExtraCostQueries DocumentHeadExtraCostQueries
          )
        {
            _DocumentHeadExtraCostQueries = DocumentHeadExtraCostQueries ?? throw new ArgumentNullException(nameof(DocumentHeadExtraCostQueries));
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {

            var result = await _DocumentHeadExtraCostQueries.GetById(id);
            return Ok(ServiceResult<DocumentHeadExtraCostModel>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetTotalExtraCost(ListIdModel model)
        {

            var result = await _DocumentHeadExtraCostQueries.GetTotalExtraCost(model.ListIds);
            return Ok(ServiceResult<decimal>.Success(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> GetByDocumentHeadId(ListIdModel model)
        {

            var result = await _DocumentHeadExtraCostQueries.GetByDocumentHeadId(model.ListIds);
            return Ok(ServiceResult< PagedList<DocumentHeadExtraCostModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery) {

           var result= await _DocumentHeadExtraCostQueries.GetAll(paginatedQuery);
           return Ok(ServiceResult<PagedList<DocumentHeadExtraCostModel>>.Success(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> Modify([FromBody] DocumentHeadExtraCostCommand model) => Ok(await Mediator.Send(model));
        

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            
            return Ok(await Mediator.Send(new DeleteDocumentHeadExtraCostCommand(){ Id= id }));
        }
        

    }
}