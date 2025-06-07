using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class UIFormControls : BaseEntity
    {
         
        public int FormId { get; set; } = default!;
        public int ParenId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int ControlId { get; set; } = default!;
        public bool IsReadOnly { get; set; } = default!;
        public bool IsVisible { get; set; } = default!;
        public bool IsRequired { get; set; } = default!;
        public string? DataSource { get; set; }
        public string? HelpText { get; set; }

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
         

        public virtual UIForms Form { get; set; } = default!;
        public virtual UIGrid UIGrid { get; set; } = default!;
    }
}
