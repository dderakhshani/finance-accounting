using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Eefa.Inventory.Domain;
using Eefa.Invertory.Infrastructure.Context;

namespace Eefa.Invertory.Infrastructure.Services.AdminApi
{
    public class AdminApiService : IAdminApiService
    {


        public async Task<Person> CallApiSavePerson(AdminApiService.PostPerson person, string Token, string Url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);
            HttpResponseMessage response = await client.PostAsJsonAsync(Url, person);
            response.EnsureSuccessStatusCode();
            var _person = response.Content.ReadFromJsonAsync<Person>();
            return await _person;
        }

        public async Task<ResultModel> CallApiAutoVoucher2(UpdateAutoVoucher request, string Token, string Url)
        {

            using (var httpClient = new HttpClient())
            {
                var requestUri = new Uri(Url);
                var jsonRequestData = JsonSerializer.Serialize(request);
                new LogWriter("inside CallApiAutoVoucher2  VoucherHeadId" + request.VoucherHeadId + "===================================== jsonRequestData=" + jsonRequestData);
                var httpContent = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var response = await httpClient.PostAsync(requestUri, httpContent);
                if (response != null)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content))
                    {
                        var result = JsonSerializer.Deserialize<ResultModel>(content);
                        var ResultJson = JsonSerializer.Serialize(request);
                        new LogWriter("response  CallApiAutoVoucher VoucherHeadId" + request.VoucherHeadId + "===================================== content=" + ResultJson);

                        return result;

                    }
                }

            }
            return new ResultModel();
        }

        public class PostPerson
        {

            public string firstName { get; set; }
            public string lastName { get; set; }
            public string fatherName { get; set; }
            public string nationalNumber { get; set; }
            public string identityNumber { get; set; }
            public DateTime birthDate { get; set; }
            public int birthPlaceCountryDivisionId { get; set; }
            public int genderBaseId { get; set; }
            public bool taxIncluded { get; set; }
            public int legalBaseId { get; set; }
            public int governmentalBaseId { get; set; }
            public int accountReferenceGroupId { get; set; }
        }


        public class ResultPerson
        {
            public int id { get; set; }
            public string fullName { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string fatherName { get; set; }
            public string nationalNumber { get; set; }
            public string identityNumber { get; set; }
            public object insuranceNumber { get; set; }
            public object mobileJson { get; set; }
            public object email { get; set; }
            public int accountReferenceId { get; set; }
            public string accountReferenceCode { get; set; }
            public DateTime birthDate { get; set; }
            public int birthPlaceCountryDivisionId { get; set; }
            public int genderBaseId { get; set; }
            public int legalBaseId { get; set; }
            public int governmentalBaseId { get; set; }
            public object legalBaseTitle { get; set; }
            public object governmentalBaseTitle { get; set; }
            public object genderBaseTitle { get; set; }
            public int accountReferenceGroupId { get; set; }
            public object[] personAddressList { get; set; }
            public object personFingerprintsList { get; set; }
            public object phoneNumbers { get; set; }
            public object photoURL { get; set; }
            public object signatureURL { get; set; }
            public bool taxIncluded { get; set; }
        }
        public class UpdateAutoVoucher
        {
            public long VoucherHeadId { get; set; }
            public List<Voucher> DataList { get; set; }

        }
        public class ResultModel
        {
            public object objResult { get; set; }
            public string message { get; set; }
            public Error[] errors { get; set; }
            public bool succeed { get; set; }
        }
        public class Error
        {
            public object source { get; set; }
            public object propertyName { get; set; }
            public string message { get; set; }
        }


        public class AutoVoucherResult
        {
            public int voucherHeadId { get; set; }
            public int documentId { get; set; }
            public int voucherNo { get; set; }
        }
        public class Voucher
        {

            public string FinancialOperationNumber { get; set; }
            public string Tag { get; set; }
            public string VoucherHeadId { get; set; }

            public string InvoiceNo { get; set; }
            public string DocumentDate { get; set; }
            public string CodeVoucherGroupId { get; set; }
            public string CodeVoucherGroupTitle { get; set; }
            public string DebitAccountHeadId { get; set; }
            public string DebitAccountReferencesGroupId { get; set; }
            public string DebitAccountReferenceId { get; set; }
            public string CreditAccountHeadId { get; set; }
            public string CreditAccountReferencesGroupId { get; set; }
            public string CreditAccountReferenceId { get; set; }
            public string ToTalPriceMinusVat { get; set; }
            public string VatDutiesTax { get; set; }
            public string PriceMinusDiscountPlusTax { get; set; }
            public string TotalQuantity { get; set; }
            public string TotalWeight { get; set; }
            public string ExtraCost { get; set; }
            public string DocumentId { get; set; }
            public string DocumentNo { get; set; }
            public string DocumentIds { get; set; }

            public string VoucherRowDescription { get; set; }
            public string DebitAccountHeadTitle { get; set; }
            public string CreditAccountHeadTitle { get; set; }



            public string CurrencyAmount { get; set; }
            public string CurrencyTypeBaseId { get; set; }
            public string CurrencyFee { get; set; }

            public string ExtraCostAccountHeadId { get; set; } = default!;
            public string ExtraCostAccountHeadTitle { get; set; } = default!;
            public string ExtraCostAccountReferenceGroupId { get; set; } = default!;
            public string ExtraCostAccountReferenceId { get; set; } = default!;

            public string IsFreightChargePaid { get; set; } = default!;

            public string TotalItemPrice { get; set; } = default!;




        }
    }
}
