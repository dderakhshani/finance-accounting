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
    public class UpdateBankCommand : CommandBase, IRequest<ServiceResult<Banks>>, IMapFrom<UpdateBankCommand>, ICommand
    {
        public int Id { get; set; }
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
            profile.CreateMap<UpdateBankCommand, Banks>().IgnoreAllNonExisting();
        }

    }

    public class UpdateBankCommandHandler : IRequestHandler<UpdateBankCommand, ServiceResult<Banks>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public UpdateBankCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Banks>> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
        {
            var bank = await _uow.Banks.FirstOrDefaultAsync(x=>x.Id == request.Id);
            if (bank == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }
            _mapper.Map(request, bank);

            _uow.Banks.Update(bank);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات بانک");
            return ServiceResult<Banks>.Success(bank);
        }
    }

}
