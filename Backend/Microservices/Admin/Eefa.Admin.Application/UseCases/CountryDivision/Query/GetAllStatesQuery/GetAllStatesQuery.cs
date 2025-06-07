using System.Linq;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.CountryDivision.Model;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.CountryDivision.Query.GetAllStatesQuery
{
    public class GetAllStatesQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        
    }

    public class GetAllStatesQueryHandler : IRequestHandler<GetAllStatesQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllStatesQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllStatesQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
            .GetAll<Data.Databases.Entities.CountryDivision>(c =>
                c.Paginate(new Pagination()
                 {
                     PageIndex = request.PageIndex,
                     PageSize = request.PageSize,
                      
                      
                 })).Select(x => new CountryDivisionModel(){Ostan = x.Ostan, OstanTitle = x.OstanTitle }).Distinct()
            //.ProjectTo<CountryDivisionModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken));
        }
    }
}
