using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Eefa.Inventory.Domain;
using System.Linq;
using Newtonsoft.Json;
using Eefa.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using static Eefa.Inventory.Application.CreateCorrectionRequestCommand;
using Eefa.NotificationServices.Dto;
using Eefa.NotificationServices.Common.Enum;
using Eefa.NotificationServices.Services.Interfaces;
using Eefa.Invertory.Infrastructure.Services.AdminApi;

namespace Eefa.Inventory.Application
{
    public class CreateCorrectionRequestCommand : CommandBase, IRequest<ServiceResult<CorrectionRequest>>, IMapFrom<CreateCorrectionRequestCommand>, ICommand
    {
        [JsonProperty("editType")]
        public int? EditType { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("documentId")]
        public int DocumentId { get; set; }

        [JsonProperty("voucherHeadId")]
        public int VoucherHeadId { get; set; }

        [JsonProperty("invoiceNo")]
        public string InvoiceNo { get; set; }

        [JsonProperty("financialOperationNumber")]
        public string FinancialOperationNumber { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("debitAccountHeadId")]
        public int? DebitAccountHeadId { get; set; } = default!;

        [JsonProperty("debitAccountReferenceId")]
        public int? DebitAccountReferenceId { get; set; } = default!;

        [JsonProperty("debitAccountReferenceGroupId")]
        public int? DebitAccountReferenceGroupId { get; set; } = default!;

        [JsonProperty("creditAccountHeadId")]
        public int? CreditAccountHeadId { get; set; } = default!;

        [JsonProperty("creditAccountReferenceId")]
        public int? CreditAccountReferenceId { get; set; } = default!;

        [JsonProperty("creditAccountReferenceGroupId")]
        public int? CreditAccountReferenceGroupId { get; set; } = default!;

        [JsonProperty("vatDutiesTax")]
        public long VatDutiesTax { get; set; }

        [JsonProperty("extraCost")]
        public long ExtraCost { get; set; }

        [JsonProperty("documentDescription")]
        public string DocumentDescription { get; set; }

        [JsonProperty("extraCostCurrency")]
        public double? ExtraCostCurrency { get; set; } = default!;

        [JsonProperty("invoiceDate")]
        public DateTime? InvoiceDate { get; set; }

        [JsonProperty("extraCostAccountHeadId")]
        public Nullable<int> ExtraCostAccountHeadId { get; set; }

        [JsonProperty("extraCostAccountReferenceGroupId")]
        public Nullable<int> ExtraCostAccountReferenceGroupId { get; set; }

        [JsonProperty("extraCostAccountReferenceId")]
        public Nullable<int> ExtraCostAccountReferenceId { get; set; }

        [JsonProperty("isNegative")]
        public bool? IsNegative { get; set; }

        [JsonProperty("ScaleBill")]
        public string ScaleBill { get; set; } = default!;

        [JsonProperty("isFreightChargePaid")]
        public bool? IsFreightChargePaid { get; set; } = default!;


        [JsonProperty("totalItemPrice")]
        public double TotalItemPrice { get; set; } = default!;

        [JsonProperty("totalProductionCost")]
        public double TotalProductionCost { get; set; } = default!;

        [JsonProperty("receiptDocumentItems")]
        public ICollection<RialsReceiptDocumentItem> ReceiptDocumentItems { get; set; }


        [JsonProperty("attachmentIds")]
        public List<int> AttachmentIds { get; set; } = default!;



        public class RialsReceiptDocumentItem : IMapFrom<ReceiptItemModel>
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("commodityId")]
            public int CommodityId { get; set; } = default!;

            [JsonProperty("documentHeadId")]
            public int DocumentHeadId { get; set; } = default!;

            [JsonProperty("unitPrice")]
            public double UnitPrice { get; set; } = default!;

            [JsonProperty("unitPriceWithExtraCost")]
            public double UnitPriceWithExtraCost { get; set; } = default!;

            [JsonProperty("currencyPrice")]
            public double CurrencyPrice { get; set; } = default!;

            [JsonProperty("currencyRate")]
            public double CurrencyRate { get; set; } = default!;


