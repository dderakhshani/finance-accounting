using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Inventory.Domain.Enum
{
    public enum WarehouseLayoutStatus : int
    {
        [Description("آزاد")]
        Free = 0,
        [Description("فقط ورودی ")]
        InputOnly = 1,
        [Description("فقط خروجی ")]
        OutputOnly = 2,
        [Description("قفل موقت ")]
        TemporaryLock = 3,
        [Description("قفل دائم")]
        PermanentLock=4
    }
}
