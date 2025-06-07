using FluentValidation;

namespace Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Delete
{
    public class DeleteVouchersDetailValidator: AbstractValidator<DeleteVouchersDetailCommand>
    {
        public DeleteVouchersDetailValidator()
        {
        }
    }
}
