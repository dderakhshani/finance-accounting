using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Models;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Queries
{
    public class GetChequeBookQuery : IRequest<ServiceResult<Payables_ChecqueBooks_View_ResponseModel>>, IQuery
    {
        public int Id { get; set; }
    }
    public class GetCheckBookQueryHandler : IRequestHandler<GetChequeBookQuery, ServiceResult<Payables_ChecqueBooks_View_ResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public GetCheckBookQueryHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Payables_ChecqueBooks_View_ResponseModel>> Handle(GetChequeBookQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.Payables_ChecqueBooks_View
                .ProjectTo<Payables_ChecqueBooks_View_ResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<Payables_ChecqueBooks_View_ResponseModel>.Success(r);
        }
    }

}
