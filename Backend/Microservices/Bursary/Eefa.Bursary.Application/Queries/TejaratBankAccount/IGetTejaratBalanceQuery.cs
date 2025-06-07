using Eefa.Bursary.Application.Queries.FinancialRequest;
using Eefa.Common.Data.Query;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Bursary.Application.Models;
using Eefa.Common;

namespace Eefa.Bursary.Application.Queries.TejaratBankAccount
{
   public interface IGetTejaratBalanceQuery : IQuery
    {
        Task<ServiceResult<List<AccountBalanceModel>>> GetAll(GetTejaratBalanceQuery query);
    }
}
