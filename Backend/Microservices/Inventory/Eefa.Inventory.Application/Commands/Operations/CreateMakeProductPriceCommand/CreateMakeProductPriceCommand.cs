using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Eefa.Common.Data;
using Eefa.Invertory.Infrastructure.Repositories;
using System;
using System.Net.WebSockets;
using System.Collections.Generic;
using Eefa.Inventory.Domain.Common;
using Eefa.Common.Data.Query;
using Eefa.Common.Exceptions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace Eefa.Inventory.Application
{
    public class CreateMakeProductPriceCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Domain.Receipt>, ICommand
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class CreateMakeProductPriceCommandHandler : IRequestHandler<CreateMakeProductPriceCommand, ServiceResult>
    {

        private readonly IMapper _mapper;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IReportsQueries _reportsQueries;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<DocumentItem> _documentItemRepository;
        private readonly IRepository<WarehouseHistory> _warehouseHistoryRepository;

        public CreateMakeProductPriceCommandHandler(
              IMapper mapper
            , IProcedureCallService procedureCallService
            , IReportsQueries reportsQueries
            , IInvertoryUnitOfWork context
            , IReceiptRepository receiptRepository
            , IRepository<DocumentItem> documentItemRepository
            , IReceiptCommandsService receiptCommandsService
            ,
IRepository<WarehouseHistory> warehouseHistoryRepository



            )
        {
            _mapper = mapper;
            _procedureCallService = procedureCallService;
            _context = context;
            _documentItemRepository = documentItemRepository;
            _receiptRepository = receiptRepository;
            _reportsQueries = reportsQueries;
            _receiptCommandsService = receiptCommandsService;
            _warehouseHistoryRepository = warehouseHistoryRepository;
        }

        public async Task<ServiceResult> Handle(CreateMakeProductPriceCommand request, CancellationToken cancellationToken)
        {
            DateTime from = Convert.ToDateTime(request.FromDate).ToUniversalTime();
            DateTime to = Convert.ToDateTime(request.ToDate).ToUniversalTime();
            var result = await _reportsQueries.GetMakeProductPriceReport(request.FromDate, request.ToDate,new PaginatedQueryModel());
            if (result ==null)
            {
                throw new ValidationError("هیچ کالایی برای قیمت گذاری وجود ندارد");
            }
            
            List<int> documentItems = new List<int>();
            List<int> HistoryIds = new List<int>();
            foreach (var item in result.Data.FirstOrDefault().MakeProductPriceReport)
            {
                var commodity =await  _context.CommodityPropertyWithThicknessView.Where(a => a.size == item.Size && a.thickness == item.Thickness && (a.DocumentDate >= from && a.DocumentDate <= to)).ToListAsync();
                foreach (var com in commodity)
                {
                    var documentItem = await _documentItemRepository.GetAll().Where(a => a.Id == com.DocumentItemId && a.CommodityId == com.CommodityId).FirstOrDefaultAsync();
                    var History = await _warehouseHistoryRepository.GetAll().Where(a => a.Id ==com.Id ).FirstOrDefaultAsync();
                    if (documentItem != null)
                    {
                        documentItems.Add(documentItem.DocumentHeadId);
                       
                        documentItem.UnitPrice = Convert.ToDouble(item.Total / item.Meterage);
                        documentItem.UnitPriceWithExtraCost = documentItem.UnitPrice;
                        documentItem.ProductionCost = documentItem.UnitPrice * documentItem.Quantity;
                       
                        _documentItemRepository.Update(documentItem);
                        if (History != null)
                        {
                            History.AVGPrice = Convert.ToDouble(documentItem.UnitPriceWithExtraCost);
                            _warehouseHistoryRepository.Update(History);
                        }

                    }
                   

                }
            }
            await _documentItemRepository.SaveChangesAsync();
            foreach (var item in documentItems)
            {
                var receipt = await _receiptRepository.Find(item);
                receipt.TotalProductionCost = await _context.DocumentItems.Where(a => a.DocumentHeadId == item).SumAsync(a => a.ProductionCost);
                receipt.TotalItemPrice = Convert.ToDouble(receipt.TotalProductionCost);
                CodeVoucherGroup NewCodeVoucherGroup = await CreateCodeVoucher(receipt);
                receipt.CodeVoucherGroupId = NewCodeVoucherGroup.Id;
                _receiptRepository.Update(receipt);
            }
            await _receiptRepository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();


        }
        private async Task<CodeVoucherGroup> CreateCodeVoucher(Receipt receipt)
        {

            switch (receipt.DocumentStauseBaseValue)
            {
                case (int)DocumentStateEnam.Leave:
                    receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmountLeave;
                    break;
                case (int)DocumentStateEnam.Direct:
                    receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.invoiceAmount;
                    break;

                case (int)DocumentStateEnam.invoiceAmountLeave:
                    if (receipt.DocumentId == null)
                        receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Leave;
                    break;
                case (int)DocumentStateEnam.invoiceAmount:
                    if (receipt.DocumentId == null)
                        receipt.DocumentStauseBaseValue = (int)DocumentStateEnam.Direct;
                    break;

                default:

                    break;
            }
            CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(receipt);
            return NewCodeVoucherGroup;
        }
    }
}
