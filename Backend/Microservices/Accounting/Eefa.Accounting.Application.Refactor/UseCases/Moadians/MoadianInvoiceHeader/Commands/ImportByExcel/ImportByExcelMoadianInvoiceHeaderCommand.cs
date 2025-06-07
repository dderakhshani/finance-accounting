using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;

public class ImportByExcelMoadianInvoiceHeaderCommand : IRequest<ServiceResult<List<MoadianInvoiceHeaderDetailedModel>>>, IMapFrom<ImportByExcelMoadianInvoiceHeaderCommand>
{
    public IFormFile File { get; set; }

    public bool IsProduction { get; set; } = false;

    public bool GenerateTaxIdWithListNumber { get; set; }

}

//public class ImportByExcelMoadianInvoiceHeaderCommandHandler : IRequestHandler<ImportByExcelMoadianInvoiceHeaderCommand, ServiceResult<List<MoadianInvoiceHeaderDetailedModel>>>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    public ImportByExcelMoadianInvoiceHeaderCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
//    {
//        _mapper = mapper;
//        _unitOfWork= unitOfWork;
//    }
//    public async Task<ServiceResult<List<MoadianInvoiceHeaderDetailedModel>>> Handle(ImportByExcelMoadianInvoiceHeaderCommand request, CancellationToken cancellationToken)
//    {
//        var entities = await this.ConvertExelToEntities(request);

//        foreach (var entity in entities)
//        {
//            entity.UId = Guid.NewGuid().ToString();
//            _unitOfWork.MoadianInvoiceHeaders.Add(entity);
//        }
//        await _unitOfWork.SaveChangesAsync(cancellationToken);

//        return ServiceResult.Success(_mapper.Map<List<MoadianInvoiceHeaderDetailedModel>>(entities));
//    }

//    public async Task<List<MoadianInvoiceHeader>> ConvertExelToEntities(ImportByExcelMoadianInvoiceHeaderCommand request)
//    {
//        var listNumber = await _unitOfWork.MoadianInvoiceHeaders.GetListAsync(x => x.IsSandbox == !request.IsProduction, y => y.MaxAsync(x => (long?)x.ListNumber) ?? 0);
//        listNumber++;

//        var convertedData = this.MapFromExcel<MoadianInvoiceImportPayloadModel>(request);

//        convertedData = convertedData.Where(x => !string.IsNullOrEmpty(x.Inno)).ToList();

//        var entities = new List<MoadianInvoiceHeader>();

//        var counter = 1;

//        if (request.IsProduction)
//        {
//            TaxApiService.Instance.Init(
//                MoadianConstants.ProductionProtectorId,
//                new SignatoryConfig(MoadianConstants.PrivateKey, null),
//                new NormalProperties(ClientType.SELF_TSP, "v1"),
//                MoadianConstants.ProductionApiUrl
//                );
//        }
//        else
//        {
//            TaxApiService.Instance.Init(
//                MoadianConstants.SandboxProtectorId,
//                new SignatoryConfig(MoadianConstants.PrivateKey, null),
//                new NormalProperties(ClientType.SELF_TSP, "v1"),
//                MoadianConstants.SandboxApiUrl
//                );
//        }
//        var serverInformation = await TaxApiService.Instance.TaxApis.GetServerInformationAsync();
//        var token = await TaxApiService.Instance.TaxApis.RequestTokenAsync();

//        foreach (var item in convertedData)
//        {
//            item.Inno = new String('0', 10 - item.Inno.Length) + item.Inno;
//            counter++;
//            var errors = new List<string>();

//            var header = new MoadianInvoiceHeader
//            {
//                Inno = item.Inno,
//                ListNumber = listNumber
//            };

//            if (!entities.Any(x => x.Inno == header.Inno))
//            {
//                header.IsSandbox = !request.IsProduction;

//                if (string.IsNullOrEmpty(item.Inno)) header.Errors += ($"شماره صوتحساب در سطر {counter} خالی میباشد.\n");
//                if (item.Inp == default) header.Errors += ($"الگوی صوتحساب در سطر {counter} خالی میباشد.\n");

//                header.Inno = item.Inno;
//                header.InvoiceDate = item.Indatim;
//                header.Indatim = new DateTimeOffset(item.Indatim).ToUnixTimeMilliseconds();
//                header.Indati2m = new DateTimeOffset(item.Indatim).ToUnixTimeMilliseconds();
//                header.Inty = (int)MoadianConstants.IntyTypes.First;
//                header.Inp = item.Inp;
//                header.Ins = item.Ins;
//                header.Tins = MoadianConstants.EefaEconomicCode;

