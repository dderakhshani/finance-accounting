using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public partial class DocumentHeadExtendModel : IMapFrom<DocumentHeadExtend>
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

        
        public string RequesterReferenceTitle { get; set; } = default!;
        
        public string FollowUpReferenceTitle { get; set; } = default!;

        public string CorroborantReferenceTitle { get; set; } = default!;

    }
}
