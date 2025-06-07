using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Queries;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Queries
{
    public class GetDocumentQuery : IRequest<ServiceResult<Payables_Documents>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, ServiceResult<Payables_Documents>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public GetDocumentQueryHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Payables_Documents>> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.Payables_Documents
                .Include(w => w.Payables_DocumentsAccounts)
                .Include(w=>w.Payables_DocumentsOperations)
                .Include(w=>w.Payables_DocumentsPayOrders)
                .Include(w=>w.ChequeBookSheet).ThenInclude(w=>w.ChequeBook).ThenInclude(w=>w.BankAccount)
                .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<Payables_Documents>.Success(r);
        }

    }


}
