using System.Linq;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.CountryDivision.Model;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.CountryDivision.Query.GetAllCitiesQuery
{
    public class GetAllCitiesQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public string StateCode { get; set; }
    }

    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCitiesQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            if (request.StateCode == default)
            {
                return ServiceResult.Success(await _repository
                    .GetAll<Data.Databases.Entities.CountryDivision>(c =>
                        c.Paginate(new Pagination()
                        {
                            PageIndex = request.PageIndex,
                            PageSize = request.PageSize,
                             
                             
                        }))
                    .Select(x => new CountryDivisionModel()
                    { Id = x.Id, Shahrestan = x.Shahrestan, ShahrestanTitle = x.ShahrestanTitle }).Distinct()
                    .ToListAsync(cancellationToken));
            }
            else
            {
                return ServiceResult.Success(await _repository
                    .GetAll<Data.Databases.Entities.CountryDivision>(c =>
                        c.ConditionExpression(x => x.Ostan.Equals(request.StateCode))
                            .Paginate(new Pagination()
                            {
                                PageIndex = request.PageIndex,
                                PageSize = request.PageSize,
                                 
                                 
                            })).Select(x => new CountryDivisionModel()
                            { Id = x.Id, Shahrestan = x.Shahrestan, ShahrestanTitle = x.ShahrestanTitle }).Distinct()
                    //  .ProjectTo<CountryDivisionModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken));
            }
        }
    }
}