//                // Replace Customer Details
//                var customerInfo = await GetCustomerInfo(item.Acc, item.Ccc);
//                if (customerInfo != null)
//                {
//                    header.Bid = customerInfo.LegalType == 3 ? customerInfo.EconomicCode : customerInfo.NationalNumber?.Split("-")?.FirstNonDefault()?.Trim();
//                    header.Tinb = customerInfo.LegalType == 1 ? (customerInfo.EconomicCode == customerInfo.NationalNumber ? null : customerInfo.EconomicCode) : (customerInfo.LegalType == 2 ? header.Bid : customerInfo.EconomicCode);
//                    header.Tob = customerInfo.LegalType;
//                    header.Bpc = customerInfo.PostalCode;

//                    header.PersonId = customerInfo.PersonId;
//                    header.CustomerId = customerInfo.CustomerId;
//                    header.AccountReferenceId = customerInfo.AccountReferenceId;
//                }
//                else
//                {
//                    header.Errors += ($"مشتری با کد تفصیل {item.Acc} و کد مشتری {item.Ccc} در صورتحساب شماره {item.Inno} پیدا نشد.\n");
//                }

//                header.Setm = (int)MoadianConstants.SetmTypes.First;

//                // Export properties
//                if (header.Inp == (int)MoadianConstants.InpTypes.Seventh)
//                {
//                    header.Scln = item.Scln;
//                    header.Scc = item.Scc;

//                    //header.Cdcd = item.Cdcd != default ? new DateTimeOffset(item.Cdcd).ToUnixTimeMilliseconds() : null;
//                    header.Cdcd = null;
//                    header.Cdcn = item.Cdcn;
//                }


//                if (request.IsProduction)
//                {
//                    var taxIdInno = request.GenerateTaxIdWithListNumber == true ? ((listNumber * 1000000) + Convert.ToInt64(header.Inno)) : Convert.ToInt64(header.Inno);

//                    header.TaxId = TaxApiService.Instance.TaxIdGenerator.GenerateTaxId(
//                                    MoadianConstants.ProductionProtectorId
//                                    , taxIdInno
//                                    , new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)header.Indatim));
//                }
//                else
//                {
//                    var R = new Random();
//                    long randomNumber = (long)R.Next(10000, 99999);
//                    long baseInnoNumber = randomNumber * 1000000;

//                    long convertedInnoNumber = Convert.ToInt64(header.Inno);
//                    long randomInno = baseInnoNumber + convertedInnoNumber;

//                    var taxIdInno = request.GenerateTaxIdWithListNumber == true ? ((listNumber * 1000000) + Convert.ToInt64(header.Inno)) : randomInno;

//                    header.TaxId = TaxApiService.Instance.TaxIdGenerator.GenerateTaxId(
//                                    MoadianConstants.SandboxProtectorId
//                                    , taxIdInno
//                                    , new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)header.Indatim));
//                }
//                header.MoadianInvoiceDetails = new List<MoadianInvoiceDetail>();
//                entities.Add(header);
//            }
//            else
//            {
//                header = entities.Find(x => x.Inno == header.Inno);
//            }

//            var detail = new MoadianInvoiceDetail();

//            if (item.Sstid == default) header.Errors += ($"شناسه کالا در سطر {counter} خالی میباشد.\n");
//            if (item.Am == default) header.Errors += ($"تعداد/مقدار در سطر {counter} خالی میباشد.\n");
//            if (item.Fee == default) header.Errors += ($"مبلغ واحد در سطر {counter} خالی میباشد.\n");

//            detail.Sstid = item.Sstid;
//            detail.Sstt = item.Sstt;
//            detail.Mu = item.Mu;
//            detail.Am = item.Am;
//            detail.Fee = item.Fee;


//            detail.Prdis = Math.Floor((decimal)detail.Am * (decimal)detail.Fee);
//            detail.Dis = item.Dis;
//            detail.Adis = Math.Floor((decimal)detail.Prdis - (decimal)detail.Dis);

//            detail.Vra = header.Inp == (int)MoadianConstants.InpTypes.First ? 9 : 0;

//            detail.Vam = Math.Floor((decimal)detail.Adis * (decimal)(detail.Vra / 100));

//            detail.Tsstam = detail.Adis + detail.Vam;

//            // Export properties
//            if (header.Inp == (int)MoadianConstants.InpTypes.Seventh)
//            {
//                detail.Cut = item.Cut;
//                detail.Exr = item.Exr;
//                if (detail.Exr > 0) detail.Cfee = item.Fee / item.Exr;

//                detail.Nw = item.Nw;

//                detail.Ssrv = Math.Floor((decimal)detail.Am * (decimal)detail.Fee);
//                if (detail.Exr > 0) detail.Sscv = detail.Ssrv / detail.Exr;
//            }

//            header.MoadianInvoiceDetails.Add(detail);
//        }

//        foreach (var entity in entities)
//        {
//            entity.Tprdis = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Prdis).Sum());
//            entity.Tdis = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Dis).Sum());
//            entity.Tadis = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Adis).Sum());
//            entity.Tvam = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Vam).Sum());
//            entity.Tbill = Math.Floor(entity.MoadianInvoiceDetails.Select(x => (decimal)x.Tsstam).Sum());

