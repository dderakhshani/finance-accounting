using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class DocumentHeadExtend : BaseEntity
    {
         

        /// <summary>
//شماره سند 
        /// </summary>
        public int DocumentHeadId { get; set; } = default!;

        /// <summary>
//کد درخواست کننده 
        /// </summary>
        public int? RequesterReferenceId { get; set; }

        /// <summary>
//کد پی گیری کننده 
        /// </summary>
        public int? FollowUpReferenceId { get; set; }

        /// <summary>
//کد تایید کننده جنس در انبار قرنطینه 
        /// </summary>
        public int? CorroborantReferenceId { get; set; }

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

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
         

        public virtual AccountReferences CorroborantReference { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual DocumentHeads DocumentHead { get; set; } = default!;
        public virtual AccountReferences FollowUpReference { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual AccountReferences RequesterReference { get; set; } = default!;
    }
}
