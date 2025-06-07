using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Queries
{
    public class GetAllChequeBookQuery : Pagination, IRequest<ServiceResult<PagedList<Payables_ChecqueBooks_View_ResponseModel>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }
    public class GetAllChequeBookQueryHandler : IRequestHandler<GetAllChequeBookQuery, ServiceResult<PagedList<Payables_ChecqueBooks_View_ResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_ChecqueBooks_View> _repository;

        public GetAllChequeBookQueryHandler(IMapper mapper, IRepository<Payables_ChecqueBooks_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Payables_ChecqueBooks_View_ResponseModel>>> Handle(GetAllChequeBookQuery request, CancellationToken cancellationToken)
        {

            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);
            return ServiceResult<PagedList<Payables_ChecqueBooks_View_ResponseModel>>.Success(new PagedList<Payables_ChecqueBooks_View_ResponseModel>()
            {
                Data = await entitis
                    .ProjectTo<Payables_ChecqueBooks_View_ResponseModel>(_mapper.ConfigurationProvider)
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
