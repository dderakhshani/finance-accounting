using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using Eefa.NotificationServices.Common.Enum;
using Eefa.NotificationServices.Dto;
using Eefa.NotificationServices.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class InsertDocumentHeadsCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<InsertDocumentHeadsCommand>, ICommand
    {
      public  WarehouseIOCommodity warehouseIOCommodity { get; set; }

    }

    public class InsertDocumentHeadsCommandHandler : IRequestHandler<InsertDocumentHeadsCommand, ServiceResult<Domain.Receipt>>
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        
        private readonly INotificationClient _notificationClient;
        private readonly IProcedureCallService _procedureCallService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        



        public InsertDocumentHeadsCommandHandler(
             IMapper mapper
            , IInvertoryUnitOfWork context
            , IReceiptRepository receiptRepository
            , INotificationClient notificationClient
            , IProcedureCallService procedureCallService
            , ICurrentUserAccessor currentUserAccessor
            
            )

        {
            _mapper = mapper;
            _context = context;
           
            _procedureCallService= procedureCallService;
            _notificationClient = notificationClient;
            _currentUserAccessor = currentUserAccessor;
           
        }
        public async Task<ServiceResult<Domain.Receipt>> Handle(InsertDocumentHeadsCommand request, CancellationToken cancellationToken)
        {

            await _procedureCallService.InsertDocumentHeads(request.warehouseIOCommodity);


            return ServiceResult<Domain.Receipt>.Success(new Receipt());
        }

      

    }
}