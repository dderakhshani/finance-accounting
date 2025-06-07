using System.Collections.Generic;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Query.GetAll
{
    public class GetAllAutoVoucherFormulaQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }

    }

    public class GetAllAutoVoucherFormulaQueryHandler : IRequestHandler<GetAllAutoVoucherFormulaQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAutoVoucherFormulaQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllAutoVoucherFormulaQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll<Data.Entities.AutoVoucherFormula>()
                .ProjectTo<AutoVoucherFormulaModel>(_mapper.ConfigurationProvider)
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
