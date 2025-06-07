namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.CreateAutoVoucher
{

    public partial class CreateAutoVoucherCommandHandler
    {
        public class AutoVoucherDetailResult
        {
            public int VoucherHeadId { get; set; }
            public int? DocumentId { get; set; }
            public int VoucherNo { get; set; }
        }

    }

}
