using Eefa.Common;
using System;

namespace Eefa.Inventory.Application
{
    public class View_Sina_FinancialOperationNumberModel : IMapFrom<Domain.View_Sina_FinancialOperationNumber>
    {
        public int Id { get; set; }
        public string PaymentNumber { get; set; }

    }
   
}
