using FluentValidation;

namespace Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Delete
{
    public class DeleteVouchersDetailsByDocumentIdsValidator : AbstractValidator<DeleteVouchersDetailsByDocumentIdsCommand>
    {
        public DeleteVouchersDetailsByDocumentIdsValidator()
        {
        }
    }
}
