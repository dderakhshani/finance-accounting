using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.Cancel;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Commands.UnCancel
{
    public class UnCancelChequeBookSheetCommand : CommandBase, IRequest<ServiceResult<Payables_ChequeBooksSheets>>, ICommand
    {
        public int Id { get; set; }
    }

    public class UnCancelChequeBookSheetCommandHandler : IRequestHandler<UnCancelChequeBookSheetCommand, ServiceResult<Payables_ChequeBooksSheets>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public UnCancelChequeBookSheetCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Payables_ChequeBooksSheets>> Handle(UnCancelChequeBookSheetCommand request, CancellationToken cancellationToken)
        {
            var sheet = await _uow.Payables_ChequeBooksSheets.FirstOrDefaultAsync(w => w.Id == request.Id);
            if (sheet == null)
            {
                throw new ValidationError("برگه چک مورد نظر در سیستم وجود ندارد");
            }
            sheet.IsCanceled = false;
            sheet.CancelDescp = null;
            sheet.CancelDate = null;

            _uow.Payables_ChequeBooksSheets.Update(sheet);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
            {
                throw new Exception("بروز خطا در ابطال برگه چک");
            }
            return ServiceResult<Payables_ChequeBooksSheets>.Success(sheet);

        }
    }

}