            [JsonProperty("currencyBaseId")]
            public int CurrencyBaseId { get; set; } = default!;
            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("productionCost")]
            public double ProductionCost { get; set; }

            [JsonProperty("quantity")]
            public double Quantity { get; set; }




        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReceiptQueryModel, CreateCorrectionRequestCommand>()
                .IgnoreAllNonExisting();
            profile.CreateMap<ReceiptItemModel, RialsReceiptDocumentItem>()
                .IgnoreAllNonExisting();

        }
    }

    public class CreateCorrectionRequestCommandHandler : IRequestHandler<CreateCorrectionRequestCommand, ServiceResult<CorrectionRequest>>
    {
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<CorrectionRequest> _repository;
        private readonly IReceiptQueries _receiptQueries;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IMapper _mapper;
        private readonly INotificationClient _notificationClient;
        private readonly IPermissionsQueries _permissionsQueries;
        public CreateCorrectionRequestCommandHandler(
            IReceiptCommandsService receiptCommandsService,
            IRepository<CorrectionRequest> repository,
            IReceiptQueries receiptQueries,
            IMapper mapper,
             IInvertoryUnitOfWork context,
             INotificationClient notificationClient,
             IPermissionsQueries permissionsQueries
            )
        {
            _mapper = mapper;
            _context = context;
            _repository = repository;
            _receiptQueries = receiptQueries;
            _receiptCommandsService = receiptCommandsService;
            _notificationClient = notificationClient;
            _permissionsQueries = permissionsQueries;
        }

        public async Task<ServiceResult<CorrectionRequest>> Handle(CreateCorrectionRequestCommand request, CancellationToken cancellationToken)
        {

            //در هنگام ارسال به صفجه ویرایش سند، فقط یک آیدی فرستاده می شود

            var DocumentHead = await _context.DocumentHeads.Where(a => a.DocumentId == request.DocumentId).FirstOrDefaultAsync();
            
            var result = await _receiptQueries.GetByDocumentId(request.DocumentId);

            if (_context.CorrectionRequest.Where(a => a.DocumentId == request.DocumentId && a.Status == 0 && result.CodeVoucherGroupId == DocumentHead.CodeVoucherGroupId).Any())
            {
                throw new ValidationError("این رسید در لیست درخواست تغییرات بررسی نشده قرارداد و امکان تغییر جدید وجود ندارد");
            }



            var OldData = _mapper.Map<CreateCorrectionRequestCommand>(result);
            var List = _mapper.Map<List<RialsReceiptDocumentItem>>(result.Items);
            OldData.ReceiptDocumentItems = List;

            var CorrectionRequest = new CorrectionRequest()
            {
                DocumentId = DocumentHead.DocumentId,
                Status = default,
                CodeVoucherGroupId = DocumentHead.CodeVoucherGroupId,
                AccessPermissionId = await _context.Permissions.Where(a => a.UniqueName == "مجوز تایید درخواست تغییرات ریالی").Select(a => a.Id).FirstOrDefaultAsync(), 
                PayLoad = Newtonsoft.Json.JsonConvert.SerializeObject(request),
                ApiUrl = "inventory/MechanizedDocumentEditing",
                ViewUrl = "inventory/mechanizedDocumentConfirmEdit",
                YearId = DocumentHead.YearId,

                Description = $"   رسید انبار شماره مالی : {DocumentHead.DocumentId.ToString()} 🔅شماره سند : {result.VoucherNo.ToString()}",

                OldData = Newtonsoft.Json.JsonConvert.SerializeObject(OldData)
            };
            switch (request.EditType)
            {
                case 1:
                    
                    CorrectionRequest.AccessPermissionId = await _context.Permissions.Where(a => a.UniqueName == "مجوز تایید درخواست تغییرات مقداری").Select(a => a.Id).FirstOrDefaultAsync();
                    CorrectionRequest.Description += " 🔅 نوع اصلاح :اصلاح تعداد کالا در انبار ";
                    break;
                case 2:
                    CorrectionRequest.AccessPermissionId = await _context.Permissions.Where(a => a.UniqueName == "مجوز تایید درخواست تغییرات شرح سند").Select(a => a.Id).FirstOrDefaultAsync(); 
                    CorrectionRequest.Description += " 🔅 نوع اصلاح :صلاح شرح سند ";

                    break;
                default:

                    break;
            }

            var entity = _repository.Insert(CorrectionRequest);

            await _repository.SaveChangesAsync(cancellationToken);

            await SendNotification(CorrectionRequest);


            //---------------------Insert Attachments----------------------------------
            await _receiptCommandsService.ModifyDocumentAttachments(request.AttachmentIds, Convert.ToInt32(DocumentHead.DocumentId));

            return ServiceResult<CorrectionRequest>.Success(_mapper.Map<CorrectionRequest>(CorrectionRequest));
        }

        private async Task SendNotification(CorrectionRequest CorrectionRequest)
        {
            var users = await _permissionsQueries.GetAllUserByPermision(CorrectionRequest.AccessPermissionId.Value).ToListAsync();
            var message = new NotificationDto
            {
                MessageTitle = "تغییر رسید انبار",
                MessageContent = CorrectionRequest.Description,
                MessageType = 1,
                Payload = CorrectionRequest.PayLoad,
                SendForAllUser = false,
                Status = MessageStatus.Sent,
                OwnerRoleId = 1,
                Listener = "notifyInventoryReciept"
            };
            foreach (var user in users.Distinct())
            {
                message.ReceiverUserId = user.Id;
                await _notificationClient.Send(message);
            }
        }
    }
}
