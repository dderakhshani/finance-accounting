using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Model;
using Eefa.Accounting.Data.Entities;
using ExcelDataReader;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Utilities;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;

namespace Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.ImportByExcel
{
    public class ImportByExcelMoadianInvoiceHeaderCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<ImportByExcelMoadianInvoiceHeaderCommand>, ICommand
    {
        public IFormFile File { get; set; }

        public bool IsProduction { get; set; } = false;

        public bool GenerateTaxIdWithListNumber { get; set; }

    }


    public class ImportByExcelMoadianInvoiceHeaderCommandHandler : IRequestHandler<ImportByExcelMoadianInvoiceHeaderCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly ICurrentUserAccessor currentUser;
        private readonly IMapper _mapper;
        private int TaxPercentage { get; init; }
        public ImportByExcelMoadianInvoiceHeaderCommandHandler(IMapper mapper, IRepository repository, ICurrentUserAccessor currentUser)
        {
            _mapper = mapper;
            _repository = repository;
            this.currentUser = currentUser;
            this.TaxPercentage = currentUser.GetYearId() == 3 ? 9 : 10;
        }
        public async Task<ServiceResult> Handle(ImportByExcelMoadianInvoiceHeaderCommand request, CancellationToken cancellationToken)
        {
            var entities = await this.ConvertExelToEntities(request);

            //foreach (var item in entities)
            //{
            //    var previousInvoice = await _repository.GetQuery<Data.Entities.MoadianInvoiceHeader>().OrderByDescending(x => x.SubmissionDate).Where(x => x.InvoiceNumber == item.InvoiceNumber && x.SubmissionDate != null && x.ReferenceId != null && x.Status == "SUCCESS").FirstOrDefaultAsync();
            //    if (previousInvoice == null && item.Ins != 1) throw new Exception("Cant Find previous invoice");
            //    item.Irtaxid = previousInvoice?.TaxId;
            //}

            foreach (var entity in entities)
            {
                entity.UId = Guid.NewGuid().ToString();
                entity.YearId = this.currentUser.GetYearId();
                _repository.Insert(entity);
            }
            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success(_mapper.Map<List<MoadianInvoiceHeaderDetailedModel>>(entities));
        }




