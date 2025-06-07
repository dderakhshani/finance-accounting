using System.Collections.Generic;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.EntityFrameworkCore;
using Eefa.Accounting.Data.Databases.SqlServer.Context;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Query.GetAll
{
    public class GetAllCodeVoucherGroupQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }

    }

    public class GetAllCodeVoucherGroupQueryHandler : IRequestHandler<GetAllCodeVoucherGroupQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAccountingUnitOfWork _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public GetAllCodeVoucherGroupQueryHandler(IRepository repository, IMapper mapper, IAccountingUnitOfWork context, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _repository = repository;
            _context = context;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult> Handle(GetAllCodeVoucherGroupQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll<Data.Entities.CodeVoucherGroup>().ApplyPermission<Data.Entities.CodeVoucherGroup>(_context,_currentUserAccessor,false,false)
                .ProjectTo<CodeVoucherGroupModel>(_mapper.ConfigurationProvider)
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
