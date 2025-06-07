public class CorrectionRequestModel
{
    public int Id { get; set; } = default!;
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
}