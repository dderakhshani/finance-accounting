using System.Collections.Generic;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersHead.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Eefa.Accounting.Data.Databases.SqlServer.Context;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Query.GetAll
{
    public class GetAllVouchersHeadQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }
    }

    public class GetAllVouchersHeadQueryHandler : IRequestHandler<GetAllVouchersHeadQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUser;
        private IAccountingUnitOfWork _context;

        public GetAllVouchersHeadQueryHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUser, IAccountingUnitOfWork context)
        {
            _mapper = mapper;
            _currentUser = currentUser;
            _repository = repository;
            _context = context;
        }

        public async Task<ServiceResult> Handle(GetAllVouchersHeadQuery request, CancellationToken cancellationToken)
        {
            var entities = _context.VouchersHeads
                .Where(x => x.YearId == _currentUser.GetYearId())
                .Include(x => x.CreatedBy).ThenInclude(x => x.Person)
                .Include(x => x.ModifiedBy).ThenInclude(x => x.Person)
                .ProjectTo<VouchersHeadModel>(_mapper.ConfigurationProvider)
                .WhereQueryMaker(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult.Success(new
            {
                Data = await entities
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = await entities.CountAsync(cancellationToken),
                TotalCredit = await entities.SumAsync(x => x.TotalCredit),
                TotalDebit = await entities.SumAsync(x => x.TotalDebit)
            });

        }
    }
}
