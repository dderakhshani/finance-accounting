using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Inventory.Domain.Enum
{
    public enum WarehouseStateForm : int
    {
        [Description("صدور فرم شمارش")]
        IssuanceOfCountingForm = 0,
        [Description("اتمام شمارش و در انتظار تایید")]
        CompleteCounting = 1,
        [Description("تایید شمارش")]
        ConfrimCountingWithoutConflict = 2,
        [Description(" تایید و صدور فرم شمارش برای مغایرت")]
        ConfirmCountingAndIssuanceCountingConflicts = 3,
        [Description("عملیات کسری و اضافی")]
        IssuingDeductionAndExtraForm = 4,
        [Description("ابطال شمارش")]
        CountCancellation = 5
    }
}
