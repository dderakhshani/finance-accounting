using AutoMapper;
using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.CommandQuery;

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Common;
using System.Collections.Generic;
using Eefa.Bursary.Application.Queries.FinancialRequest;

namespace Eefa.Bursary.Application.Commands.FinancialRequest.Create
{
    public class CreateFinancialRequestCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateFinancialRequestCommand>, ICommand
    {


        public string RequestNumber { get; set; } = default!;
        public int? RunningWorkflowId { get; set; }
        public int? ParentId { get; set; }
        public int ReferenceId { get; set; } = default!;
        public int AccountHeadId { get; set; } = default!;

        /// <summary>
        /// طرف دوم حساب
        /// </summary>
        public int? SecondReferenceId { get; set; }
        public int? SecondAccountHeadId { get; set; }

        /// <summary>
        /// 1= علل حساب
        /// 2= پیش پرداخت
        /// 3= تسویه
        /// </summary>
        public int PaymentType { get; set; } = default!;

        /// <summary>
        /// 1=پیشفاکتور
        /// 2= فاکتور
        /// 3=صورت وضعیت
        /// 4=قرارداد
        /// </summary>
        public int FactorType { get; set; } = default!;
        public long Amount { get; set; } = default!;
        public long TotalCost { get; set; } = default!;

        /// <summary>
        /// مقدار کسر شده از پرداخت
        /// </summary>
        public long DeductAmount { get; set; } = default!;
        public string? DeductionReason { get; set; }
        public int? CurrencyId { get; set; }

        /// <summary>
        /// 0=در صورتی که عملیات دریافت یا ضمانت باشد نیازی به نحوه پرداخت نیست
        /// </summary>
        public int? FeeType { get; set; }
        public string? Description { get; set; }
        public DateTime IssueDate { get; set; } = default!;

        /// <summary>
        /// 1=پرداخت
        /// 2=دریافت
        /// 3=ضمانت
        /// 4=جابجایی
        /// 5=تسهیلات
        /// 6=ابطالی
        /// </summary>
        public short? Type { get; set; }
        public int? SubjectBaseId { get; set; }
        public string? ExtraFieldJson { get; set; }
        public string? MissedDocumentJson { get; set; }

        /// <summary>
        /// آخرین وضعیت سند
        /// </summary>
        public int Status { get; set; } = default!;
        public int UserId { get; set; } = default!;
        public string? WorkflowState { get; set; }

        /// <summary>
        /// پرداخت فوری
        /// </summary>
        public bool IsEmergent { get; set; } = default!;

        /// <summary>
        /// پرداخت تجمیعی
        /// </summary>
        public bool IsAccumulativePayment { get; set; } = default!;

        /// <summary>
        /// آیا در برنامه تیک خورده یا نه، تیک خورده ها پرداخت می شوند
        /// </summary>
        public short AccumulativeSelectStatus { get; set; } = default!;
        public short AuditorConfirmStatus { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateFinancialRequestCommand, Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>().IgnoreAllNonExisting();
        }

    }

    public class CreateFinancialRequestCommandHandler : IRequestHandler<CreateFinancialRequestCommand, ServiceResult>
    {
        private readonly IMapper _autoMaper;
        private readonly IFinancialRequestRepository _financialRequestRepository;

        public CreateFinancialRequestCommandHandler(IMapper autoMaper, IFinancialRequestRepository financialRequestRepository)
        {
            _autoMaper = autoMaper;
            _financialRequestRepository = financialRequestRepository;
        }

        public async Task<ServiceResult> Handle(CreateFinancialRequestCommand request, CancellationToken cancellationToken)
        {


            //var newDatas = new List<FinancialRequestModel>();
            //var newData1 = new  FinancialRequestModel();
            //var newData2 = new  FinancialRequestModel();
            //var newData3 = new  FinancialRequestModel();
            //var newData4 = new  FinancialRequestModel();

            //newDatas.Add(newData1);
            //newDatas.Add(newData2);
            //newDatas.Add(newData3);
            //newDatas.Add(newData4);



            //foreach(var item in newDatas)
            //{
            //    var test = new Domain.Aggregates.FinancialRequestAggregate.FinancialRequest { 
                
            //    Amount = item.Amount,
            //    CodeVoucherGroupId = item.CodeVoucherGroupId,
                
            //    };

            //    _financialRequestRepository.add
             
        //    }




            var financialRequest = _autoMaper.Map<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>(request);

            var entity = _financialRequestRepository.Insert(financialRequest);

            await _financialRequestRepository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }

    }


}
