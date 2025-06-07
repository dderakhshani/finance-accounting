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


    public class CodeVoucherGroupsController : ApiControllerBase
    {
        ICodeVoucherGroupsQueries _Repository;
       

        public CodeVoucherGroupsController(

           ICodeVoucherGroupsQueries repository

            )
        {
            _Repository = repository ?? throw new ArgumentNullException(nameof(repository));

        }

        [HttpPost]
        public async Task<IActionResult> GetReceiptALLVoucherGroup(string Code)
        {
           var result = await _Repository.GetReceiptALLVoucherGroup(Code);
            return Ok(ServiceResult<PagedList<ReceiptALLStatusModel>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetALL(PaginatedQueryModel query)
        {
            var result = await _Repository.GetALL();
            return Ok(ServiceResult<PagedList<ReceiptALLStatusModel>>.Success(result));

        }


    }
}
