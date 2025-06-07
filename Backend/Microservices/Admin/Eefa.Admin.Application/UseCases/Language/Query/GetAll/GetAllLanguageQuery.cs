using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Language.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;

namespace Eefa.Admin.Application.CommandQueries.Language.Query.GetAll
{
    public class GetAllLanguageQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }

    }

    public class GetAllLanguageQueryHandler : IRequestHandler<GetAllLanguageQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllLanguageQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllLanguageQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
            .GetAll<Data.Databases.Entities.Language>()
            .ProjectTo<LanguageModel>(_mapper.ConfigurationProvider)
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
