using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Queries;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Queries
{
    public class GetDocumentOperationQuery : IRequest<ServiceResult<Payables_DocumentsOperations_View>>, IQuery
    {
        public int Id { get; set; }

    }

    public class GetDocumentOperationQueryHandler : IRequestHandler<GetDocumentOperationQuery, ServiceResult<Payables_DocumentsOperations_View>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public GetDocumentOperationQueryHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Payables_DocumentsOperations_View>> Handle(GetDocumentOperationQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.Payables_DocumentsOperations_View
                .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<Payables_DocumentsOperations_View>.Success(r);
        }
    }

}
