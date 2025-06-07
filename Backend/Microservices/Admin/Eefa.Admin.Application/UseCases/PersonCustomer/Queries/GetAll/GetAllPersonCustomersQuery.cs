using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.PersonCustomer.Models;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonCustomer.Queries.GetAll
{
    public class GetAllPersonCustomersQuery : Pagination, IRequest<ServiceResult>, IQuery
    {
        public List<Condition> Conditions { get; set; }
    }

    public class GetAllPersonCustomersQueryHandler : IRequestHandler<GetAllPersonCustomersQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPersonCustomersQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(GetAllPersonCustomersQuery request, CancellationToken cancellationToken)
        {
            var entities = _repository
                .GetAll<Data.Databases.Entities.Customer>()
                .ProjectTo<PersonCustomerModel>(_mapper.ConfigurationProvider)
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
