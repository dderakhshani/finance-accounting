using FluentValidation;

namespace Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Create
{
    public class CreateVouchersDetailValidator: AbstractValidator<CreateVouchersDetailCommand>
    {
        public CreateVouchersDetailValidator()
        {
        }
    }
}