        public async Task<List<Data.Entities.MoadianInvoiceHeader>> ConvertExelToEntities(ImportByExcelMoadianInvoiceHeaderCommand request)
        {
            var listNumber = await _repository.GetAll<Data.Entities.MoadianInvoiceHeader>().Where(x => x.IsSandbox == !request.IsProduction).MaxAsync(x => (long?)x.ListNumber) ?? 0;
            listNumber++;

            var convertedData = this.MapFromExcel<MoadianInvoiceImportPayloadModel>(request);

            convertedData = convertedData.Where(x => !string.IsNullOrEmpty(x.Inno)).ToList();

            var entities = new List<Data.Entities.MoadianInvoiceHeader>();


            var counter = 1;


            if (request.IsProduction)
            {
                TaxApiService.Instance.Init(
                    MoadianConstants.ProductionProtectorId,
                    new SignatoryConfig(MoadianConstants.PrivateKey, null),
                    new NormalProperties(ClientType.SELF_TSP, "v1"),
                    MoadianConstants.ProductionApiUrl
                    );
            }
            else
            {
                TaxApiService.Instance.Init(
                    MoadianConstants.SandboxProtectorId,
                    new SignatoryConfig(MoadianConstants.PrivateKey, null),
                    new NormalProperties(ClientType.SELF_TSP, "v1"),
                    MoadianConstants.SandboxApiUrl
                    );


            }
            var serverInformation = await TaxApiService.Instance.TaxApis.GetServerInformationAsync();
            var token = await TaxApiService.Instance.TaxApis.RequestTokenAsync();

            foreach (var item in convertedData)
            {
                counter++;
                var errors = new List<string>();

                var header = new Data.Entities.MoadianInvoiceHeader
                {
                    InvoiceNumber = item.Inno,
                    ListNumber = listNumber,
                    Irtaxid = item.Irtaxid,
                };


                if (!entities.Any(x => x.InvoiceNumber == header.InvoiceNumber))
                {
                    header.IsSandbox = !request.IsProduction;

                    if (string.IsNullOrEmpty(header.InvoiceNumber)) header.Errors += ($"شماره صوتحساب در سطر {counter} خالی میباشد.\n");
                    if (item.Inp == default) header.Errors += ($"الگوی صوتحساب در سطر {counter} خالی میباشد.\n");



                    header.InvoiceDate = item.Indatim;
                    header.Indatim = new DateTimeOffset(item.Indatim).ToUnixTimeMilliseconds();
                    header.Indati2m = new DateTimeOffset(item.Indatim).ToUnixTimeMilliseconds();
                    header.Inty = (int)MoadianConstants.IntyTypes.First;
                    header.Inp = item.Inp;
                    header.Ins = item.Ins;
                    header.Tins = MoadianConstants.EefaEconomicCode;

                    // Replace Customer Details
                    var customerInfo = await GetCustomerInfo(item.Acc, item.Ccc);
                    if (customerInfo != null)
                    {
                        header.Bid = customerInfo.LegalType == 3 ? customerInfo.EconomicCode : customerInfo.NationalNumber?.Split("-")?.FirstNonDefault()?.Trim();
                        header.Tinb = customerInfo.LegalType == 1 ? (customerInfo.EconomicCode == customerInfo.NationalNumber ? null : customerInfo.EconomicCode) : (customerInfo.LegalType == 2 ? header.Bid : customerInfo.EconomicCode);
                        header.Tob = customerInfo.LegalType;
                        header.Bpc = customerInfo.PostalCode;

                        header.PersonId = customerInfo.PersonId;
                        header.CustomerId = customerInfo.CustomerId;
                        header.AccountReferenceId = customerInfo.AccountReferenceId;

                        if(customerInfo.LegalType == 5) header.Inty = (int)MoadianConstants.IntyTypes.Second;
                    }
                    else
                    {
                        header.Errors += ($"مشتری با کد تفصیل {item.Acc} و کد مشتری {item.Ccc} در صورتحساب شماره {header.InvoiceNumber} پیدا نشد.\n");
                    }


                    header.Setm = (int)MoadianConstants.SetmTypes.First;

                    // Export properties
                    if (header.Inp == (int)MoadianConstants.InpTypes.Seventh)
                    {
                        header.Scln = item.Scln;
                        header.Scc = item.Scc;

                        //header.Cdcd = item.Cdcd != default ? new DateTimeOffset(item.Cdcd).ToUnixTimeMilliseconds() : null;
                        header.Cdcd = null;
                        header.Cdcn = item.Cdcn;
                    }


                    if (request.IsProduction)
                    {
                        string uniqueInput = header.InvoiceNumber + DateTime.UtcNow.Ticks;
                        using var sha256 = SHA256.Create();
                        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(uniqueInput));
                        long hashInt = BitConverter.ToInt64(hashBytes, 0);
                        var taxIdInno = Math.Abs(hashInt % 10000000000L).ToString().PadRight(10, '0');

                        header.Inno = taxIdInno;

                        header.TaxId = TaxApiService.Instance.TaxIdGenerator.GenerateTaxId(
                                        MoadianConstants.ProductionProtectorId
                                        , long.Parse(header.Inno)
                                        , DateTime.UnixEpoch.AddMilliseconds((long)header.Indatim));
                    }
                    else
                    {
                        string uniqueInput = header.InvoiceNumber + DateTime.UtcNow.Ticks;
                        using var sha256 = SHA256.Create();
                        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(uniqueInput));
                        long hashInt = BitConverter.ToInt64(hashBytes, 0);
                        var taxIdInno = Math.Abs(hashInt % 10000000000L).ToString().PadRight(10, '0');

                        header.Inno = taxIdInno;

                        header.TaxId = TaxApiService.Instance.TaxIdGenerator.GenerateTaxId(
                                        MoadianConstants.SandboxProtectorId
                                        , long.Parse(header.Inno)
                                        , DateTime.UnixEpoch.AddMilliseconds((long)header.Indatim));
                    }

                    header.MoadianInvoiceDetails = new List<Data.Entities.MoadianInvoiceDetail>();
                    entities.Add(header);
                }
                else
                {
                    header = entities.Find(x => x.InvoiceNumber == header.InvoiceNumber);
                }

                var detail = new Data.Entities.MoadianInvoiceDetail();

                if (item.Sstid == default) header.Errors += ($"شناسه کالا در سطر {counter} خالی میباشد.\n");
                if (item.Am == default) header.Errors += ($"تعداد/مقدار در سطر {counter} خالی میباشد.\n");
                if (item.Fee == default) header.Errors += ($"مبلغ واحد در سطر {counter} خالی میباشد.\n");



                detail.Sstid = item.Sstid;
                detail.Sstt = item.Sstt;
                detail.Mu = item.Mu;
                detail.Am = item.Am;
                detail.Fee = item.Fee;


                detail.Prdis = Math.Floor((decimal)detail.Am * (decimal)detail.Fee);
                detail.Dis = item.Dis;
                detail.Adis = Math.Floor((decimal)detail.Prdis - (decimal)detail.Dis);

                detail.Vra = header.Inp == (int)MoadianConstants.InpTypes.First && detail.Sstid != "2720000176311" ? this.TaxPercentage : 0;

                detail.Vam = Math.Floor((decimal)detail.Adis * (decimal)(detail.Vra / 100));

                detail.Tsstam = detail.Adis + detail.Vam;



