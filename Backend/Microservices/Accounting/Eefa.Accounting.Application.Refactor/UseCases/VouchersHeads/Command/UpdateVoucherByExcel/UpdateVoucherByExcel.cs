using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
// TODO check the Handler class
public class UpdateVoucherByExcel : IRequest<ServiceResult>
{
    public IFormFile File { get; set; }

    public int VoucherId { get; set; }
}

public class VoucherDetailExcelUpdateModel
{
    public string ARG { get; set; }
    public string AR { get; set; }
    public long Amount { get; set; }
    public string Name { get; set; }
}

//public class UpdateVoucherByExcelHandler : IRequestHandler<UpdateVoucherByExcel, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;

//    public UpdateVoucherByExcelHandler(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork= unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(UpdateVoucherByExcel request, CancellationToken cancellationToken)
//    {
//        var convertedData = MapFromExcel<VoucherDetailExcelUpdateModel>(request.File);

//        var voucherDetails = await _unitOfWork.VouchersDetails.GetListAsync(x => x.VoucherId == request.VoucherId);

//        foreach (var detail in convertedData)
//        {
//            var accountReferenceId = await _unitOfWork.AccountReferences.GetAsync(x => x.Code == detail.AR, y => y.Select(z => z.Id));
//            var voucherDetailToUpdate = voucherDetails.FirstOrDefault(x => x.ReferenceId1 == accountReferenceId && x.AccountReferencesGroupId == (detail.ARG == "04001" ? 28 : 29));

//            if (voucherDetailToUpdate != null)
//            {
//                voucherDetailToUpdate.Debit = detail.Amount > 0 ? detail.Amount : 0;
//                voucherDetailToUpdate.Credit = detail.Amount < 0 ? detail.Amount * -1 : 0;
//                _unitOfWork.VouchersDetails.Update(voucherDetailToUpdate);
//                await _unitOfWork.SaveChangesAsync(cancellationToken);
//            }
//        };

//        return ServiceResult.Success();
//    }

//    public List<T> MapFromExcel<T>(IFormFile file)
//    {
//        string[] headerRow = new string[0];

//        bool isFirstRow = true;
//        List<T> convertedDataList = new List<T>();

//        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
//        using (var stream = new MemoryStream())
//        {
//            file.CopyTo(stream);
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
//                reader.Close();
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