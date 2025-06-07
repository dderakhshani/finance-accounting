using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Eefa.Inventory.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Eefa.Invertory.Infrastructure.Context;
using Eefa.Common.Exceptions;

namespace Eefa.Inventory.WebApi.Controllers.Receipt
{
    public class CallWindowsServiceController : ApiControllerBase
    {
        IReceiptQueries _receiptQueries;
        IReceiptCommandsService _adminApiService;
        ICurrentUserAccessor _currentUserAccessor;



        public CallWindowsServiceController(IReceiptQueries receiptQueries,
            IAccountReferences accountReferences,
            IReceiptCommandsService adminApiService,
            ICurrentUserAccessor currentUserAccessor
            )
        {
            _receiptQueries = receiptQueries ?? throw new ArgumentNullException(nameof(receiptQueries));
            _adminApiService = adminApiService;
            _currentUserAccessor = currentUserAccessor;

        }

        
        
       
    }


}