//            entity.Cap = entity.Tadis;

//            // Export properties
//            if (entity.Inp == (int)MoadianConstants.InpTypes.Seventh)
//            {
//                entity.Tonw = entity.MoadianInvoiceDetails.Select(x => x.Nw).Sum();
//                entity.Tocv = entity.MoadianInvoiceDetails.Select(x => x.Sscv).Sum();
//                entity.Torv = entity.MoadianInvoiceDetails.Select(x => x.Ssrv).Sum();
//            }
//            entity.Errors = entity.Errors?.Length > 0 ? "خطاهای بارگزاری اطلاعات به سیستم:\n\n" + entity.Errors : null;
//            entity.Status = entity.Errors?.Length > 0 ? "INVALID_DATA" : null;
//        }
//        return entities;
//    }

//    public async Task<MoadianCustomerDetailsModel> GetCustomerInfo(string accountReferenceCode, string customerCode)
//    {
//        Specification<Person> specification = new Specification<Person> ();
//        specification.ApplicationConditions.Add(x => 
//            x.AccountReference.Code == accountReferenceCode && 
//            x.Customers.Any(x => x.CustomerCode == customerCode));
//        specification.Includes = x =>
//            x.Include(x => x.Customers)
//             .Include(x => x.PersonAddresses)
//             .Include(x => x.AccountReference);
//        var customerInfo = await _unitOfWork.Persons
//                .GetProjectedAsync<MoadianCustomerDetailsModel>(specification);
//        if (customerInfo != null && string.IsNullOrEmpty(customerInfo.EconomicCode)) customerInfo.EconomicCode = null;
//        return customerInfo;
//    }
//    public List<T> MapFromExcel<T>(ImportByExcelMoadianInvoiceHeaderCommand payload)
//    {
//        string[] headerRow = new string[0];

//        bool isFirstRow = true;
//        List<T> convertedDataList = new List<T>();

//        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
//        using (var stream = new MemoryStream())
//        {
//            payload.File.CopyTo(stream);
//            stream.Position = 0;
//            using (var reader = ExcelReaderFactory.CreateReader(stream))
//            {
//                while (reader.Read()) //Each row of the file
//                {
//                    var record = (T)Activator.CreateInstance(typeof(T));

//                    Type convertedDataRecordType = record.GetType();

//                    if (isFirstRow)
//                    {
//                        isFirstRow = false;
//                        var properties = convertedDataRecordType.GetProperties();

//                        headerRow = new string[reader.FieldCount];

//                        for (int i = 0; i < reader.FieldCount; i++)
//                        {
//                            var headerColValue = reader.GetValue(i) ?? "";
//                            var propInfo = properties.Where(x => headerColValue.ToString().ToLower().Contains($"({x.Name.ToLower()})")).FirstOrDefault();
//                            headerRow[i] = propInfo?.Name;
//                        }
//                    }
//                    else
//                    {
//                        foreach (var propInfo in convertedDataRecordType.GetProperties())
//                        {
//                            var colHeaderName = headerRow.Where(x => propInfo.Name.EqualsIgnoreCase(x)).FirstOrDefault();
//                            var propColumnIndexInPayload = colHeaderName != null ? Array.IndexOf(headerRow, colHeaderName) : -1;
//                            if (propColumnIndexInPayload != -1)
//                            {
//                                var propNewValue = reader.GetValue(propColumnIndexInPayload);
//                                if (propInfo.PropertyType == typeof(string))
//                                {
//                                    propNewValue = propNewValue?.ToString()?.Trim();
//                                    propNewValue = propNewValue?.ToString()?.Replace("ي", "ی");
//                                    propNewValue = propNewValue?.ToString()?.Replace("ى", "ی");
//                                    propNewValue = propNewValue?.ToString()?.Replace("ك", "ک");
//                                    propNewValue = ToEnglishNumbers(propNewValue?.ToString());
//                                }


//                                propInfo.SetValue(record, Convert.ChangeType(propNewValue ?? propInfo.PropertyType.GetDefaultValue(), propInfo.PropertyType), null);
//                            };
//                        }
//                        convertedDataList.Add(record);
//                    }
//                }
//            }
//        }
//        return convertedDataList;
//    }

//    public string ToEnglishNumbers(string number)
//    {
//        number = number?.Trim();
//        if (string.IsNullOrEmpty(number)) return number;
//        number = number.Replace('٠', '0');
//        number = number.Replace('١', '1');
//        number = number.Replace('٢', '2');
//        number = number.Replace('٣', '3');
//        number = number.Replace('٤', '4');
//        number = number.Replace('٥', '5');
//        number = number.Replace('٦', '6');
//        number = number.Replace('٧', '7');
//        number = number.Replace('٨', '8');
//        number = number.Replace('٩', '9');
//        number = number.Replace('.', '.');
//        return number;
//    }
//}