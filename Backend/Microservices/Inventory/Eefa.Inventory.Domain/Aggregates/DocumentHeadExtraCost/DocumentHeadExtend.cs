using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    public partial class DocumentHeadExtend : DomainBaseEntity
    {

        public int DocumentHeadId { get; set; } = default!;

        /// <summary>
        /// کد درخواست کننده 
        /// </summary>
        public int? RequesterReferenceId { get; set; } = default!;

        /// <summary>
        /// کد پی گیری کننده 
        /// </summary>
        public int? FollowUpReferenceId { get; set; } = default!;

        /// <summary>
        ///کد تایید کننده جنس در انبار قرنطینه 
        /// </summary>
        public int? CorroborantReferenceId { get; set; } = default!;

       
        //public virtual AccountReference RequesterReference { get; set; } = default!;
        //public virtual AccountReference FollowUpReference { get; set; } = default!;
        //public virtual AccountReference CorroborantReference { get; set; } = default!;
        //public virtual Receipt Receipt { get; set; } = default!;
    }
}
