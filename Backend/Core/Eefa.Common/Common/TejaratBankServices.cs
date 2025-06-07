
using Eefa.Common.Common.Abstraction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eefa.Common
{
    public class TejaratBankServices:ITejaratBankServices
    {
        const string accountBalanceUrl = "https://cb.tejaratbank.ir:443/ws/accounthistoryService";

        public string SetDepositId(string inputNumber)
        {
            int[] digits = Array.ConvertAll(inputNumber.ToCharArray(), c => (int)char.GetNumericValue(c));

            int sum1 = 0;
            for (int i = 0; i < digits.Length; i++)
            {
                sum1 += digits[i] * (i + 1);
            }

            int n1 = (sum1 + 11) % 11;
            int checkDigit = (n1 < 9) ? n1 : 0;

            string result = inputNumber + checkDigit;

            return result;
        }
         
        public async Task<ServiceResult<ResponseTejaratModel>> CallGetCreditAccountBalance(string fromPersianDate, string toPersianDate, string accountNumber, string creditDebit)
        {

            var tejaratResult = new ResponseTejaratModel();

            XmlDocument xmlDoc = new XmlDocument();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, accountBalanceUrl);

            request.Headers.Add("SOAPAction", "");
            request.Method = HttpMethod.Post;

            var content = new StringContent("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:acc=\"http://sabapardazesh.com/schema/accountHistoryRequest\">\n   <soapenv:Header/>\n   <soapenv:Body>\n      <acc:AccountHistoryRequest>\n         <acc:Credential>\n            <acc:Identity>1002555877</acc:Identity>\n            <acc:Password>xn4OF7J4PT</acc:Password>\n         </acc:Credential>\n         <acc:accountNumber>" + accountNumber + "</acc:accountNumber>\n <acc:fromDate>" + fromPersianDate + "</acc:fromDate>\n         <acc:toDate>" + toPersianDate + "</acc:toDate>\n         <!--Optional:-->\n         <acc:fromTime>000000</acc:fromTime>\n         <!--Optional:-->\n         <acc:toTime>235959</acc:toTime>\n         <acc:transactionCount>5000</acc:transactionCount>\n         <acc:statementType>10</acc:statementType>\n    <acc:transactionType>" + creditDebit + "</acc:transactionType>\n \n      </acc:AccountHistoryRequest>\n   </soapenv:Body>\n</soapenv:Envelope>", Encoding.UTF8, "text/xml");
            request.Content = content;
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {

                try
                {

                    var result = await response.Content.ReadAsStringAsync();

                    xmlDoc.LoadXml(result);

                    dynamic jsonData = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);


                    Welcome res = JsonConvert.DeserializeObject<Welcome>(jsonData);

                    var model = new ResponseTejaratModel();

                    model.Balance = res.SoapenvEnvelope.SoapenvBody.AccountHistoryResponse.StatementBalance;
                    model.TransactionCount = res.SoapenvEnvelope.SoapenvBody.AccountHistoryResponse.TransactionCount;
                    model.DetailItems = res.SoapenvEnvelope.SoapenvBody.AccountHistoryResponse.AccountHistoryItems.AccountHistoryItem;


                    foreach (var item in model.DetailItems)
                    {
                        item.BankName = accountNumber;
                        item.init();
                    }



                    return ServiceResult<ResponseTejaratModel>.Success(model);
                }
                catch (Exception ex)
                {
                    return ServiceResult<ResponseTejaratModel>.Failed();

                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {error}");

                return ServiceResult<ResponseTejaratModel>.Failed();
            }

        }

        public async Task<ServiceResult<ResponseTejaratModel>> CallGetAccountBalance(string fromPersianDate, string toPersianDate, string accountNumber)
        {

            var tejaratResult = new ResponseTejaratModel();

            XmlDocument xmlDoc = new XmlDocument();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, accountBalanceUrl);

            request.Headers.Add("SOAPAction", "");
            request.Method = HttpMethod.Post;

            var content = new StringContent("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:acc=\"http://sabapardazesh.com/schema/accountHistoryRequest\">\n   <soapenv:Header/>\n   <soapenv:Body>\n      <acc:AccountHistoryRequest>\n         <acc:Credential>\n            <acc:Identity>1002555877</acc:Identity>\n            <acc:Password>xn4OF7J4PT</acc:Password>\n         </acc:Credential>\n         <acc:accountNumber>" + accountNumber + "</acc:accountNumber>\n <acc:fromDate>" + fromPersianDate + "</acc:fromDate>\n         <acc:toDate>" + toPersianDate + "</acc:toDate>\n         <!--Optional:-->\n         <acc:fromTime>000000</acc:fromTime>\n         <!--Optional:-->\n         <acc:toTime>235959</acc:toTime>\n         <acc:transactionCount>5000</acc:transactionCount>\n         <acc:statementType>10</acc:statementType>\n    <acc:transactionType>" + 'C' + "</acc:transactionType>\n \n     </acc:AccountHistoryRequest>\n   </soapenv:Body>\n</soapenv:Envelope>", Encoding.UTF8, "text/xml");
            request.Content = content;
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {

                try
                {

                    var result = await response.Content.ReadAsStringAsync();

                    xmlDoc.LoadXml(result);

                    dynamic jsonData = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);


                    Welcome res = JsonConvert.DeserializeObject<Welcome>(jsonData);

                    var model = new ResponseTejaratModel();

                    model.Balance = res.SoapenvEnvelope.SoapenvBody.AccountHistoryResponse.StatementBalance;
                    model.TransactionCount = res.SoapenvEnvelope.SoapenvBody.AccountHistoryResponse.TransactionCount;
                    model.DetailItems = res.SoapenvEnvelope.SoapenvBody.AccountHistoryResponse.AccountHistoryItems?.AccountHistoryItem;

                    
                    if(model.DetailItems != null)
                    foreach (var item in model?.DetailItems)
                    {
                        item.BankName = accountNumber;
                        item.init();
                    }



                    return ServiceResult<ResponseTejaratModel>.Success(model);
                }
                catch (Exception ex)
                {
                    return ServiceResult<ResponseTejaratModel>.Failed();

                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {error}");

                return ServiceResult<ResponseTejaratModel>.Failed();
            }

        }

    
    
    
    }
}
