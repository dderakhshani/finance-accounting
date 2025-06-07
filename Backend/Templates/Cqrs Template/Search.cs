using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.Services.$fileinputname$.Model;
using MediatR;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data.SqlServer;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utility;

namespace $rootnamespace$.$fileinputname$.Query.Search
{
    public class $safeitemname$ : Pagination,IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<SearchQueryItem> Queries { get; set; }
    }

    public class $safeitemname$Handler : IRequestHandler<$safeitemname$, ServiceResult>
    {
        private readonly IMapper _mapper;

        public $safeitemname$Handler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle($safeitemname$ request, CancellationToken cancellationToken)
        {
            return ServiceResult.Set(true);

        }
    }
}
    