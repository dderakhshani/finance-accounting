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

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Delete
{
    public class DeleteBankBranchCommand : CommandBase, IRequest<ServiceResult<BankBranches>>, IMapFrom<DeleteBankBranchCommand>, ICommand
    {
        public int Id { get; set; }

    }

    public class DeleteBankBranchCommandHandler : IRequestHandler<DeleteBankBranchCommand, ServiceResult<BankBranches>>
    {

        private readonly IBursaryUnitOfWork _uow;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public DeleteBankBranchCommandHandler(IBursaryUnitOfWork uow, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<BankBranches>> Handle(DeleteBankBranchCommand request, CancellationToken cancellationToken)
        {
            var bankbranch = await _uow.BankBranches.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (bankbranch == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }

            bankbranch.IsDeleted = true;
            bankbranch.ModifiedAt = DateTime.UtcNow;
            bankbranch.ModifiedById = _currentUserAccessor.GetId();
            _uow.BankBranches.Update(bankbranch);

            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در حذف اطلاعات بانک");
            return ServiceResult<BankBranches>.Success(bankbranch);
        }
    }


}
