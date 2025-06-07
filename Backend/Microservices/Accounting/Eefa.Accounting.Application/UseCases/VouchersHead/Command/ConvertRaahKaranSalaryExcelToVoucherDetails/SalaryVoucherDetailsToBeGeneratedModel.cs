namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.ConvertRaahKaranSalaryExcelToVoucherDetails
{
    public class SalaryVoucherDetailsToBeGeneratedModel
    {
        public string Title { get; set; }
        public string PropertyName { get; set; }
        public string AccountHeadCode { get; set; }
        public string AccountReferenceGroupCode { get; set; }
        public string AccountReferenceCode { get; set; }
        public bool IsCredit { get; set; }
        public int OrderIndex { get; set; }
    }
}
