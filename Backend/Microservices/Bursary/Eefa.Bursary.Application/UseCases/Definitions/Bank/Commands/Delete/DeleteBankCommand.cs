using AutoMapper;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Add;
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

namespace Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Update
{
    public class DeleteBankCommand : CommandBase, IRequest<ServiceResult<Banks>>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteBankCommandHandler : IRequestHandler<DeleteBankCommand, ServiceResult<Banks>>
    {
        private readonly IBursaryUnitOfWork _uow;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public DeleteBankCommandHandler(IBursaryUnitOfWork uow, ICurrentUserAccessor currentUserAccessor)
        {
            _uow = uow;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<Banks>> Handle(DeleteBankCommand request, CancellationToken cancellationToken)
        {
            var bank = await _uow.Banks.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (bank == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }

            bank.IsDeleted = true;
            bank.ModifiedAt = DateTime.UtcNow;
            bank.ModifiedById = _currentUserAccessor.GetId();
            _uow.Banks.Update(bank);

            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در حذف اطلاعات بانک");
            return ServiceResult<Banks>.Success(bank);
        }
    }

}
