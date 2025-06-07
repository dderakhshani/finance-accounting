using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.BaseValue.Model;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.BaseValue.Query.GetAllByCategoryUniqueName
{
    public class GetAllByCategoryUniqueNameQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public string BaseValueTypeUniqueName { get; set; }
    }

    public class GetAllBaseValueQueryHandler : IRequestHandler<GetAllByCategoryUniqueNameQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllBaseValueQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllByCategoryUniqueNameQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
            .GetAll<Data.Databases.Entities.BaseValue>(c =>
                c.ConditionExpression(x => x.BaseValueType.UniqueName == request.BaseValueTypeUniqueName)
                .Paginate(new Pagination()
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                }))
            .ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken));
        }
    }
}
