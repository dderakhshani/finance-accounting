using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Exceptions;

namespace Eefa.Inventory.Application
{
    public class SplitCommodityQuantityCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int DocumentItemId { get; set; }
        public List<Quantities> Quantities { get; set; }

    }
    public class Quantities
    {
        public int Quantity { get; set; }
    }
    public class SplitCommodityQuantityCommandHandler : IRequestHandler<SplitCommodityQuantityCommand, ServiceResult>
    {
        private readonly IRepository<DocumentItem> _receiptRepository;
        private readonly IRepository<WarehouseHistory> _warehouseHistory;
        private readonly IMapper _mapper;

        public SplitCommodityQuantityCommandHandler(
            IRepository<DocumentItem> receiptRepository,
             IRepository<WarehouseHistory> warehouseHistory,
            IMapper mapper)
        {
            _mapper = mapper;
            _receiptRepository = receiptRepository;
            _warehouseHistory = warehouseHistory;

        }

        public async Task<ServiceResult> Handle(SplitCommodityQuantityCommand request, CancellationToken cancellationToken)
        {
            await EditDocumentItems(request);

            return ServiceResult.Success();

        }

        private async Task EditDocumentItems(SplitCommodityQuantityCommand request)
        {
            DocumentItem item = await _receiptRepository.GetAll().Where(a => a.Id == request.DocumentItemId).FirstOrDefaultAsync();
            var history = await _warehouseHistory.GetAll().Where(a => a.DocumentItemId == request.DocumentItemId).FirstOrDefaultAsync();
           
            if (item.Quantity != request.Quantities.Sum(a => a.Quantity))
            {
                throw new ValidationError("مجموع تعداد کالا تقسیم شده با موجودی انبار متفاوت است");
            }

            item.Quantity = request.Quantities[0].Quantity;
            history.Quantity = request.Quantities[0].Quantity;
            int i = 0;
            foreach(var quantity in request.Quantities)
            {

                if (quantity.Quantity > 0 && i != 0)
                {

                    var documentItem = new DocumentItem()
                    {
                        CurrencyBaseId = item.CurrencyBaseId,
                        DocumentHeadId = item.DocumentHeadId,
                        CurrencyPrice = 1,
                        DocumentMeasureId = item.DocumentMeasureId,
                        MainMeasureId = item.MainMeasureId,
                        Quantity = quantity.Quantity,
                        RequestDocumentItemId = item.Id,
                        RemainQuantity = item.Quantity,
                        Description = item.Description,
                        SecondaryQuantity = item.SecondaryQuantity,
                        CommodityId = item.CommodityId,
                        IsWrongMeasure = item.IsWrongMeasure,
                        UnitBasePrice = item.UnitBasePrice,
                        UnitPrice = item.UnitPrice,
                        YearId = item.YearId,
                    };
                    _receiptRepository.Insert(documentItem);
                    await _receiptRepository.SaveChangesAsync();
                    await EditHistory(documentItem.Id, history, quantity.Quantity);
                }
                i++;

            };
           await _warehouseHistory.SaveChangesAsync();
        }
        private async Task EditHistory(int DocumentItemId, WarehouseHistory history, double Quantity)
        {
           
           
            var documentItem = new WarehouseHistory()
            {
                DocumentItemId = DocumentItemId,
                DocumentHeadId = history.DocumentHeadId,
                Quantity = Quantity,
                WarehousesId= history.WarehousesId,
                WarehouseLayoutId= history.WarehouseLayoutId,
                Commodityld= history.Commodityld,
                Mode= history.Mode,
                RequestNo= history.RequestNo,
                

            };
            _warehouseHistory.Insert(documentItem);



        }
    }
}
