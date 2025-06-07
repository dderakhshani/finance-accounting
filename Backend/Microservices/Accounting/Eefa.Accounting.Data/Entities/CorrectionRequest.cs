using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class CorrectionRequest : BaseEntity
    {
        public int Status { get; set; } = default!;
        public int CodeVoucherGroupId { get; set; } = default!;
        public int? AccessPermissionId { get; set; } = default!;
        public int? DocumentId { get; set; }
        public string OldData { get; set; } = default!;
        public int VerifierUserId { get; set; } = default!;
        public string? PayLoad { get; set; }
        public string ApiUrl { get; set; } = default!;
        public string? ViewUrl { get; set; }
        public string? Description { get; set; }

        public virtual CodeVoucherGroup CodeVoucherGroup { get; set; } = default!;

    }
}
