using System.Collections.Generic;

namespace Eefa.Bursary.Application.UseCases.AutoVouchers.PayableDocs.Models
{
    public class PayableDocument_Issue_Voucher_Model
    {
        public bool saveChanges { get; set; } = true;
        public int menueId { get; set; } = 0;
        public List<PayableDocument_Issue_Voucher_DataList_Model> dataList { get; set; }
    }
}
