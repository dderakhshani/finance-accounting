using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class UpdateStartDocumentItemsCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<UpdateStartDocumentItemsCommand>, ICommand
    {

        public int CommodityId { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;
        public double UnitPrice { get; set; } = default!;
        public double Quantity { get; set; } = default!;

        public int YearId { get; set; } = default!;
    }

    public class UpdateStartDocumentItemsCommandHandler : IRequestHandler<UpdateStartDocumentItemsCommand, ServiceResult<Domain.Receipt>>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<WarehouseHistory> _warehouseHistoryRepository;

        private readonly IProcedureCallService _procedureCallService;




        public UpdateStartDocumentItemsCommandHandler(
              IReceiptRepository receiptRepository
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<DocumentItem> documentItemRepository
            , IProcedureCallService procedureCallService
            , IRepository<WarehouseHistory> warehouseHistoryRepository
            , IInvertoryUnitOfWork context
            , IMapper mapper

            )

        {
            _mapper = mapper;
            _context = context;
            _receiptRepository = receiptRepository;
            _currentUserAccessor = currentUserAccessor;
            _procedureCallService = procedureCallService;
            _warehouseHistoryRepository = warehouseHistoryRepository;
            _documentItemRepository = documentItemRepository;


        }


        public async Task<ServiceResult<Domain.Receipt>> Handle(UpdateStartDocumentItemsCommand request, CancellationToken cancellationToken)
        {

            var receipt = await _context.DocumentHeads.Where(a => a.WarehouseId == request.WarehouseId && a.YearId == request.YearId && (a.DocumentStauseBaseValue == (int)DocumentStateEnam.invoiceAmountStart || a.DocumentStauseBaseValue == (int)DocumentStateEnam.registrationAccountingStart)).FirstOrDefaultAsync();
            if (receipt == null)
            {
                throw new ValidationError("سند افتتاحیه این سال مالی برای این انبار ثبت نشده است.");
            }


            var item = await _documentItemRepository.GetAll().Where(a => a.CommodityId == request.CommodityId && a.DocumentHeadId == receipt.Id).FirstOrDefaultAsync();

            if (item != null)
            {
                receipt.TotalItemPrice = receipt.TotalItemPrice - (item.UnitPrice * item.Quantity) + (request.UnitPrice * request.Quantity);
                receipt.TotalProductionCost = receipt.TotalProductionCost - (item.UnitPrice * item.Quantity) + (request.UnitPrice * request.Quantity);
                item.Quantity = request.Quantity;
                item.UnitPrice = request.UnitPrice;
                item.ProductionCost = item.UnitPrice * item.Quantity;
                item.UnitPriceWithExtraCost = item.UnitPrice;
                var warehouseHistory = await _warehouseHistoryRepository.GetAll().Where(a => a.DocumentItemId == item.Id).FirstOrDefaultAsync();
                warehouseHistory.AVGPrice = request.UnitPrice;
                warehouseHistory.Quantity = request.Quantity;
                _receiptRepository.Update(receipt);
                _documentItemRepository.Update(item);

                await _receiptRepository.SaveChangesAsync();


            }

            var spComputeAvgPrice = new spComputeAvgPriceParam() { CommodityId = Convert.ToInt32(request.CommodityId), ReceiptId = receipt.Id, YearId = request.YearId, CaredxRepairId = Guid.NewGuid() };
            var CardexRepair = new spUpdateinventory_CaredxRepairParam() { CaredxRepairId = Guid.NewGuid(), YearId = request.YearId };
            await _procedureCallService.ComputeAvgPrice(spComputeAvgPrice, CardexRepair);

            return ServiceResult<Domain.Receipt>.Success(receipt);

        }




    }
}