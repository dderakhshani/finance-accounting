//using Eefa.Common.CommandQuery;
 
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Eefa.Bursary.Application.Queries.DocumentHead;
//using Eefa.Bursary.Application.Queries.FinancialRequest;
 
//using AutoMapper;
//using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
//using System.Threading;
//using Eefa.Common;

//namespace Eefa.Bursary.Application.Commands.FinancialRequest.Update
//{
//   public class UpdateFinancialRequestCommand : CommandBase, IRequest<ServiceResult<FinancialRequestModel>>, IMapFrom<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>, ICommand
//    {
//        public int Id { get; set; }
//        public string RequestNumber { get; set; } = default!;
//        public int? RunningWorkflowId { get; set; }
//        public int? ParentId { get; set; }
//        public int ReferenceId { get; set; } = default!;
//        public int AccountHeadId { get; set; } = default!;

//        /// <summary>
//        /// طرف دوم حساب
//        /// </summary>
//        public int? SecondReferenceId { get; set; }
//        public int? SecondAccountHeadId { get; set; }

//        /// <summary>
//        /// 1= علل حساب
//        /// 2= پیش پرداخت
//        /// 3= تسویه
//        /// </summary>
//        public int PaymentType { get; set; } = default!;

//        /// <summary>
//        /// 1=پیشفاکتور
//        /// 2= فاکتور
//        /// 3=صورت وضعیت
//        /// 4=قرارداد
//        /// </summary>
//        public int FactorType { get; set; } = default!;
//        public long Amount { get; set; } = default!;
//        public long TotalCost { get; set; } = default!;

//        /// <summary>
//        /// مقدار کسر شده از پرداخت
//        /// </summary>
//        public long DeductAmount { get; set; } = default!;
//        public string? DeductionReason { get; set; }
//        public int? CurrencyId { get; set; }

//        /// <summary>
//        /// 0=در صورتی که عملیات دریافت یا ضمانت باشد نیازی به نحوه پرداخت نیست
//        /// </summary>
//        public int? FeeType { get; set; }
//        public string? Description { get; set; }
//        public DateTime IssueDate { get; set; } = default!;

//        /// <summary>
//        /// 1=پرداخت
//        /// 2=دریافت
//        /// 3=ضمانت
//        /// 4=جابجایی
//        /// 5=تسهیلات
//        /// 6=ابطالی
//        /// </summary>
//        public short? Type { get; set; }
//        public int? SubjectBaseId { get; set; }
//        public string? ExtraFieldJson { get; set; }
//        public string? MissedDocumentJson { get; set; }

//        /// <summary>
//        /// آخرین وضعیت سند
//        /// </summary>
//        public int Status { get; set; } = default!;
//        public int UserId { get; set; } = default!;
//        public string? WorkflowState { get; set; }

//        /// <summary>
//        /// پرداخت فوری
//        /// </summary>
//        public bool IsEmergent { get; set; } = default!;

//        /// <summary>
//        /// پرداخت تجمیعی
//        /// </summary>
//        public bool IsAccumulativePayment { get; set; } = default!;

//        /// <summary>
//        /// آیا در برنامه تیک خورده یا نه، تیک خورده ها پرداخت می شوند
//        /// </summary>
//        public short AccumulativeSelectStatus { get; set; } = default!;
//        public short AuditorConfirmStatus { get; set; } = default!;
//        public List<DocumentHeadModel> DocumentHeads { get; set; } = default!;



//        public void Mapping(Profile profile)
//        {
//            profile.CreateMap<UpdateFinancialRequestCommand, Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>()
//                .IgnoreAllNonExisting();
//        }

//    }

//   public class UpdateFinancialRequestCommandHandler : IRequestHandler<UpdateFinancialRequestCommand, ServiceResult<FinancialRequestModel>>
//   {
//       private readonly IFinancialRequestRepository _financialRequestRepository;
//       private readonly IFinancialRequestDocumentHeadRepository _financialRequestDocumentHeadRepository;

//       private readonly IMapper _mapper;

//       public UpdateFinancialRequestCommandHandler(IFinancialRequestRepository financialRequestRepository,
//           IFinancialRequestDocumentHeadRepository financialRequestDocumentHeadRepository,
//           IMapper mapper)
//       {
//           _mapper = mapper;
//           _financialRequestRepository = financialRequestRepository;
//           _financialRequestDocumentHeadRepository = financialRequestDocumentHeadRepository;
//       }

//       public async Task<ServiceResult<FinancialRequestModel>> Handle(UpdateFinancialRequestCommand request, CancellationToken cancellationToken)
//       {
//           var entity = await _financialRequestRepository.Find(request.Id);

//           var documentHeadsId = request.DocumentHeads.Select(x => x.Id).ToList();

            

//           _mapper.Map<UpdateFinancialRequestCommand, Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>(request, entity);

//           _financialRequestRepository.Update(entity);
//           await _financialRequestRepository.SaveChangesAsync(cancellationToken);

//           var model = _mapper.Map<FinancialRequestModel>(entity);
//           return ServiceResult<FinancialRequestModel>.Success(model);
//       }
//   }

//}
