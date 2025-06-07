using Eefa.Common.Data;

namespace Eefa.Purchase.Domain.Entities
{

    public partial class AccountHeadRelReferenceGroup : BaseEntity
    {

        /// <summary>
        /// کد حساب سرپرست
        /// </summary>
        public int AccountHeadId { get; set; } = default!;

        /// <summary>
        /// کد گروه مرجع
        /// </summary>
        public int ReferenceGroupId { get; set; } = default!;

        /// <summary>
        /// شماره مرجع
        /// </summary>
        public int ReferenceNo { get; set; } = default!;

        /// <summary>
        /// بدهکار است؟
        /// </summary>
        public bool IsDebit { get; set; } = default!;

        /// <summary>
        /// معتبر است؟
        /// </summary>
        public bool IsCredit { get; set; } = default!;

     
        public virtual AccountHead AccountHead { get; set; } = default!;
        public virtual AccountReferencesGroup AccountReferencesGroup { get; set; } = default!;
 
    }
}
