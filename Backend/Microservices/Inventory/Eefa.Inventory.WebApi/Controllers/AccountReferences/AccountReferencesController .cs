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
    public class AccountReferencesController : ApiControllerBase
    {

        IAccountReferences _accountReferences;

        public AccountReferencesController(
            IAccountReferences accountReferences
            )
        {
            _accountReferences = accountReferences ?? throw new ArgumentNullException(nameof(accountReferences));

        }

        [HttpPost]
        public async Task<IActionResult> GetAccountReferences(PaginatedQueryModel paginatedQuery,int ? AccountHeadId)
        {
            var result = await _accountReferences.GetAccountReferences(paginatedQuery, AccountHeadId);

            return Ok(ServiceResult<PagedList<AccountReferenceViewModel>>.Success(result));
        }
        [HttpPost]

        public async Task<IActionResult> GetAccountReferencesPerson(PaginatedQueryModel paginatedQuery)
        {


            var result = await _accountReferences.GetAccountReferencesPerson(paginatedQuery);

            return Ok(ServiceResult<PagedList<AccountReferenceViewModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetRequesterReferenceWarhouse(
            int DocumentStauseBaseValue,
            string FromDate,
            string ToDate, 
            PaginatedQueryModel paginatedQuery)
        {

            var result = await _accountReferences.GetRequesterReferenceWarhouse(DocumentStauseBaseValue, FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<AccountReferenceModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetReferenceReceipt(PaginatedQueryModel paginatedQuery)
        {

            var result = await _accountReferences.GetReferenceReceipt(paginatedQuery);

            return Ok(ServiceResult<PagedList<AccountReferenceModel>>.Success(result));
        }
        [HttpPost]

        public async Task<IActionResult> GetAccountReferencesProvider(PaginatedQueryModel paginatedQuery)
        {


            var result = await _accountReferences.GetAccountReferencesProvider(paginatedQuery);

            return Ok(ServiceResult<PagedList<AccountReferenceViewModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetAccountHead(PaginatedQueryModel paginatedQuery)
        {
            var result = await _accountReferences.GetAccountHead(paginatedQuery);

            return Ok(ServiceResult<PagedList<AccountHead>>.Success(result));
        }
    }
}