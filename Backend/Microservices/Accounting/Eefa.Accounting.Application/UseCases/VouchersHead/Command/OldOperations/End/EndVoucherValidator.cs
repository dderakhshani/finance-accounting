using Eefa.Accounting.Application.UseCases.VouchersHead.Command.OldOperations.End;
using FluentValidation;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.End
{
    public class EndVoucherValidator : AbstractValidator<EndVoucherCommand>
    {
        public EndVoucherValidator()
        {

        }
    }
}
