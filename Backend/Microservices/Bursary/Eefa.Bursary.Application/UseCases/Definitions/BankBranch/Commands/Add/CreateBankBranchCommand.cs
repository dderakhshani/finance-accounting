using AutoMapper;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Add;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Add
{
    public class CreateBankBranchCommand : CommandBase, IRequest<ServiceResult<BankBranches>>, IMapFrom<CreateBankBranchCommand>, ICommand
    {
        public int BankId { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int CountryDivisionId { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string TelephoneJson { get; set; }
        public string ManagerFullName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBankBranchCommand, BankBranches>().IgnoreAllNonExisting();
        }


    }

    public class CreateBankBranchCommandHandler : IRequestHandler<CreateBankBranchCommand, ServiceResult<BankBranches>>
    {

        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public CreateBankBranchCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<BankBranches>> Handle(CreateBankBranchCommand request, CancellationToken cancellationToken)
        {
            var bankbranch = _mapper.Map<BankBranches>(request);

            if (bankbranch == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }
            _uow.BankBranches.Add(bankbranch);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات بانک");

            return ServiceResult<BankBranches>.Success(bankbranch);
        }
    }


}
