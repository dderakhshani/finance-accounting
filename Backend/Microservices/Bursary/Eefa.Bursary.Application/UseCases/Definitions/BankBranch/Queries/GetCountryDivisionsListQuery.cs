using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Models;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Queries
{
    public class GetCountryDivisionsListQuery : Pagination, IRequest<ServiceResult<PagedList<CountryDivisionResponseModel>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetCountryDivisionsListQueryHandler : IRequestHandler<GetCountryDivisionsListQuery, ServiceResult<PagedList<CountryDivisionResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CountryDivisions_View> _repository;

        public GetCountryDivisionsListQueryHandler(IMapper mapper, IRepository<CountryDivisions_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<CountryDivisionResponseModel>>> Handle(GetCountryDivisionsListQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<CountryDivisionResponseModel>>.Success(new PagedList<CountryDivisionResponseModel>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ProjectTo<CountryDivisionResponseModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }


}
