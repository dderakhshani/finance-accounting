using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Contracts.BursaryAccounting
{
    public class FinancialEvent
    {
        public string EventType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
    }

}
