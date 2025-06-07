using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.UseCases.Help.Model;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.UseCases.Help.Query.Get
{
    public class GetHelpQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetHelpQueryHandler : IRequestHandler<GetHelpQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetHelpQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetHelpQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.Help>(c
                => c.ObjectId(request.Id))
                .ProjectTo<HelpModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
