using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.PayTypes.Models;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.PayTypes.Queries
{
    public class GetAllPayTypesQuery : Pagination, IRequest<ServiceResult<PagedList<Payables_PayTypesResponseModel>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllPayTypesQueryHandler : IRequestHandler<GetAllPayTypesQuery, ServiceResult<PagedList<Payables_PayTypesResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_PayTypes_View> _repository;

        public GetAllPayTypesQueryHandler(IMapper mapper, IRepository<Payables_PayTypes_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Payables_PayTypesResponseModel>>> Handle(GetAllPayTypesQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<Payables_PayTypesResponseModel>>.Success(new PagedList<Payables_PayTypesResponseModel>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ProjectTo<Payables_PayTypesResponseModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }

    }

}
