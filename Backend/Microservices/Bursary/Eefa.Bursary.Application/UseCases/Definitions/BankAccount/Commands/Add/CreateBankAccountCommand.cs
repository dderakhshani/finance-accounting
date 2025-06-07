using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Eefa.Bursary.Domain.Entities.Definitions;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common.Exceptions;
using System;
using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Add;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Commands.Add
{
    public class CreateBankAccountCommand : CommandBase, IRequest<ServiceResult<BankAccounts>>, IMapFrom<CreateBankAccountCommand>, ICommand
    {
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
            profile.CreateMap<CreateBankAccountCommand, BankAccounts>().IgnoreAllNonExisting();
        }

    }

    public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, ServiceResult<BankAccounts>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public CreateBankAccountCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<BankAccounts>> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
        {
            var bankaccount = _mapper.Map<BankAccounts>(request);

            if (bankaccount == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }
            _uow.BankAccounts.Add(bankaccount);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات بانک");

            return ServiceResult<BankAccounts>.Success(bankaccount);
        }
    }


}
