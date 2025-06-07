using Eefa.Bursary.Domain.Entities;
using System.Collections.Generic;

namespace Eefa.Bursary.Application.Models
{
    public class Result<T>
    {
        public bool succeed { get; set; }
        public T objResult { get; set; }
    }

    public class VoucherModel{

        public List<int> voucherNo { get; set; }
        public List<int> voucherHeadId { get; set; }
        public List<VouchersHead> VoucherHeads { get; set; }
    }
  

}
