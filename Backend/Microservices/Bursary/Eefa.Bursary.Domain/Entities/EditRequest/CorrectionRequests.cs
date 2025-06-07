using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities.EditRequest
{
    public partial class CorrectionRequests : BaseEntity
    {
         
        public short Status { get; set; } = default!;
        public int CodeVoucherGroupId { get; set; } = default!;
        public int? DocumentId { get; set; }
        public string OldData { get; set; } = default!;
        public int VerifierUserId { get; set; } = default!;
        public string PayLoad { get; set; }
        public string ApiUrl { get; set; } = default!;
        public string ViewUrl { get; set; }
        public string Description { get; set; }
        public string VerifierDescription { get; set; }
        public string RequesterDescription { get; set; }
        public int? AccessPermissionId { get; set; } = default!;
        public int YearId { get; set; } = default!;
    }
}
