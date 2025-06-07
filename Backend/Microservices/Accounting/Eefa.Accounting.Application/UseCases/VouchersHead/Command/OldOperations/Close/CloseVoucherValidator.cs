using FluentValidation;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.Close
{
    public class CloseVoucherValidator : AbstractValidator<CreateCloseVoucherCommand>
    {
        public CloseVoucherValidator()
        {

        }
    }
}
