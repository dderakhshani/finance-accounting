using AutoMapper;

public class AutoVoucherFormulaModel : IMapFrom<AutoVoucherFormula>
{
    public int Id { get; set; }
    public int VoucherTypeId { get; set; } = default!;
    public int SourceVoucherTypeId { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
    public byte DebitCreditStatus { get; set; } = default!;
    public int AccountHeadId { get; set; } = default!;
    public string? RowDescription { get; set; }
    public string? Formula { get; set; }
    public string? Conditions { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<AutoVoucherFormula, AutoVoucherFormulaModel>().IgnoreAllNonExisting();
    }
}