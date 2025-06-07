using Eefa.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class FinancialRequestLogs:BaseEntity
    {
        public string RequestJSON { get; set; }
        public string RequestType { get; set; }
        public string ResponseJSON { get; set; }
        public int Status { get; set; }
        public string ProjectTitle { get; set; }
        public virtual Users CreatedBy { get; set; } = default!;

    }
}
