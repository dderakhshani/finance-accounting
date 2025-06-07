using System.Collections.Generic;

namespace Eefa.Bursary.Application.UseCases.AutoVouchers.GeneralAV.Models
{
    public class AddVoucherGenericModel
    {
        public bool saveChanges { get; set; } = true;
        public int menueId { get; set; } = 0;
        public List<dynamic> dataList { get; set; }
    }
}
