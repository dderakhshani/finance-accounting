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
    public class CreateStartDocumentCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<CreateStartDocumentCommand>, ICommand
    {
        
        public int CodeVoucherGroupId { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;
        public int CreditAccountReferenceId { get; set; } = default!;
        public int CreditAccountReferenceGroupId { get; set; } = default!;
        public string DocumentDescription { get; set; }
        public DateTime DocumentDate { get; set; } = default!;
        public string Tags { get; set; }
        public int? ViewId { get; set; } = default!;
        public int YearId { get; set; } = default!;
        public int DocumentStauseBaseValue { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateStartDocumentCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }

    }
   
    public class CreateStartDocumentCommandHandler : IRequestHandler<CreateStartDocumentCommand, ServiceResult<Domain.Receipt>>
    {
        private readonly IReceiptRepository _receiptRepository;
        
        private readonly IRepository<BaseValue> _baseValueRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IInvertoryUnitOfWork _contex;
        private readonly IReceiptCommandsService _reciptCommandsService;
        private readonly IProcedureCallService _procedureCallService;

        public CreateStartDocumentCommandHandler(
              IReceiptRepository receiptRepository
            , ICurrentUserAccessor currentUserAccessor
            , IRepository<BaseValue> baseValueRepository
            , IInvertoryUnitOfWork contex
            , IMapper mapper
            , IReceiptCommandsService reciptCommandsService
            , IProcedureCallService procedureCallService

            )

        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _baseValueRepository = baseValueRepository;
            _receiptRepository = receiptRepository;
            _contex = contex;
            _reciptCommandsService = reciptCommandsService;
            _procedureCallService = procedureCallService;
        }


        public async Task<ServiceResult<Domain.Receipt>> Handle(CreateStartDocumentCommand request, CancellationToken cancellationToken)
        {
            
                var doplicate = await _contex.DocumentHeads.Where(a => a.WarehouseId == request.WarehouseId && a.YearId == _currentUserAccessor.GetYearId() && a.CodeVoucherGroupId == request.CodeVoucherGroupId).CountAsync();
                if (doplicate > 0)
                {
                    throw new ValidationError("سند افتتاحیه این سال مالی در این گروه قبلا ثبت شده است.");
                }

                var currency = await _baseValueRepository.GetAll().Where(a => a.UniqueName == ConstantValues.ConstBaseValue.currencyIRR).Select(a => a.Id).FirstOrDefaultAsync();

                if (currency == null)
                {
                    throw new ValidationError("واحد ارز ریالی تعریف نشده است");
                }
                var warehouse = await _contex.Warehouses.Where(a => a.Id == request.WarehouseId).FirstOrDefaultAsync();
                if (warehouse == null)
                {
                    throw new ValidationError("کد انبار اشتباه است");
                }
                var CodeVoucher = await _contex.CodeVoucherGroups.Where(a => a.Id == request.CodeVoucherGroupId).FirstOrDefaultAsync();

                if (CodeVoucher == null)
                {
                    throw new ValidationError("کد گروه سند وجود ندارد");
                }


                var receipt = _mapper.Map<Domain.Receipt>(request);
                receipt.YearId = request.YearId;
                receipt.TotalWeight = default;
                receipt.TotalQuantity = default;
                receipt.DocumentDiscount = default;
                receipt.DiscountPercent = default;
                receipt.DocumentStateBaseId = ConstantValues.ConstBaseValue.NotChecked;
                receipt.PaymentTypeBaseId = 1;

                receipt.CommandDescription = "Command:CreateStartDocumentCommand -codeVoucherGroup.id=" + request.CodeVoucherGroupId.ToString();

                var year = await _contex.Years.Where(a => a.Id==request.YearId).FirstOrDefaultAsync();
                receipt.ExpireDate = year.LastDate;
                receipt.DocumentStauseBaseValue = request.DocumentStauseBaseValue;
                receipt.DocumentDate = request.DocumentDate;


                await _reciptCommandsService.SerialFormula(receipt, CodeVoucher.Code, cancellationToken);

                int lastNo = await _reciptCommandsService.lastDocumentNo(receipt, cancellationToken);

                receipt.DocumentNo = lastNo + 1;

                receipt = _reciptCommandsService.ConvertTagArray(request.Tags, receipt);

                //-----------------انبار بدهکار---------------------------
                
                receipt.DebitAccountHeadId = warehouse.AccountHeadId;

                //------------------تامین کننده بستانکار--------------------
                receipt.CreditAccountReferenceId = warehouse.AccountReferenceId;
                receipt.CreditAccountReferenceGroupId = warehouse.AccountRererenceGroupId;
                receipt.CreditAccountHeadId = warehouse.AccountHeadId;

                //await InsertDocumentItems(request, receipt, currency);

                _receiptRepository.Insert(receipt);
                if (await _receiptRepository.SaveChangesAsync() > 0)
                {
                    receipt.DocumentId = await _reciptCommandsService.InsertAndUpdateDocument(receipt);
                    _receiptRepository.Update(receipt);
                    await _receiptRepository.SaveChangesAsync();
                }
                else
                {
                    return ServiceResult<Domain.Receipt>.Failed();
                }
            
            return ServiceResult<Domain.Receipt>.Failed();

        }

        private  async Task<int> InsertDocumentItems(CreateStartDocumentCommand request, Domain.Receipt receipt, int currency)
        {
            var stock =await (from q in _contex.WarehouseLayoutQuantities
                         join l in _contex.WarehouseLayouts on q.WarehouseLayoutId equals l.Id
                         join c in _contex.Commodities on q.CommodityId equals c.Id
                         where l.WarehouseId == receipt.WarehouseId
                         select new
                         {
                             CommodityId= q.CommodityId,
                             WarehouseLayoutId = q.WarehouseLayoutId,
                             Quantity=q.Quantity,
                             MeasureId=c.MeasureId
                         }
                         ).ToListAsync();

            foreach (var items in stock)
            {
                var documentItem = new DocumentItem();
                
                documentItem.CurrencyBaseId = currency;
                documentItem.CurrencyPrice = 1;
                documentItem.DocumentMeasureId = Convert.ToInt32(items.MeasureId);
                documentItem.MainMeasureId = Convert.ToInt32(items.MeasureId);
                documentItem.Quantity = items.Quantity;
                documentItem.RemainQuantity = items.Quantity;
                documentItem.CommodityId = items.CommodityId;
                documentItem.ModifiedAt = DateTime.UtcNow;
                documentItem.CreatedAt = DateTime.UtcNow;
                documentItem.CreatedById = _currentUserAccessor.GetId();
                documentItem.IsDeleted = false;
                documentItem.OwnerRoleId = _currentUserAccessor.GetRoleId();
                //await _reciptCommandsService.GetPriceBuyItems(items.CommodityId, documentItem.BomValueHeaderId> 0?documentItem.Id:null, items.Quantity, documentItem);
                receipt.AddItem(documentItem);


            }
            return 1;
        }
       
       

    }
}