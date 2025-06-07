using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Queries
{
    public class GetAllChequeBookSheetsQuery : Pagination, IRequest<ServiceResult<PagedList<ChequeBookSheetResponseModel>>>
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllChequeBookSheetsQueryHandler : IRequestHandler<GetAllChequeBookSheetsQuery, ServiceResult<PagedList<ChequeBookSheetResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_ChequeBooksSheets_View> _repository;

        public GetAllChequeBookSheetsQueryHandler(IMapper mapper, IRepository<Payables_ChequeBooksSheets_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<ChequeBookSheetResponseModel>>> Handle(GetAllChequeBookSheetsQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .ProjectTo<ChequeBookSheetResponseModel>(_mapper.ConfigurationProvider)
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);
            return ServiceResult<PagedList<ChequeBookSheetResponseModel>>.Success(new PagedList<ChequeBookSheetResponseModel>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }



}
