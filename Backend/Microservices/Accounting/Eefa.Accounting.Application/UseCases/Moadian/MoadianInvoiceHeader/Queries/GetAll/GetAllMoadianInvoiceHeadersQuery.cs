using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.EntityFrameworkCore;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Model;
using System.Linq;

namespace Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Queries.GetAll
{
    public class GetAllMoadianInvoiceHeadersQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }
        public bool IsProduction { get; set; }
    }


    public class GetAllMoadianInvoiceHeadersQueryHandler : IRequestHandler<GetAllMoadianInvoiceHeadersQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor currentUser;

        public GetAllMoadianInvoiceHeadersQueryHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUser)
        {
            _mapper = mapper;
            this.currentUser = currentUser;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllMoadianInvoiceHeadersQuery request, CancellationToken cancellationToken)
        {

            var entities = _repository
                .GetAll<Data.Entities.MoadianInvoiceHeader>()
                .Where(x => x.IsSandbox == !request.IsProduction)
                .Where(x => x.YearId == currentUser.GetYearId())
                .Include(x => x.CreatedBy).ThenInclude(x => x.Person)
                .ProjectTo<MoadianInvoiceHeaderModel>(_mapper.ConfigurationProvider)
                .WhereQueryMaker(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult.Success(new PagedList()
            {
                Data = await entities
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entities
                        .CountAsync(cancellationToken)
                    : 0
            });

        }
    }

}
