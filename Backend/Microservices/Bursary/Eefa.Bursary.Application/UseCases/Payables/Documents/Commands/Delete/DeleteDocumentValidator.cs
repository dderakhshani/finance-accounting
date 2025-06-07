using Eefa.Bursary.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Delete
{
    public class DeleteDocumentValidator : AbstractValidator<DeleteDocumentCommand>
    {
        private IBursaryUnitOfWork _uow;

        public DeleteDocumentValidator(IBursaryUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x.Id).MustAsync(doesExists).WithMessage("برگه پرداخت مورد نظر در سیستم وجود ندارد");
        }

        private async Task<bool> doesExists(int Id, CancellationToken cancellationToken)
        {
            var r = _uow.Payables_Documents.FirstOrDefaultAsync(x => x.Id == Id);
            if (r == null) return false;
            return true;
        }

    }
}
