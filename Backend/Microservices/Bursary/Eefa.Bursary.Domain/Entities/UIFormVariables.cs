using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class UIFormVariables : BaseEntity
    {
         
        public int UiFormId { get; set; } = default!;
        public string VariableName { get; set; } = default!;

        /// <summary>
//1=int 2=String 3=Array
        /// </summary>
        public short Type { get; set; } = default!;

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
         

        public virtual UIForms UiForm { get; set; } = default!;
    }
}
