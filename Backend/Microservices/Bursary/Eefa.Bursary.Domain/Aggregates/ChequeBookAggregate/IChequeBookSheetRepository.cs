using Eefa.Bursary.Domain.Entities;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Domain.Aggregates.ChequeBookAggregate
{
    public interface IPayables_ChequeBooksSheetsRepository : IRepository<Payables_ChequeBooksSheets>
    {
    }
}
