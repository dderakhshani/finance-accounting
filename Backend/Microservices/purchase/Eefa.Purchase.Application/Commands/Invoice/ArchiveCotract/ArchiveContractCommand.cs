using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Purchase.Domain.Common;
using Eefa.Purchase.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Purchase.Application.Commands.Invoice.ArchiveCotract
{
    public class ArchiveContractCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class ArchiveContractCommandHandler : IRequestHandler<ArchiveContractCommand, ServiceResult>
    {
        private readonly IRepository<Domain.Aggregates.InvoiceAggregate.Invoice> _InvoiceRepository;
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly IMapper _mapper;

        public ArchiveContractCommandHandler(IRepository<Domain.Aggregates.InvoiceAggregate.Invoice> InvoiceRepository,
            IRepository<CodeVoucherGroup> codeVoucherGroupRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _InvoiceRepository = InvoiceRepository;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
        }

        public async Task<ServiceResult> Handle(ArchiveContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _InvoiceRepository.Find(request.Id);
            var codeVoucherGroup = (await _codeVoucherGroupRepository
               .GetAll(x => x.ConditionExpression(t => t.UniqueName == ConstantValues.CodeVoucherGroup.ArchiveContract))
               .FirstOrDefaultAsync());
            entity.CodeVoucherGroupId=codeVoucherGroup.Id;
            
            if (await Domain.Aggregates.InvoiceAggregate.Invoice.UpdateInvoiceAsync(entity, _InvoiceRepository)> 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
    }
}
