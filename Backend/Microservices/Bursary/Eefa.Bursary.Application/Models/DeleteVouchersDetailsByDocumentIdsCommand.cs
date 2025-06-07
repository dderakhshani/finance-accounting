using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Models
{
   public class DeleteVouchersDetailsByDocumentIdsCommand
    {
        public int VoucherId { get; set; }
        public List<int> DocumentIds { get; set; }

    }
}
