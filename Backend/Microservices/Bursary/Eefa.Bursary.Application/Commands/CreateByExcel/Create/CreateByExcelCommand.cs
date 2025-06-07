using AutoMapper;
using Eefa.Bursary.Application.Commands.CustomerReceipt.Create;
using Eefa.Bursary.Application.Commands.CustomerReceipt.Update;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Library.Interfaces;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Commands.CreateByExcel.Create
{
    public class CreateByExcelCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>, IMapFrom<CreateByExcelCommand>, Common.CommandQuery.ICommand
    {
        public string? FileAttachmentAddress { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateByExcelCommand, Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>()
                .IgnoreAllNonExisting();
        }


        public class CreateByExcelCommandHandler : IRequestHandler<CreateByExcelCommand, ServiceResult<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>>
        {
            private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRepository;
            private readonly IMediator _mediator;
            private readonly IRepository<AccountReferencesGroup> _accountReferenceGroupRepository;
            private readonly IRepository<AccountReference> _accountReferenceRepository;
            private readonly IRepository<FinancialRequestDetail> _detailRepository;

            private readonly IConfigurationAccessor _configurationAccessor;
            private readonly IMapper _mapper;


            public CreateByExcelCommandHandler(IMapper mapper, IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRepository, IRepository<FinancialRequestDetail> detailRepository, IMediator mediator, IConfigurationAccessor configurationAccessor, IRepository<AccountReference> accountReferenceRepository)
            {
                _mapper = mapper;
                _financialRepository = financialRepository;
                _detailRepository = detailRepository;
                _mediator = mediator;
                _configurationAccessor = configurationAccessor;
                _accountReferenceRepository = accountReferenceRepository;
            }




            public async Task<ServiceResult<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>> Handle(CreateByExcelCommand request, CancellationToken cancellationToken)
            {
                string updateFile = @"D:\Eefa\Update.txt";
                string createFile = @"D:\Eefa\Create.txt";
                var financialRequests = _financialRepository.GetAll().Include(x=>x.FinancialRequestDetails).ThenInclude(x=>x.CreditAccountReference).Where(x => x.YearId == 3).ToList();

                if (!string.IsNullOrEmpty(request.FileAttachmentAddress))
                {

                    //var sourcePath = Path.Combine(_configurationAccessor.GetIoPaths().Root,
                    //    request.FileAttachmentAddress.Remove(0, 7));
                    //sourcePath = sourcePath.Replace('/', '\\');
                    var filePath = Directory.GetCurrentDirectory() + "\\" + request.FileAttachmentAddress;
                    using (var excelUtility = new ExcelUtility())
                    {
                        var data = excelUtility.ReadData(filePath);

                        int counter = 1;

                        for (var i = 2; i < data.RowsCount(); i++)
                        {
                          
                            if (string.IsNullOrEmpty(data[i, 1].ToString())) continue;



                              

                            var tadbirId =Utility.ConvertNumberToEnglish(data[i, 1]?.ToString() ?? "");
                            var receiptDate = data[i, 2]?.ToString().Replace("/", "-");
                            var codehesabDaryaftConande = Utility.ConvertNumberToEnglish(data[i, 3]?.ToString());
                            var titlehesabDaryaftConande = data[i, 4]?.ToString();
                            var customerTypeCode = Utility.ConvertNumberToEnglish(data[i, 9]?.ToString());
                            var customerTypeTitle = data[i, 10]?.ToString();
                            var codehesabPardakhtConande = Utility.ConvertNumberToEnglish(data[i, 11]?.ToString());
                            var titleHesabPardakhtConande = data[i, 12]?.ToString();
                            var totalAmount = Utility.ConvertNumberToEnglish(data[i, 15]?.ToString());
                            var amount = Utility.ConvertNumberToEnglish(data[i, 16]?.ToString());
                            var description = data[i, 18]?.ToString();


                             var financialRequest = financialRequests.Where(x => x.DocumentNo == int.Parse(tadbirId)).SingleOrDefault();

                            if ( 1>2)
                            {
                                UpdateCustomerReceiptListCommand update = new UpdateCustomerReceiptListCommand();
                                update.DocumentNo = int.Parse(tadbirId);
                                update.CodeVoucherGroupId = 2259;
                                update.Amount = decimal.Parse(amount);
                                update.TotalAmount = decimal.Parse(totalAmount);
                                update.Description = description;

                              
                                update.DocumentDate = TimeZoneInfo.ConvertTimeToUtc(receiptDate.ToDateTime().AddHours(1).AddMilliseconds(1), TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time"));
                             
                                ReceiptModel updateReceipt = new ReceiptModel();

                                updateReceipt.Amount = decimal.Parse(amount);

                                updateReceipt.CreditAccountHeadId = 1901;
                                updateReceipt.CreditAccountReferenceGroupId = customerTypeCode == "04001" ? 28 : 29;

                                var updateCreditReference = await _accountReferenceRepository.GetAll().Where(x => x.Code == codehesabPardakhtConande).FirstOrDefaultAsync();
                                updateReceipt.CreditAccountReferenceId = updateCreditReference.Id;
                                updateReceipt.DebitAccountHeadId = 1900;
                                updateReceipt.DebitAccountReferenceGroupId = 4;


                                var updateDebitReference = await _accountReferenceRepository.GetAll().Where(x => x.Code == codehesabDaryaftConande).FirstOrDefaultAsync();



                                if (updateDebitReference == null)
                                    Console.WriteLine(codehesabPardakhtConande);

                                updateReceipt.DebitAccountReferenceId = updateDebitReference.Id;

                                updateReceipt.DocumentTypeBaseId = 28509;
                                updateReceipt.Description = description;
                                updateReceipt.IsRial = true;
                                updateReceipt.FinancialReferenceTypeBaseId = 28516;
                                updateReceipt.PaymentCode = tadbirId;


                                update.FinancialRequestDetails = new List<ReceiptModel>();
                                update.FinancialRequestDetails.Add(updateReceipt);


                                try
                                {

                                    long result = Convert.ToInt64(financialRequest.Amount);
                                    if (result != update.Amount || financialRequest.FinancialRequestDetails[0].CreditAccountReferenceId != updateReceipt.CreditAccountReferenceId)
                                    {
                                        using (StreamWriter sw = File.AppendText(updateFile))
                                            sw.WriteLine($"DocNumber {update.DocumentNo} :Befor : {financialRequest.Amount} After: Amount {update.Amount} +++++ Credit Befor :{financialRequest.FinancialRequestDetails[0].CreditAccountReference.Title} After :{titleHesabPardakhtConande} ");
                                        Console.WriteLine($"Update:{update.DocumentNo}", i);
                                   
                                    Console.WriteLine($"Update:{update.DocumentNo}", i);
                                    var res = await _mediator.Send(update, cancellationToken);
                                    }
                                }
                                catch (Exception e)
                                {

                                }
                            }
                            else
                            { 

                            CreateCustomerReceiptCommand create = new CreateCustomerReceiptCommand();
                            ReceiptModel receipt = new ReceiptModel();
                            int number = financialRequests.Max(x => x.DocumentNo) + 1;
                            create.DocumentNo = number;
                            create.CodeVoucherGroupId = 2259;
                            create.Amount = decimal.Parse(amount);
                            create.TotalAmount = decimal.Parse(totalAmount);
                            create.Description = description;
                            create.DocumentDate = TimeZoneInfo.ConvertTimeToUtc(receiptDate.ToDateTime().AddHours(1).AddMilliseconds(1), TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time")) ;
                            receipt.Amount = decimal.Parse(amount);
                            receipt.CreditAccountHeadId = 1901;
                            receipt.CreditAccountReferenceGroupId = customerTypeCode == "04001" ?  28 : 29;

                            var creditReference = await _accountReferenceRepository.GetAll().Where(x => x.Code == codehesabPardakhtConande).FirstOrDefaultAsync();



                            //creditReferences.Contains()Where(x => x.Code == codehesabDaryaftConande).FirstOrDefault();
                            if (creditReference == null)
                                throw new Exception($"کد تفصیل {codehesabPardakhtConande} به اسم {titleHesabPardakhtConande} در سیستم وجود ندارد");

                            receipt.CreditAccountReferenceId = creditReference.Id;
                            receipt.DebitAccountHeadId = 1900;
                            receipt.DebitAccountReferenceGroupId = 4;


                            var debitReference = await _accountReferenceRepository.GetAll().Where(x => x.Code == codehesabDaryaftConande).FirstOrDefaultAsync();

                          

                            if (debitReference == null)
                                Console.WriteLine(codehesabPardakhtConande);

                            receipt.DebitAccountReferenceId = debitReference.Id;

                            receipt.DocumentTypeBaseId = 28509;
                            receipt.PaymentCode = tadbirId;
                            receipt.Description = $"بابت دریافت شماره ردیف  {create.DocumentNo} و به شماره پیگیری {receipt.PaymentCode}";
                            receipt.IsRial = true;
                            receipt.FinancialReferenceTypeBaseId = 28516;
                           


                            create.FinancialRequestDetails = new List<ReceiptModel>();
                            create.FinancialRequestDetails.Add(receipt);
                            Console.WriteLine(counter++);

                            try
                            {
                                    using (StreamWriter sw = File.AppendText(createFile))
                                    {
                                        sw.WriteLine($"DocNumber {create.DocumentNo} : Amount {create.Amount} is Inserted +++++ ");
                                    }

                                    var res = await _mediator.Send(create, cancellationToken);
                            }
                            catch (Exception e)
                            {

                            }
                            }

                        }



                         await _financialRepository.SaveChangesAsync(cancellationToken);


                    }
                    return ServiceResult<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>.Success(new Domain.Aggregates.FinancialRequestAggregate.FinancialRequest());
                }
                return ServiceResult<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>.Success(new Domain.Aggregates.FinancialRequestAggregate.FinancialRequest());
            }
        }
    }
}

    
 
