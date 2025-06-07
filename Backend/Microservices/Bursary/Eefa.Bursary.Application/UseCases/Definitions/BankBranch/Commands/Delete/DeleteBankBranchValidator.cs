using FluentValidation;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Delete
{
    public class DeleteBankBranchValidator : AbstractValidator<DeleteBankBranchCommand>
    {
        public DeleteBankBranchValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("شناسه شعبه بانک وارد نشده است");
        }
    }
}
