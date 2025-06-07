using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1601;&#1575;&#1740;&#1604; &#1607;&#1575;&#1740; &#1583;&#1585;&#1582;&#1608;&#1575;&#1587;&#1578; &#1605;&#1575;&#1604;&#1740;
    /// </summary>
    public partial class FinancialRequestAttachments : BaseEntity
    {
         

        /// <summary>
//شماره درخواست
        /// </summary>
        public int FinancialRequestId { get; set; } = default!;

        /// <summary>
//شماره ضمیمه 
        /// </summary>
        public int AttachmentId { get; set; } = default!;

        public int? ChequeSheetId { get; set; } = default!;

        /// <summary>
//آیا ضمیمه تایید شده هست ؟
        /// </summary>
        public bool IsVerified { get; set; } = default!;
 
        public virtual Attachment Attachment { get; set; } = default!;
        public virtual ChequeSheets ChequeSheet { get; set; }= default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual FinancialRequest FinancialRequest { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
