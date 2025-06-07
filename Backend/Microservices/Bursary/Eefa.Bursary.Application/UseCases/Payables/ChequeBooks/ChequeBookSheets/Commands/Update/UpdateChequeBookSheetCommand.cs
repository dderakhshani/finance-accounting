using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Add;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Update;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Update
{
    public class UpdateChequeBookSheetCommand : CommandBase, IRequest<ServiceResult<Payables_ChequeBooksSheets>>, IMapFrom<UpdateChequeBookSheetCommand>, ICommand
    {
        public int Id { get; set; }
        public string SayyadNo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateChequeBookSheetCommand, Payables_ChequeBooksSheets>().IgnoreAllNonExisting();
        }
    }

    public class UpdateChequeBookSheetCommandHandler : IRequestHandler<UpdateChequeBookSheetCommand, ServiceResult<Payables_ChequeBooksSheets>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public UpdateChequeBookSheetCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Payables_ChequeBooksSheets>> Handle(UpdateChequeBookSheetCommand request, CancellationToken cancellationToken)
        {
            var sheet = await _uow.Payables_ChequeBooksSheets.FirstOrDefaultAsync(w => w.Id == request.Id);
            if (sheet == null)
            {
                throw new ValidationError("برگه چک مورد نظر در سیستم وجود ندارد");
            }
            _mapper.Map(request, sheet);
            
            _uow.Payables_ChequeBooksSheets.Update(sheet);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
            {
                throw new Exception("بروز خطا در ثبت اطلاعات برگه چک");
            }
            return ServiceResult<Payables_ChequeBooksSheets>.Success(sheet);
        }
    }


}
