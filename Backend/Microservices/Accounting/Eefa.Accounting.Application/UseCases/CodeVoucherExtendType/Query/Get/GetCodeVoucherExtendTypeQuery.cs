using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Model;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Query.Get
{
    public class GetCodeVoucherExtendTypeQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetCodeVoucherExtendTypeQueryHandler : IRequestHandler<GetCodeVoucherExtendTypeQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetCodeVoucherExtendTypeQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetCodeVoucherExtendTypeQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Entities.CodeVoucherExtendType>(c
            => c.ObjectId(request.Id))
            .ProjectTo<CodeVoucherExtendTypeModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
