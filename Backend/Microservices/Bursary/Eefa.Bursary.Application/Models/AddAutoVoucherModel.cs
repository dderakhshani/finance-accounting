using System.Collections.Generic;

namespace Eefa.Bursary.Application.Models
{
    public class AddAutoVoucherModel
    {
        public bool saveChanges { get; set; } = true;
        public int menueId { get; set; } = 0;
        public List<BursaryDocumentModel> dataList { get; set; }
    }
}
