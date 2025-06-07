using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.VouchersDetail.Query.Get
{
    public class GetVouchersDetailQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
        public int? ReferenceId1 { get; set; }
    }

    public class GetVouchersDetailQueryHandler : IRequestHandler<GetVouchersDetailQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetVouchersDetailQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetVouchersDetailQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Entities.VouchersDetail>(c
            => c.ObjectId(request.Id))
            .ProjectTo<VouchersDetailModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
