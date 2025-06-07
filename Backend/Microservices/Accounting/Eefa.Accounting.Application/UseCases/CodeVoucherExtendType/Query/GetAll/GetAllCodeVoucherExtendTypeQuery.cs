using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Query.GetAll
{
    public class GetAllCodeVoucherExtendTypeQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }

    }

    public class GetAllCodeVoucherExtendTypeQueryHandler : IRequestHandler<GetAllCodeVoucherExtendTypeQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCodeVoucherExtendTypeQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllCodeVoucherExtendTypeQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll<Data.Entities.CodeVoucherExtendType>()
                .ProjectTo<CodeVoucherExtendTypeModel>(_mapper.ConfigurationProvider)
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
