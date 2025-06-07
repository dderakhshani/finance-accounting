using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1662;&#1740;&#1575;&#1605; &#1607;&#1575;&#1740; &#1575;&#1593;&#1578;&#1576;&#1575;&#1585; &#1587;&#1606;&#1580;&#1740;
    /// </summary>
    public partial class ValidationMessages : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کلمات کلیدی
        /// </summary>
        public string Keyword { get; set; } = default!;

        /// <summary>
//پیام
        /// </summary>
        public string Message { get; set; } = default!;

        /// <summary>
//کد زبان
        /// </summary>
        public int LanguageId { get; set; } = default!;

 
   

        public virtual Languages Language { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
