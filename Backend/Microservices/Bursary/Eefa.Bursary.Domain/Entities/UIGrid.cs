using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class UIGrid : BaseEntity
    {
        public UIGrid()
        {
            UIGridColumns = new HashSet<UIGridColumns>();
        }

         
        public bool Searchable { get; set; } = default!;
        public bool ServerSideLoading { get; set; } = default!;
        public bool Filterable { get; set; } = default!;
        public bool Sortable { get; set; } = default!;
        public bool Groupable { get; set; } = default!;
        public string DataSource { get; set; } = default!;

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
//آیا حذف شده است؟
        /// </summary>
         

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        public virtual UIFormControls IdNavigation { get; set; } = default!;
        public virtual ICollection<UIGridColumns> UIGridColumns { get; set; } = default!;
    }
}
