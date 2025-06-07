using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class ArchiveDocumentHeadsByDocumentDateCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int WarehouseId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int DocumentStatuesBaseValue { get; set; }

    }

    public class ArchiveDocumentHeadsByDocumentDateCommandHandler : IRequestHandler<ArchiveDocumentHeadsByDocumentDateCommand, ServiceResult>
    {
        private readonly IReceiptCommandsService _Repository;
        
        private readonly IMapper _mapper;

        public ArchiveDocumentHeadsByDocumentDateCommandHandler(
            IReceiptCommandsService Repository,
            IMapper mapper)
        {
            _mapper = mapper;
            _Repository = Repository;
           
        }
        public async Task<ServiceResult> Handle(ArchiveDocumentHeadsByDocumentDateCommand request, CancellationToken cancellationToken)
        {

           await _Repository.ArchiveDocumentHeadsByDocumentDate(request.FromDate,request.ToDate,request.WarehouseId,request.DocumentStatuesBaseValue);
            return ServiceResult.Success();
           
        }

       
    }
}
