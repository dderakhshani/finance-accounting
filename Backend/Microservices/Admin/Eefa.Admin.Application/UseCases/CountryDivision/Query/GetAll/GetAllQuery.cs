using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.CountryDivision.Query.GetAll
{
    public class GetAllQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {

    }

    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .GetAll<Data.Databases.Entities.CountryDivision>(c =>
                    c.Paginate(new Pagination()
                    {
                        PageIndex = request.PageIndex,
                        PageSize = request.PageSize,
                         
                         
                    }))
                .ToListAsync(cancellationToken));
        }
    }
}