                // Export properties
                if (header.Inp == (int)MoadianConstants.InpTypes.Seventh)
                {
                    detail.Cut = item.Cut;
                    detail.Exr = item.Exr;
                    if (detail.Exr > 0) detail.Cfee = item.Fee / item.Exr;

                    detail.Nw = item.Nw;

                    detail.Ssrv = Math.Floor((decimal)detail.Am * (decimal)detail.Fee);
                    if (detail.Exr > 0) detail.Sscv = detail.Ssrv / detail.Exr;
                }




                header.MoadianInvoiceDetails.Add(detail);
            }


            foreach (var entity in entities)
            {
                entity.Tprdis = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Prdis).Sum());
                entity.Tdis = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Dis).Sum());
                entity.Tadis = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Adis).Sum());
                entity.Tvam = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Vam).Sum());
                entity.Tbill = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Tsstam).Sum());

                entity.Cap = entity.Tadis;

                // Export properties
                if (entity.Inp == (int)MoadianConstants.InpTypes.Seventh)
                {
                    entity.Tonw = entity.MoadianInvoiceDetails.Select(x => x.Nw).Sum();
                    entity.Tocv = entity.MoadianInvoiceDetails.Select(x => x.Sscv).Sum();
                    entity.Torv = entity.MoadianInvoiceDetails.Select(x => x.Ssrv).Sum();
                }
                entity.Errors = entity.Errors?.Length > 0 ? "خطاهای بارگزاری اطلاعات به سیستم:\n\n" + entity.Errors : null;
                entity.Status = entity.Errors?.Length > 0 ? "INVALID_DATA" : null;
            }
            return entities;
        }

        public async Task<MoadianCustomerDetailsModel> GetCustomerInfo(string accountReferenceCode, string customerCode)
        {

            var customerInfo = await _repository.GetQuery<Person>()
            .Where(x => x.AccountReference.Code == accountReferenceCode && x.Customers.Any(x => x.CustomerCode == customerCode))
            .Include(x => x.Customers)
            .Include(x => x.PersonAddresses)
            .Include(x => x.AccountReference)
            .ProjectTo<MoadianCustomerDetailsModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
            if (customerInfo != null && string.IsNullOrEmpty(customerInfo.EconomicCode)) customerInfo.EconomicCode = null;
            return customerInfo;
        }
        public List<T> MapFromExcel<T>(ImportByExcelMoadianInvoiceHeaderCommand payload)
        {
            string[] headerRow = new string[0];

            bool isFirstRow = true;
            List<T> convertedDataList = new List<T>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                payload.File.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each row of the file
                    {
                        var record = (T)Activator.CreateInstance(typeof(T));

                        Type convertedDataRecordType = record.GetType();

                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            var properties = convertedDataRecordType.GetProperties();

                            headerRow = new string[reader.FieldCount];

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var headerColValue = reader.GetValue(i) ?? "";
                                var propInfo = properties.Where(x => headerColValue.ToString().ToLower().Contains($"({x.Name.ToLower()})")).FirstOrDefault();
                                headerRow[i] = propInfo?.Name;
                            }
                        }
                        else
                        {
                            foreach (var propInfo in convertedDataRecordType.GetProperties())
                            {
                                var colHeaderName = headerRow.Where(x => propInfo.Name.EqualsIgnoreCase(x)).FirstOrDefault();
                                var propColumnIndexInPayload = colHeaderName != null ? Array.IndexOf(headerRow, colHeaderName) : -1;
                                if (propColumnIndexInPayload != -1)
                                {
                                    var propNewValue = reader.GetValue(propColumnIndexInPayload);
                                    if (propInfo.PropertyType == typeof(string))
                                    {
                                        propNewValue = propNewValue?.ToString()?.Trim();
                                        propNewValue = propNewValue?.ToString()?.Replace("ي", "ی");
                                        propNewValue = propNewValue?.ToString()?.Replace("ى", "ی");
                                        propNewValue = propNewValue?.ToString()?.Replace("ك", "ک");
                                        propNewValue = ToEnglishNumbers(propNewValue?.ToString());
                                    }


                                    propInfo.SetValue(record, Convert.ChangeType(propNewValue ?? propInfo.PropertyType.GetDefaultValue(), propInfo.PropertyType), null);
                                };
                            }
                            convertedDataList.Add(record);
                        }
                    }
                }
            }
            return convertedDataList;
        }


        public string ToEnglishNumbers(string number)
        {
            number = number?.Trim();
            if (string.IsNullOrEmpty(number)) return number;
            number = number.Replace('٠', '0');
            number = number.Replace('١', '1');
            number = number.Replace('٢', '2');
            number = number.Replace('٣', '3');
            number = number.Replace('٤', '4');
            number = number.Replace('٥', '5');
            number = number.Replace('٦', '6');
            number = number.Replace('٧', '7');
            number = number.Replace('٨', '8');
            number = number.Replace('٩', '9');
            number = number.Replace('.', '.');
            return number;
        }
    }
}
