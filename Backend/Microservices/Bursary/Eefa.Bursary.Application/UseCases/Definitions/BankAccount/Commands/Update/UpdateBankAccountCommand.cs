using AutoMapper;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Commands.Add;
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

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Commands.Update
{
    public class UpdateBankAccountCommand : CommandBase, IRequest<ServiceResult<BankAccounts>>, IMapFrom<UpdateBankAccountCommand>, ICommand
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public int BankBranchId { get; set; } = default!;
        public string Sheba { get; set; } = default!;
        public string AccountNumber { get; set; } = default!;
        public int? SubsidiaryCodeId { get; set; }
        public int? RelatedBankAccountId { get; set; }
        public int AccountTypeBaseId { get; set; } = default!;
        public int AccountHeadId { get; set; }
        public int? AccountReferencesGroupId { get; set; }
        public int? ReferenceId { get; set; }
        public int? AccountStatus { get; set; }
        public decimal WithdrawalLimit { get; set; } = 0;
        public bool HaveChekBook { get; set; } = default!;
        public int CurrenceTypeBaseId { get; set; } = default!;
        public string SignersJson { get; set; } = "";

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBankAccountCommand, BankAccounts>().IgnoreAllNonExisting();
        }
    }

    public class UpdateBankAccountCommandHandler : IRequestHandler<UpdateBankAccountCommand, ServiceResult<BankAccounts>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public UpdateBankAccountCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<BankAccounts>> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
        {
            var bankaccount = await _uow.BankAccounts.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (bankaccount == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }
            _mapper.Map(request, bankaccount);

            _uow.BankAccounts.Update(bankaccount);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات بانک");
            return ServiceResult<BankAccounts>.Success(bankaccount);
        }
    }


}
