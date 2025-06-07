using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class BusinessRuleConditions : BaseEntity
    {
         
        public int BusinessRuleId { get; set; } = default!;
        public string TableName { get; set; } = default!;
        public string FieldName { get; set; } = default!;

 
        public int Operator { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? ForeignKeyName { get; set; }

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
         

        public virtual BusinessRules BusinessRule { get; set; } = default!;
    }
}
