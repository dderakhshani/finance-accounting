using System;
using Eefa.Common;
using Eefa.Common.Data;

namespace Eefa.Purchase.Domain.Entities
{
    [HasUniqueIndex]
    public partial class CodeVoucherGroup : BaseEntity
    {
        [UniqueIndex]
        public string Code { get; set; } = default!;

        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;
        public string UniqueName { get; set; }

        /// <summary>
        /// تاریخ قفل شدن اطلاعات
        /// </summary>
        public DateTime? LastEditableDate { get; set; }
        public int OrderIndex { get; set; }
        /// <summary>
        /// اتوماتیک است؟
        /// </summary>
        public bool IsAuto { get; set; } = default!;

        /// <summary>
        /// آیا قابل ویرایش است؟
        /// </summary>
        public bool IsEditable { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;

        /// <summary>
        /// گروه سند مکانیزه
        /// </summary>
        public bool AutoVoucherEnterGroup { get; set; } = default!;

        /// <summary>
        /// فرمول جایگزین خالی بودن
        /// </summary>
        public string? BlankDateFormula { get; set; }

        /// <summary>
        /// کد گزارش
        /// </summary>
        public int? ViewId { get; set; }
        public int? ExtendTypeId { get; set; }
        public string? ExtendTypeName { get; set; }
        public string? TableName { get; set; }
        public string? SchemaName { get; set; }

    }
}
