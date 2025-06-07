using AutoMapper;
using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Add;
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

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Commands.Update
{
    public class UpdateBankBranchCommand : CommandBase, IRequest<ServiceResult<BankBranches>>, IMapFrom<UpdateBankBranchCommand>, ICommand
    {
        public int Id { get; set; }
        public int BankId { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int CountryDivisionId { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string TelephoneJson { get; set; }
        public string ManagerFullName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBankBranchCommand, BankBranches>().IgnoreAllNonExisting();
        }

    }

    public class UpdateBankBranchCommandHandler : IRequestHandler<UpdateBankBranchCommand, ServiceResult<BankBranches>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public UpdateBankBranchCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<BankBranches>> Handle(UpdateBankBranchCommand request, CancellationToken cancellationToken)
        {
            var bankbranch = await _uow.BankBranches.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (bankbranch == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }
            _mapper.Map(request, bankbranch);

            _uow.BankBranches.Update(bankbranch);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات بانک");
            return ServiceResult<BankBranches>.Success(bankbranch);
        }
    }


}
