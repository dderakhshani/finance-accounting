using AutoMapper;
using Eefa.Bursary.Application.Commands.Cheque.Create;
using Eefa.Bursary.Domain.Aggregates.ChequeAggregate;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Add
{
    public class CreateChequeBookCommand : CommandBase, IRequest<ServiceResult<Payables_ChequeBooks>>, IMapFrom<CreateChequeBookCommand>, ICommand
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public DateTime GetDate { get; set; }
        public string Serial { get; set; }
        public int SheetsCount { get; set; }
        public long StartNumber { get; set; }
        public string? Descp { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateChequeBookCommand, Payables_ChequeBooks>().IgnoreAllNonExisting();
        }
    }

    public class CreateChequeBookCommandHandler : IRequestHandler<CreateChequeBookCommand, ServiceResult<Payables_ChequeBooks>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public CreateChequeBookCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Payables_ChequeBooks>> Handle(CreateChequeBookCommand request, CancellationToken cancellationToken)
        {
            var chequeBook = _mapper.Map<Payables_ChequeBooks>(request);

            if (chequeBook == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }

            _uow.Payables_ChequeBooks.Add(chequeBook);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات دسته چک");

            for (int i = 0; i < request.SheetsCount; i++)
            {
                var sheet = new Payables_ChequeBooksSheets()
                {
                    ChequeBookId = chequeBook.Id,
                    ChequeSheetNo = chequeBook.StartNumber + i
                };
                _uow.Payables_ChequeBooksSheets.Add(sheet);
            }

            value = await _uow.SaveChangesAsync(cancellationToken);
            if (value <= 0)
            {
                _uow.Payables_ChequeBooks.Remove(chequeBook);
                throw new Exception("بروز خطا در ثبت اطلاعات برگه های دسته چک");
            }
            return ServiceResult<Payables_ChequeBooks>.Success(chequeBook);
        }
    }
}
