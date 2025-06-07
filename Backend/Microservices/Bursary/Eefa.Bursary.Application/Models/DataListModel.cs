using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Application.Models
{
    public class DataListModel
    {
        public string DocumentNo { get; set; }
        public int DocumentId { get; set; }
        public DateTime DocumentDate { get; set; }
        public int CodeVoucherGroupId { get; set; }
        public string DebitAccountHeadId { get; set; }
        public string DebitAccountReferenceGroupId { get; set; }
        public string DebitAccountReferenceId { get; set; }
        public string CreditAccountHeadId { get; set; }
        public string CreditAccountReferenceGroupId { get; set; }
        public string CreditAccountReferenceId { get; set; }
        public long Amount { get; set; }
        public int DocumentTypeBaseId { get; set; }
        public string SheetUniqueNumber { get; set; }
        public int CurrencyFee { get; set; }
        public long CurrencyAmount { get; set; }
        public int CurrencyTypeBaseId { get; set; }
        public int NonRialStatus { get; set; }
        public int ChequeSheetId { get; set; }
        public string Description { get; set; }
        public bool IsRial { get; set; }
        public bool AccountIsFloat { get; set; }
        public string DocumentIds { get; set; }
        public string PersianDocumentDate { get; set; }
        public string InvoiceNumbers { get; set; }
        public int? SheetSeqNumber { get; set; }
        public bool? BesCurrencyStatus { get; set; }
        public bool? BedCurrencyStatus { get; set; }


    }

    public class SendDocument<T>
    {
        public bool saveChanges { get; set; } = true;
        public int menueId { get; set; } = 0;
        public int? voucherHeadId { get; set; } = null;
        public List<T> dataList { get; set; }
    }


    public class VoucherResultModel
    {
        public int voucherHeadId { get; set; }
        public int documentId { get; set; }
        public int voucherNo { get; set; }
    }


    public class ServiceResultModel<T>
    {
        public bool succeed { get; set; }
        public List<string> exceptions { get; set; }
        public T objResult { get; set; }
    }



}
