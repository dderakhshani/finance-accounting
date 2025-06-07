using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Queries
{
    public class GetChequeBookSheetQuery:IRequest<ServiceResult<ChequeBookSheetResponseModel>>,IQuery
    {
        public int Id { get; set; }
    }

    public class GetChequeBookSheetQueryHandler : IRequestHandler<GetChequeBookSheetQuery, ServiceResult<ChequeBookSheetResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public GetChequeBookSheetQueryHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<ChequeBookSheetResponseModel>> Handle(GetChequeBookSheetQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.Payables_ChequeBooksSheets_View
                .ProjectTo<ChequeBookSheetResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(w=>w.Id == request.Id);
            return ServiceResult<ChequeBookSheetResponseModel>.Success(r);
        }
    }


}
