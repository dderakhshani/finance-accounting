using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Eefa.Common.Data;
using Eefa.Common.Domain;
using Eefa.Purchase.Domain.Common;
using Eefa.Purchase.Domain.Entities;
using Eefa.Purchase.Domain.Entities.SqlView;

namespace Eefa.Purchase.Domain.Aggregates.InvoiceAggregate
{
    /// <summary>
    /// DataBase Table: DocumentHead
    /// </summary>
    [Table("DocumentHead", Schema = "inventory")]
    public partial class Invoice : DomainBaseEntity, IAggregateRoot
    {
       
        public int CodeVoucherGroupId { get; set; } = default!;
        /// <description>
        /// کد سال
        ///</description>

        public int YearId { get; set; } = default!;
        /// <description>
        /// کد انبار
        ///</description>

        public int WarehouseId { get; set; } = default!;
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; } = default!;
        /// <description>
        /// کد مرجع
        ///</description>

        public int? DebitAccountHeadId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;

        public int? CreditAccountHeadId { get; set; } = default!;
        public int? CreditAccountReferenceId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; } = default!;
        
        /// <description>
        /// شماره سند
        ///</description>

        public int? DocumentNo { get; set; } = default!;
        /// <description>
        /// تاریخ سند
        ///</description>

        public DateTime DocumentDate { get; set; } = default!;
        /// <description>
        /// تاریخ درخواست
        ///</description>
        public DateTime? RequestDate { get; set; } = default!;
        /// <description>
        /// توضیحات سند
        ///</description>

        public string DocumentDescription { get; set; }

        /// <description>
        /// شماره فاکتور فروشنده
        ///</description>
        public string InvoiceNo { get; set; }
        public string FinancialOperationNumber { get; set; }

        public string DocumentSerial { get; set; }
        /// <description>
        /// کد وضعیت سند
        ///</description>

        public int DocumentStateBaseId { get; set; } = default!;
        /// <description>
        /// دستی
        ///</description>

        public bool IsManual { get; set; } = default!;
        /// <description>
        /// کد سند حسابداری
        ///</description>

        public int? VoucherHeadId { get; set; }
        public double TotalWeight { get; set; } = default!;
        public double TotalQuantity { get; set; } = default!;
        /// <description>
        /// جمع مبلغ قابل پرداخت
        ///</description>

        public double TotalItemPrice { get; set; } = default!;
        /// <description>
        /// عوارض ارزش افزوده
        ///</description>

        public long VatTax { get; set; } = default!;
        /// <description>
        /// مالیات ارزش افزوده
        ///</description>

        public long VatDutiesTax { get; set; } = default!;
        /// <description>
        /// عوارض سلامت
        ///</description>

        public long HealthTax { get; set; } = default!;

        /// <description>
        /// درصد مالیات ارزش افزوده
        ///</description>
        public int VatPercentage { get; set; } = default!;
        /// <description>
        /// جمع تخفیف
        ///</description>

        public long TotalItemsDiscount { get; set; } = default!;
        /// <description>
        /// جمع قیمت تمام شده
        ///</description>

        public double? TotalProductionCost { get; set; } = default!;
        /// <description>
        /// درصد تخفیف کل فاکتور
        ///</description>

        public double? DiscountPercent { get; set; } = default!;
        /// <description>
        /// تخفیف کل سند
        ///</description>

        public double? DocumentDiscount { get; set; } = default!;
        /// <description>
        /// قیمت بعد از کسر تخفیف
        ///</description>

        public double? PriceMinusDiscount { get; set; } = default!;
        /// <description>
        /// قیمت با مالیات بعد از کسر تخفیف 
        ///</description>

        public double? PriceMinusDiscountPlusTax { get; set; } = default!;
        /// <description>
        /// نوع پرداخت
        ///</description>

        public int PaymentTypeBaseId { get; set; } = default!;
        /// <description>
        /// تاریخ انقضا
        ///</description>

        public DateTime? ExpireDate { get; set; } = default!;
        /// <description>
        /// شماره بخش
        ///</description>

        public string? PartNumber { get; set; } = default!;
        public string RequestNo { get; set; } = default!;
        public string Tags { get; set; } = default!;

        public bool? IsPlacementComplete { get; set; } = default!;
        public bool? IsImportPurchase { get; set; } = default!;

        public string CommandDescription { get; set; } = default!;

        public int DocumentStauseBaseValue { get; set; } = default!;
       
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }



        public virtual BaseValue DocumentStateBase { get; set; } = default!;
        //public virtual Person Person { get; set; } = default!;
        public virtual Warehouse Warehouse { get; set; } = default!;

       
        public virtual CodeVoucherGroup CodeVoucherGroup { get; set; } = default!;
        public virtual Invoice? Parent { get; set; }
        public virtual BaseValue PaymentTypeBase { get; set; } = default!;
        public virtual Year Year { get; set; } = default!;

        public virtual ICollection<DocumentItem> Items { get; set; } = default!;
        public virtual ICollection<DocumentPayment> Payments { get; set; } = default!;
        public virtual ICollection<Invoice> InverseParent { get; set; } = default!;
        
        //-------------------------------------------------------------------------------------
        public DocumentItem AddItem(DocumentItem documentItem)
        {
            documentItem.YearId = YearId;
            Items ??= new List<DocumentItem>();
            this.Items.Add(documentItem);
            return documentItem;
        }
        //-----------------------------------------DocumentItem---------------------------------
        public static async Task<int> AddDocumentItemAsync(DocumentItem model, IRepository<DocumentItem> _Repository)
        {

            _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();

        }
        public static void AddDocumentItem(DocumentItem model, IRepository<DocumentItem> _Repository)
        {

            _Repository.Insert(model);
           

        }
        public static async Task<int> UpdateDocumentItemAsync(DocumentItem model, IRepository<DocumentItem> _Repository)
        {

            _Repository.Update(model);
            return await _Repository.SaveChangesAsync();

        }
        public static void UpdateDocumentItem(DocumentItem model, IRepository<DocumentItem> _Repository)
        {

            _Repository.Update(model);


        }
        //-----------------------------------------Invoice---------------------------------
        public static async Task<int> AddInvoiceAsync(Invoice model, IRepository<Invoice> _Repository)
        {

            _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();



        }

        public static async Task<int> UpdateInvoiceAsync(Invoice model, IRepository<Invoice> _Repository)
        {

            _Repository.Update(model);
            return await _Repository.SaveChangesAsync();

        }

        public static void UpdateInvoice(Invoice model, IRepository<Invoice> _Repository)
        {

            _Repository.Update(model);


        }

        //-----------------------------------------Document---------------------------------
        public static async Task<int> AddDocumentAsync(Document model, IRepository<Document> _Repository)
        {
                _Repository.Insert(model);
                return await _Repository.SaveChangesAsync();
        }

        public static async Task<int> UpdateDocumentAsync(Document model, IRepository<Document> _Repository)
        {

            _Repository.Update(model);
            return await _Repository.SaveChangesAsync();

        }
        //-----------------------------------------DocumentHeadExtend-------------------------
        public static async Task<int> AddDocumentHeadExtendAsync(DocumentHeadExtend model, IRepository<DocumentHeadExtend> _Repository)
        {

            _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();

        }

        public static async Task<int> UpdateDocumentHeadExtendAsync(DocumentHeadExtend model, IRepository<DocumentHeadExtend> _Repository)
        {

            _Repository.Update(model);
            
            return await _Repository.SaveChangesAsync();

        }
        //--------------------------------آیا تامین کننده خارجی است-----------------------------------------------------
        public static Invoice UpdateImportPurchaseInvoice(Invoice Invoice, List<AccountReferenceView> model)
        {
            var referenceGroupList = model;


            var countImport = referenceGroupList.Where(a => a.Code.Length > 2 && a.Code.Substring(2, 2) == ConstantValues.AccountReferenceGroup.ExternalProvider).ToList();
            if (countImport.Any())
            {
                Invoice.IsImportPurchase = true;

            }
            return Invoice;
        }
        //------------------------------------------------------------
        public static Invoice ConvertTagArray(string Tag, Invoice a)
        {
            if (String.IsNullOrEmpty(Tag))
            {
                return a;
            }
            string[] TagArray = Tag.Split(",");
            

            a.Tags = "[";
            foreach (var item in TagArray)
            {
                if (item != "")
                    a.Tags = a.Tags + "{\"Key\":\"" + item + "\"},";
            }
            if (TagArray.Length > 0)
            {
                a.Tags.Remove(a.Tags.Length - 1, 1);
            }
            a.Tags = a.Tags + "]";
            return a;
        }

        public static DateTime? SetExpireDate(DateTime? ExpireDate, DateTime LastDate)
        {

            //اگر تاریخ پایان انتخاب نشده باشد  یا تاریخ پایان بیشتر از پایان سال باشد ، تاریخ به صورت پیش فرض می خورد
            if (ExpireDate == default || ExpireDate > LastDate)
            {
                ExpireDate = LastDate;
            }
            return ExpireDate;
        }

    }


}
