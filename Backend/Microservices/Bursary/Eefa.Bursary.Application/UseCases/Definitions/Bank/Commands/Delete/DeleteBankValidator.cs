using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Update;
using Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Commands.Delete;
using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Update
{
    public class DeleteBankValidator : AbstractValidator<DeleteBankCommand>
    {
        public DeleteBankValidator()
        {

            RuleFor(x=>x.Id).GreaterThan(0).WithMessage("شناسه بانک ثبت نشده است");
        }
    }
}
