using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.CompanyInformation.Model;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.CompanyInformation.Query.Get
{
    public class GetCompanyInformationQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetCompanyInformationQueryHandler : IRequestHandler<GetCompanyInformationQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetCompanyInformationQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetCompanyInformationQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.CompanyInformation>(c
            => c.ObjectId(request.Id))
            .ProjectTo<CompanyInformationModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
