using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Add;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Commands.Update
{
    public class UpdateChequeBookCommand : CommandBase, IRequest<ServiceResult<Payables_ChequeBooks>>, IMapFrom<CreateChequeBookCommand>, ICommand
    {
        public int Id { get; set; }
        public DateTime GetDate { get; set; }
        public string Serial { get; set; }
        public int SheetsCount { get; set; }
        public long StartNumber { get; set; }
        public string? Descp { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateChequeBookCommand, Payables_ChequeBooks>().IgnoreAllNonExisting();
        }

    }

    public class UpdateChequeBookCommandHandler : IRequestHandler<UpdateChequeBookCommand, ServiceResult<Payables_ChequeBooks>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public UpdateChequeBookCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Payables_ChequeBooks>> Handle(UpdateChequeBookCommand request, CancellationToken cancellationToken)
        {
            var chequeBook = await _uow.Payables_ChequeBooks.AsNoTracking().AsQueryable().FirstOrDefaultAsync(w => w.Id == request.Id);
            if (chequeBook == null)
            {
                throw new ValidationError("دسته چک مورد نظر در سیستم وجود ندارد");
            }
            _mapper.Map(request, chequeBook);

            _uow.Payables_ChequeBooks.Update(chequeBook);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            var shts = await _uow.Payables_ChequeBooksSheets.Where(w => w.ChequeBookId == request.Id).ToListAsync();
            foreach (var s in shts)
            {
                _uow.Payables_ChequeBooksSheets.Remove(s);
            }

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
