using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
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

    }
}
