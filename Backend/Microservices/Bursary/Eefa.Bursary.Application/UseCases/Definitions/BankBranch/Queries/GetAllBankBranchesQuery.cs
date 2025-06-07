using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Models;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Queries
{
    public class GetAllBankBranchesQuery : Pagination, IRequest<ServiceResult<PagedList<BankBranchResponseModel>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllBankBranchesQueryHandler : IRequestHandler<GetAllBankBranchesQuery, ServiceResult<PagedList<BankBranchResponseModel>>>
    {

        private readonly IMapper _mapper;
        private readonly IRepository<BankBranches_View> _repository;

        public GetAllBankBranchesQueryHandler(IMapper mapper, IRepository<BankBranches_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<BankBranchResponseModel>>> Handle(GetAllBankBranchesQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<BankBranchResponseModel>>.Success(new PagedList<BankBranchResponseModel>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ProjectTo<BankBranchResponseModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }

}
