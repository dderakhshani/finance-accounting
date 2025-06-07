using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Add;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.Bank.Commands.Add
{
    public class CreateBankCommand : CommandBase, IRequest<ServiceResult<Banks>>, IMapFrom<CreateBankCommand>, ICommand
    {
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? GlobalCode { get; set; }
        public int TypeBaseId { get; set; } = default!;
        public string? SwiftCode { get; set; }
        public string? ManagerFullName { get; set; }
        public string? Descriptions { get; set; }
        public string? TelephoneJson { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBankCommand, Banks>().IgnoreAllNonExisting();
        }

    }

    public class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, ServiceResult<Banks>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public CreateBankCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Banks>> Handle(CreateBankCommand request, CancellationToken cancellationToken)
        {
            var bank = _mapper.Map<Banks>(request);
            
            if (bank == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }
            _uow.Banks.Add(bank);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات بانک");

            return ServiceResult<Banks>.Success(bank);
        }
    }

}
