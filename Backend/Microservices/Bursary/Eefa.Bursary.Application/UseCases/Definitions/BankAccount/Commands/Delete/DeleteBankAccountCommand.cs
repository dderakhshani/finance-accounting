using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Update;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Commands.Delete
{
    public class DeleteBankAccountCommand : CommandBase, IRequest<ServiceResult<BankAccounts>>, IMapFrom<DeleteBankCommand>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteBankAccountCommandHandler : IRequestHandler<DeleteBankAccountCommand, ServiceResult<BankAccounts>>
    {
        private readonly IBursaryUnitOfWork _uow;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public DeleteBankAccountCommandHandler(IBursaryUnitOfWork uow, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<BankAccounts>> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
        {
            var bankaccount = await _uow.BankAccounts.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (bankaccount == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }

            bankaccount.IsDeleted = true;
            bankaccount.ModifiedAt = DateTime.UtcNow;
            bankaccount.ModifiedById = _currentUserAccessor.GetId();
            _uow.BankAccounts.Update(bankaccount);

            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در حذف اطلاعات بانک");
            return ServiceResult<BankAccounts>.Success(bankaccount);
        }
    }


}
