using System.Collections.Generic;
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
using Eefa.Invertory.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Eefa.Inventory.Domain.Common.ConstantValues;

namespace Eefa.Inventory.Application
{
    public class ArchiveReceiptCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }

    }

    public class ArchiveReceiptCommandHandler : IRequestHandler<ArchiveReceiptCommand, ServiceResult>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<Assets> _assetsRepository;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<VouchersDetail> _vouchersDetailRepository;
        private readonly IRepository<CodeVoucherGroup> _codeVoucherGroupRepository;
        private readonly IWarehouseLayoutCommandsService _warehouseLayoutCommandsService;
        private readonly IRepository<WarehouseRequestExit> _warehouseRequestExitRepository;


        public ArchiveReceiptCommandHandler(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IReceiptRepository receiptRepository,
            IRepository<Assets> assetsRepository,
            IProcedureCallService procedureCallService,
            IReceiptCommandsService receiptCommandsService,
            IRepository<VouchersDetail> vouchersDetailRepository,
            IRepository<CodeVoucherGroup> codeVoucherGroupRepository,
            IWarehouseLayoutCommandsService warehouseLayoutCommandsService,
            IRepository<WarehouseRequestExit> warehouseRequestExitRepository
            )
        {
            _mapper = mapper;
            _context = context;
            _receiptRepository = receiptRepository;
            _assetsRepository = assetsRepository;
            _receiptCommandsService = receiptCommandsService;
            _procedureCallService = procedureCallService;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
            _warehouseLayoutCommandsService = warehouseLayoutCommandsService;
            _warehouseRequestExitRepository = warehouseRequestExitRepository;
            _vouchersDetailRepository = vouchersDetailRepository;

        }
        CodeVoucherGroup codeVoucherGroup;
        public async Task<ServiceResult> Handle(ArchiveReceiptCommand request, CancellationToken cancellationToken)
        {
            var entity = await _receiptRepository.Find(request.Id);
            if (entity.ExtraCost > 0)
            {
                throw new ValidationError("رسیدهایی که دارای کرایه حمل هستند قابلیت بایگانی ندارند");
            }

            var entityLink = await _context.DocumentHeads.Where(a => a.ParentId == entity.Id).FirstOrDefaultAsync();
            codeVoucherGroup = await _context.CodeVoucherGroups.Where(a => a.Id == entity.CodeVoucherGroupId).FirstOrDefaultAsync();

            //جایی که رسید محصول یا انتقال داشته باشیم خروجی های آن هم پیدا شود و به انبار وارد شود.
            if (entityLink != null)
            {
                await WhenTwoReceiptArchive(entity, entityLink);
                await vouchersDetailDelete(entity);
                await vouchersDetailDelete(entityLink);

            }
            else
            {
                await WhenOneReceiptArchive(entity);
            }

            var assets = await _assetsRepository.GetAll().Where(a => a.DocumentHeadId == entity.Id).ToListAsync();
            assets.ForEach(a =>
            {
                a.IsDeleted = true;
                _assetsRepository.Update(a);
            });
            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }

        private async Task WhenOneReceiptArchive(Receipt entity)
        {
            switch (entity.DocumentStauseBaseValue)
            {
                //------------------------------------------------------------------------------------------
                case (int)DocumentStateEnam.Temp:
                    entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                    break;
                case (int)DocumentStateEnam.Direct:
                    entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                    await resetQuantity(entity, (int)(WarehouseHistoryMode.Exit));//باید از موجودی انبار کم شود
                    break;

                case (int)DocumentStateEnam.invoiceAmount:
                    entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                    await resetQuantity(entity, (int)(WarehouseHistoryMode.Exit));
                    break;
                //-------------------------------------------------------------------------------------------
                case (int)DocumentStateEnam.Leave:
                    entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                    await resetQuantity(entity, (int)(WarehouseHistoryMode.Enter));//باید به موجودی انبار اضافه شود
                    break;
                case (int)DocumentStateEnam.invoiceAmountLeave:
                    entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                    await resetQuantity(entity, (int)(WarehouseHistoryMode.Enter));
                    break;
                //-----------------------------------------------------------------------------------------

                case (int)DocumentStateEnam.registrationAccountingLeave:
                    entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                    await resetQuantity(entity, (int)(WarehouseHistoryMode.Enter));//باید به موجودی انبار اضافه شود
                    await vouchersDetailDelete(entity);
                    break;
                case (int)DocumentStateEnam.registrationAccounting:
                    entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
                    await resetQuantity(entity, (int)(WarehouseHistoryMode.Exit));
                    await vouchersDetailDelete(entity);
                    break;
                //-----------------------------------------------------------------------------------------
                case (int)DocumentStateEnam.requestBuy:
                    entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveBuy;
                    break;
                case (int)DocumentStateEnam.requestReceive:
                    entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveRequest;
                    break;

                default:
                    // code block
                    break;
            }
            CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(entity);
            entity.CodeVoucherGroupId = NewCodeVoucherGroup.Id;

            _receiptRepository.Update(entity);
        }

        private async Task WhenTwoReceiptArchive(Receipt entity, Receipt entityLink)
        {
            entityLink.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
            entity.DocumentStauseBaseValue = (int)DocumentStateEnam.archiveReceipt;
            await resetQuantity(entity, (int)(WarehouseHistoryMode.Exit));

            //اگر انتقال داشتیم باید از انباری که خارج شده است وارد شود.
            if (codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.ChangeMaterialWarhouse)
            {
                await resetQuantity(entityLink, (int)(WarehouseHistoryMode.Enter));
            }

            CodeVoucherGroup NewCodeVoucherGroup = await _receiptCommandsService.GetNewCodeVoucherGroup(entity);
            entity.CodeVoucherGroupId = NewCodeVoucherGroup.Id;

            CodeVoucherGroup NewCodeVoucherGroupLink = await _receiptCommandsService.GetNewCodeVoucherGroup(entityLink);
            entity.CodeVoucherGroupId = NewCodeVoucherGroupLink.Id;

            _receiptRepository.Update(entity);
            _receiptRepository.Update(entityLink);
        }

        //--------------اگر در انبار جایگذاری شده باشد ، به حالت قبل برگردانده شود
        private async Task resetQuantity(Receipt entity, int historyMode)
        {
            var documentItems = await _context.DocumentItems.Where(a => a.DocumentHeadId == entity.Id && !a.IsDeleted).ToListAsync();
            if (entity.RequestNo != null)
            {
                await WarehouseRequestExitUpdate(entity.RequestNo, entity.Id);
            }

            foreach (var documentItem in documentItems)
            {
                //این کالا قبلا در انبار جایگذاری شده است برای همین نیازی به پیدا کردن اولین لوکشن نیست
                int WarehouseLayoutId = await _context.WarehouseHistories.Where(a => a.DocumentItemId == documentItem.Id).Select(a => a.WarehouseLayoutId).FirstOrDefaultAsync();
                if (WarehouseLayoutId == 0) continue;
                await UpdateWarehouseLayout(entity.WarehouseId, WarehouseLayoutId, documentItem.CommodityId, documentItem.Quantity, historyMode, documentItem.Id);

                //اگر فرمول ساخت داشتیم
                if (codeVoucherGroup.UniqueName == ConstantValues.CodeVoucherGroupValues.ProductReceiptWarehouse)
                {
                    var bomItems = await _context.DocumentItemsBom.Where(a => a.DocumentItemsId == documentItem.Id && !a.IsDeleted).ToListAsync();
                    if (bomItems != null)
                    {
                        foreach (var Item in bomItems)
                        {
                            await UpdateWarehouseLayout(entity.WarehouseId, WarehouseLayoutId, Item.CommodityId, Item.Quantity, (int)(WarehouseHistoryMode.Enter), documentItem.Id);
                        };

                    }
                }
                await _procedureCallService.CalculateRemainQuantityRequest(entity.RequestNo, documentItem.CommodityId);
                await _procedureCallService.GetAndUpdatePriceBuy(documentItem.CommodityId, entity.WarehouseId, null);

            }
        }
        /// <summary>
        /// فقط برای انتقال و خروجی های انبار که تنها از یک رسید است مناسب است.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task vouchersDetailDelete(Receipt entity)
        {


            var vouchersDetails = await _vouchersDetailRepository.GetAll().Where(a => a.DocumentId == entity.DocumentId && a.VoucherId == entity.VoucherHeadId).ToListAsync();
            if (vouchersDetails != null)
            {
                vouchersDetails.ForEach(a =>
                {
                    a.IsDeleted = true;
                    _vouchersDetailRepository.Update(a);
                });

                await _vouchersDetailRepository.SaveChangesAsync();
            }
            entity.VoucherHeadId = null;


        }
        private async Task WarehouseRequestExitUpdate(string RequestId, int DocumentHeadId)
        {

            var _requestExit = await _warehouseRequestExitRepository.GetAll().Where(a => a.RequestId.ToString() == RequestId.ToString() && a.DocumentHeadId == DocumentHeadId).ToListAsync();
            _requestExit.ForEach(a =>
            {
                a.Quantity = 0;
                a.IsDeleted = true;
                _warehouseRequestExitRepository.Update(a);

            });
            await _warehouseRequestExitRepository.SaveChangesAsync();



        }

        //======================================================================
        //----------------------------------------------------------------------
        //-----------------افزایش و کاهش ظرفیت فعلی در مکان--------------------
        private async Task UpdateWarehouseLayout(int WarehouseId, int WarehouseLayoutId, int CommodityId, double Quantity, int historyMode, int receiptItemsId)
        {
            var WarehouseLayoutQuantity = await _context.WarehouseLayoutQuantities.Where(a => a.CommodityId == CommodityId && a.WarehouseLayoutId == WarehouseLayoutId && !a.IsDeleted).FirstOrDefaultAsync();
            var stock = await _context.WarehouseStocks.Where(a => a.CommodityId == CommodityId && a.WarehousId == WarehouseId && !a.IsDeleted).FirstOrDefaultAsync();

            await _warehouseLayoutCommandsService.DeleteWarehouseHistory(receiptItemsId, CommodityId);
            await _warehouseLayoutCommandsService.InsertLayoutQuantity(CommodityId, Quantity, historyMode, WarehouseLayoutQuantity, WarehouseLayoutId);
            await _warehouseLayoutCommandsService.InsertStock(WarehouseId, CommodityId, Quantity, historyMode, stock);

        }

    }
}
