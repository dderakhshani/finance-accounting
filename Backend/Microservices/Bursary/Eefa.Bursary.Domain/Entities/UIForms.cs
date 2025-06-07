using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class UIForms : BaseEntity
    {
        public UIForms()
        {
            UIFormControls = new HashSet<UIFormControls>();
            UIFormVariables = new HashSet<UIFormVariables>();
        }

         
        public string Title { get; set; } = default!;
        public string Name { get; set; } = default!;

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
         

        public virtual ICollection<UIFormControls> UIFormControls { get; set; } = default!;
        public virtual ICollection<UIFormVariables> UIFormVariables { get; set; } = default!;
    }
}
