using System;
using AutoMapper;
 
using Eefa.Common;
 

namespace Eefa.Bursary.Application.Models
{
    public class ChequeSheetModel : IMapFrom<Domain.Entities.ChequeSheets>
    {
        public int Id { get; set; }
        public int? PayChequeId { get; set; } = default!;
        public string SheetSeqNumber { get; set; } = default!;
        public string SheetUniqueNumber { get; set; } = default!;
        public string SheetSeriNumber { get; set; } = default!;
        public decimal TotalCost { get; set; } = default!;
        public int? AccountReferenceId { get; set; } = default!;
        public int BankBranchId { get; set; } = default!;
        public DateTime? IssueDate { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public int? IssuerEmployeeId { get; set; }
        public string? Description { get; set; }
        public int ChequeTypeBaseId { get; set; }
        public string ChequeTypeTitle { get; set; }
        public bool? IsActive { get; set; } = default!;
        public int? BankId { get; set; }
        public string BankTitle { get; set; }
        public string BranchName { get; set; }
        public string? AccountNumber { get; set; }
        public int? ChequeSheetId { get; set; }
        public string Title { get; set; }
        public bool IsUsed { get; set; }
        public string CreditAccountReferenceTitle { get;   set; }
        public string CreditAccountReferenceCode { get;   set; }
        public string DebitAccountReferenceTitle { get;   set; }
        public string DebitAccountReferenceCode { get;   set; }
        public string CreditAccountReferenceGroupTitle { get;   set; }
        public string CreateName { get;  set; }
        public int CreditAccountHeadId { get; set; } = default!;
        public int? CreditAccountReferenceGroupId { get; set; }
        public int? CreditAccountReferenceId { get; set; }
        public int DebitAccountHeadId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; }
        public int? DebitAccountReferenceId { get; set; }
        public string FinancialRequestDescription { get; internal set; }
        public int? ChequeDocumentState { get; set; }
        public string ChequeDocumentStateTitle { get; set; }
        public int? FinancialRequestId { get; set; }
        public string IssueReferenceBankTitle { get;   set; }
        public int? IssueReferenceBankId { get;   set; }
        public int? VoucherHeadId { get; set; }
        public int? OwnerChequeReferenceId { get; set; }
        public string OwnerChequeReferenceTitle { get; set; }
        public string OwnerChequeReferenceCode { get;  set; }
        public string CreditAccountReferenceGroupCode { get; internal set; }
        public string CreditAccountHeadTitle { get; internal set; }
        public string CreditAccountHeadCode { get; internal set; }
        public string DebitAccountReferenceGroupCode { get; internal set; }
        public string DebitAccountReferenceGroupTitle { get; internal set; }
        public string DebitAccountHeadCode { get; internal set; }
        public string DebitAccountHeadTitle { get; internal set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ChequeSheets, ChequeSheetModel>()
                .ForMember(src => src.Title, opt => opt.MapFrom(x => x.SheetUniqueNumber + " " + x.Description + " " + x.TotalCost));
        }

        public void init()
        {
            ChequeDocumentStateTitle = GetChequeSateTitle((ChequeStateEnum)ChequeDocumentState);
            
            CreditAccountReferenceTitle = CreditAccountReferenceTitle== null ? "-": CreditAccountReferenceTitle + " (" + CreditAccountReferenceCode+")";
            CreditAccountReferenceGroupTitle = CreditAccountReferenceGroupTitle == null ? "-": CreditAccountReferenceGroupTitle + " (" + CreditAccountReferenceGroupCode+ ")";
            CreditAccountHeadTitle = CreditAccountHeadTitle + " (" + CreditAccountHeadCode+ ")";


            DebitAccountReferenceTitle = DebitAccountReferenceTitle == null ? "-": DebitAccountReferenceTitle + " (" + DebitAccountReferenceCode+")";
            DebitAccountReferenceGroupTitle = DebitAccountReferenceGroupTitle == null ? "-" : DebitAccountReferenceGroupTitle + " (" + DebitAccountReferenceGroupCode + ")";
            DebitAccountHeadTitle = DebitAccountHeadTitle + " (" + DebitAccountHeadCode + ")";

          //  ChequeTypeTitle = GetChequeTypeTitle((ChequeTypeEnum)ChequeTypeBaseId);
        }

        public string GetChequeSateTitle(ChequeStateEnum ChequeTypeBaseId) => ChequeTypeBaseId switch
        {
            ChequeStateEnum.ReceiveCheque => "دریافت",
            ChequeStateEnum.SendToBank => "ارسال به بانک",
            ChequeStateEnum.ChequeCashing => "وصول",
            ChequeStateEnum.ReturnCheque => "برگشت",
            ChequeStateEnum.RefundCheque => "عودت",
            _ => throw new ArgumentOutOfRangeException(nameof(ChequeTypeBaseId), $"Not expected direction value: {ChequeTypeBaseId}"),
        };

        //public string GetChequeTypeTitle(ChequeTypeEnum ChequeTypeTitle) => ChequeTypeTitle switch
        //{
        //    ChequeTypeEnum.GuaranteedCheque => "چک تضمینی",
        //    ChequeTypeEnum.CommonCheque => "چک عادی",
        //    ChequeTypeEnum.NonRialCheque => "چک ارزی",
        //    _ => throw new ArgumentOutOfRangeException(nameof(ChequeTypeTitle), $"Not expected direction value: {ChequeTypeTitle}"),
        //};


        public enum ChequeStateEnum:int
        {
            ReceiveCheque = 0,
            SendToBank = 1,
            ChequeCashing = 2,
            ReturnCheque = 3,
            RefundCheque = 4
        }


        public enum ChequeTypeEnum : int
        {
            GuaranteedCheque = 28542,
            CommonCheque = 28543,
            NonRialCheque = 28704,
     
        }


    }
}
