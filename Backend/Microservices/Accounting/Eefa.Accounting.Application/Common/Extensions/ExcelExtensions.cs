using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Common.Extensions
{
    public static class ExcelExtensions
    {

        public static List<T> ParseTo<T>(this IFormFile file, List<string> headersColumnIndex)
        {
            bool isFirstRow = true;
            List<T> convertedDataList = new List<T>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each row of the file
                    {
                        if (isFirstRow) isFirstRow = false;
                        else
                        {
                            var record = (T)Activator.CreateInstance(typeof(T));

                            Type convertedDataRecordType = record.GetType();

                            foreach (var propInfo in convertedDataRecordType.GetProperties())
                            {
                                try
                                {
                                    var propColumnIndexInPayload = Int32.Parse(headersColumnIndex.FirstOrDefault(x => x.Split(",").First().EqualsIgnoreCase(propInfo.Name))?.Split(",")?.Last() ?? "-1");

                                    if (propColumnIndexInPayload != -1)
                                    {
                                        var propNewValue = reader.GetValue(propColumnIndexInPayload);
                                        if (propNewValue != null && (propInfo.PropertyType == typeof(string) || propNewValue.GetType() == typeof(string)))
                                        {
                                            propNewValue = propNewValue?.ToString()?.Trim();
                                            propNewValue = propNewValue?.ToString()?.Replace("ي", "ی");
                                            propNewValue = propNewValue?.ToString()?.Replace("ى", "ی");
                                            propNewValue = propNewValue?.ToString()?.Replace("ك", "ک");
                                            propNewValue = propNewValue?.ToString()?.ToEnglishNumbers();
                                        }


                                        if (propNewValue != null) propInfo.SetValue(record, Convert.ChangeType(propNewValue, propInfo.PropertyType), null);
                                    };
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(propInfo.Name);
                                }
                            }
                            convertedDataList.Add(record);
                        }



                    }
                }
            }
            return convertedDataList;
        }
    }
}
