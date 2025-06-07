using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Application.UseCases.VoucherAttachment.Model;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VoucherAttachment.Query.GetAllByVoucherHeadId
{
    public class GetAllVoucherAttachmentByVoucherHeadIdQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public int VoucherHeadId { get; set; }
    }

    public class GetAllVoucherAttachmentQueryHandler : IRequestHandler<GetAllVoucherAttachmentByVoucherHeadIdQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllVoucherAttachmentQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllVoucherAttachmentByVoucherHeadIdQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
            .GetAll<Data.Entities.VoucherAttachment>(c =>
                 c.Paginate(new Pagination()
                 {
                     PageSize = request.PageSize,
                     PageIndex = request.PageIndex,
                      
                      
                 }).ConditionExpression(x=>x.VoucherHeadId == request.VoucherHeadId))
            .ProjectTo<VoucherAttachmentModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken));
        }
    }
}
