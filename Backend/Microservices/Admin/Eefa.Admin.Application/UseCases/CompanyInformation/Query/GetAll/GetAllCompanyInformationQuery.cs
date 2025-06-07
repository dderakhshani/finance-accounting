using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.CompanyInformation.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.CompanyInformation.Query.GetAll
{
    public class GetAllowedUserCompanyByUserIdQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions{ get; set; }

    }

    public class GetAllCompanyInformationQueryHandler : IRequestHandler<GetAllowedUserCompanyByUserIdQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCompanyInformationQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllowedUserCompanyByUserIdQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll<Data.Databases.Entities.CompanyInformation>()
                .ProjectTo<CompanyInformationModel>(_mapper.ConfigurationProvider)
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
