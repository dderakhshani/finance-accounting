using System;

namespace Eefa.Bursary.Application.Models
{
    public class CustomerReceiptModel
    {
        public int InsertStatus { get; set; }
        public string TargetAccountName { get; set; }
        public string ReceicePersianDate { get; set; }
        public string BankName { get; set; }
        public string PaymentNumber { get; set; }
        public long AmountPay { get; set; }
        public string AccountNo { get; set; }
        public string CreatorName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int TaskId { get; set; }
        public int AccumulativePaymentTypes { get; set; }
        public string AccountOwner { get; set; }
        public string FullName { get; set; }
        public bool AccountIsFloat { get; set; }
        public string SubjectTitle { get; set; }
        public string TargetAccount { get; set; }
        public string TargetAccountCode { get; set; }
        public bool? TargetAccountIsFloat { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public string FeeTypeTitle { get; set; }
        public string PaymentTitle { get; set; }
        public string FactorTitle { get; set; }
        public string PaymentStatusTitle { get; set; }
        public string PaymentCurrencyTitle { get; set; }
        public string AccountSheba { get; set; }
        public DateTime InvoiceIssuanceDateTime { get; set; }
        public string InvoiceIssuanceDate { get; set; }
        public string PersianDateTime { get; set; }
        public string Code { get; set; }
        public long DeductionSum { get; set; }
        public string PersianFinalIssuance { get; set; }
        public DateTime? CreateTime { get; set; }
        public string StateTitle { get; set; }
        public string BankCode { get; set; }
        public string DebtorSubsidiaryCode { get; set; }
        public string DebtorSubsidiaryName { get; set; }
        public int CustomerType { get; set; }
        public string CreditorSubsidiaryCode { get; set; }
        public string CreditorSubsidiaryName { get; set; }
        public string CustomerPaymentIdentifier { get; set; }
        public string DocNumber { get; set; }
        public string AccountCode { get; set; }
        public string FloatingCode { get; set; }
        public int GuaranteeType { get; set; }
        public string GuaranteeTypeTitle { get; set; }
        public int Id { get; set; }
        public int CustomerPaymentId { get; set; }
        public int PaymentType { get; set; }
        public int FactorType { get; set; }
        public long TotalCost { get; set; }
        public string ProductName { get; set; }
        public string InvoiceNumber { get; set; }
        public string RequestNumber { get; set; }
        public string Description { get; set; }
        public System.DateTime DateTime { get; set; }
        public Nullable<int> SubjectBaseId { get; set; }
        public Nullable<int> FacilityNumber { get; set; }
        public Nullable<int> PaymentCurrency { get; set; }
        public Nullable<int> FeeType { get; set; }
        public int Status { get; set; }
        public string MissedDocument { get; set; }
        public int UserId { get; set; }
        public bool IsDelete { get; set; }
        public string State { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public bool IsAccumulativePayment { get; set; }
        public bool ForeignTrading { get; set; }
        public bool IsEmergent { get; set; }
        public short AccumulativeSelectStatus { get; set; }
        public Nullable<int> DebtorSubsidiaryCodeId { get; set; }
        public Nullable<int> CreditorSubsidiaryCodeId { get; set; }
        public short AuditStatus { get; set; }
        public string Deduction { get; set; }
        public Nullable<int> LendingBankId { get; set; }
    }
}
