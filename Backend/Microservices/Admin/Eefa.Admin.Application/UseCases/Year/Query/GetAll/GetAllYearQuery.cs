using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Year.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Year.Query.GetAll
{
    public class GetAllYearQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions{ get; set; }

    }

    public class GetAllYearQueryHandler : IRequestHandler<GetAllYearQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllYearQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllYearQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll<Data.Databases.Entities.Year>()
                .ProjectTo<YearModel>(_mapper.ConfigurationProvider)
                .WhereQueryMaker(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult.Success(new PagedList()
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
