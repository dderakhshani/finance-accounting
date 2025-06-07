using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Library.Utility
{

    public class SsrsUtil
    {
        public enum ReportFormat
        {
            None = 1,
            Pdf = 2,
            Excel = 3,
            Xml = 4,
            Csv = 5,
            Image = 6,
            Word = 7,
            Html = 8,
            Pptx = 9,
        }

        public string GetContentType(ReportFormat format)
        {
            var contentType = format switch
            {
                ReportFormat.Xml => "application/xml",
                ReportFormat.Pdf => "application/pdf",
                ReportFormat.Excel => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ReportFormat.Csv => "text/csv",
                ReportFormat.Image => "image/png",
                ReportFormat.Word => "application/docx",
                ReportFormat.Html => "text/html",
                ReportFormat.Pptx => "application/vnd.ms-powerpoint",
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };

            return contentType;
        }

        public string GetFileExtention(ReportFormat format)
        {
            var extentionType = format switch
            {
                ReportFormat.Xml => "xml",
                ReportFormat.Pdf => "pdf",
                ReportFormat.Excel => "xlsx",
                ReportFormat.Csv => "csv",
                ReportFormat.Image => "png",
                ReportFormat.Word => "doc",
                ReportFormat.Html => "html",
                ReportFormat.Pptx => "pptx",
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };

            return extentionType;
        }

        public async Task<FileResult> GetReport<TEntity>(TEntity entity, string reportName, ReportFormat reportFormat,
            Dictionary<string, string?>? paramters = null)
        {
            var steamReport = await new SsrsUtil().LoadReport(entity, reportName, reportFormat);
            var fileContentResult = new FileStreamResult(steamReport, GetContentType(reportFormat))
            {
                FileDownloadName = $"{Guid.NewGuid()}.{GetFileExtention(reportFormat)}"
            };
            return fileContentResult;
        }

        public async Task<MemoryStream> LoadReport<TEntity>(TEntity entity, string reportName, ReportFormat reportFormat)
        {
            var url = new StringBuilder();
            url.Append("http://192.168.2.150:80/ReportServer/Pages/ReportViewer.aspx?%2fReporting%2f");
            url.Append(reportName);
            url.Append("&rs:Command=Render");
            url.Append("&rc:Toolbar=False");
            url.Append("&rc:Parameters=Collapsed");
            //url.Append("&rc:FindString=Code");

            if (reportFormat != ReportFormat.Html)
            {
                url.Append("&rc:PageWidth=14in&rc:PageHeight=8.5in");
                url.Append("&rc:MarginTop=0.1in&rc:MarginRight=0.1in&rc:MarginBottom=0.1in&rc:MarginLeft=0.1in");
            }

            switch (reportFormat)
            {
                case ReportFormat.Pdf:
                    url.Append($"&rs:Format=PDF");
                    break;
                case ReportFormat.Excel:
                    url.Append($"&rs:Format=EXCELOPENXML");
                    break;
                case ReportFormat.Xml:
                    url.Append($"&rs:Format=XML");
                    break;
                case ReportFormat.Csv:
                    url.Append($"&rs:Format=CSV");
                    break;
                case ReportFormat.Image:
                    url.Append($"&rs:Format=IMAGE");
                    break;
                case ReportFormat.Word:
                    url.Append($"&rs:Format=WORD");
                    break;
                case ReportFormat.Html:
                    url.Append($"&rs:Format=HTML5");
                    break;
                case ReportFormat.Pptx:
                    url.Append($"&rs:Format=PPTX");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reportFormat), reportFormat, null);
            }



            foreach (var paramter in entity.GetType().GetProperties())
            {
                url.Append($"&{paramter.Name}");

                url.Append(paramter.GetValue(entity) == null ? ":IsNull=True" : $"={paramter.GetValue(entity)?.ToString()}");

            }


            var webClient = new WebClient();
            webClient.Credentials = new NetworkCredential("administrator", "Eef@2023@");
            //webClient.UseDefaultCredentials = true;
            var a = url.ToString();

            try
            {
                var myDataBuffer = await webClient.DownloadDataTaskAsync(new Uri(url.ToString()));

                return new MemoryStream(myDataBuffer);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}