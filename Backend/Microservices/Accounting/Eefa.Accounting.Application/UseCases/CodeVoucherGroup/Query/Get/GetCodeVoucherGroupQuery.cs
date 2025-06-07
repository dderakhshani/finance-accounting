using AutoMapper;
using Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Query.Get
{
    public class GetCodeVoucherGroupQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetCodeVoucherGroupQueryHandler : IRequestHandler<GetCodeVoucherGroupQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetCodeVoucherGroupQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetCodeVoucherGroupQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Entities.CodeVoucherGroup>(c
            => c.ObjectId(request.Id))
            .ProjectTo<CodeVoucherGroupModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
