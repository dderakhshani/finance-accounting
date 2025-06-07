using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
   
   
    public partial class AccountReference : DomainBaseEntity
    {


        /// <summary>
        /// شناسه
        /// </summary>
       
        public string Code { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;


        //public virtual  ICollection<DocumentHeadExtend> RequesterDocumentHeadExtend { get; set; } = default!;
        //public virtual ICollection<DocumentHeadExtend> FollowUpDocumentHeadExtend { get; set; } = default!;
        //public virtual ICollection<DocumentHeadExtend> CorroborantDocumentHeadExtend { get; set; } = default!;


    }
}
