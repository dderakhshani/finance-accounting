using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Data.SqlServer;
using Infrastructure.Interfaces;
using Infrastructure.Utility;
using MediatR;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Accounting.Application.CommandQueries.$fileinputname$.Model;
using Infrastructure.Data.Models;

namespace $rootnamespace$.$fileinputname$.Query.Search
{
    public class $safeitemname$ : Pagination,IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<SearchQueryItem> Queries { get; set; }
    }

    public class $safeitemname$Handler : IRequestHandler<$safeitemname$, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public $safeitemname$Handler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle($safeitemname$ request, CancellationToken cancellationToken)
        {
              return ServiceResult.Set(
                (await _repository
                    .GetAll<Data.Entities.$fileinputname$>(config => config
                        .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Data.Entities.$fileinputname$>(request.Queries))
                         .Paginate(new Pagination()
                        {
                             Skip = request.Skip,
                             Take = request.Take,
                             OrderByProperty = request.OrderByProperty,
                             SortDirection = request.SortDirection
                        }))
                    .ProjectTo<$fileinputname$Model>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)));
        }
    }
}
    