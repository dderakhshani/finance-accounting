using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public  class PermissionsModel : IMapFrom<Permissions>
    {
        /// <description>
         /// کد سطح
         ///</description>

        public string LevelCode { get; set; } = default!;
        public bool IsDataRowLimiter { get; set; } = default!;
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }

        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;

        /// <description>
        /// نام اختصاصی
        ///</description>

        public string UniqueName { get; set; } = default!;

        /// <description>
        /// مقدار
        ///</description>

        public string SubSystem { get; set; } = default!;

    }
}
