using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;

namespace Eefa.Bursary.Application.Queries.FinancialRequest
{
   public class _ChequeSheetModel :IMapFrom<Domain.Entities.ChequeSheet>
    {
        public int? PayChequeId { get; set; } = default!;

        /// <summary>
        /// شماره سریال چک
        /// </summary>
        public string SheetSeqNumber { get; set; } = default!;

        /// <summary>
        /// شماره چک
        /// </summary>
        public string SheetUniqueNumber { get; set; } = default!;

        /// <summary>
        /// شماره سری چک 
        /// </summary>
        public string SheetSeriNumber { get; set; } = default!;
        public decimal TotalCost { get; set; } = default!;
        public int? AccountReferenceId { get; set; } = default!;
        public int BankBranchId { get; set; } = default!;

        /// <summary>
        /// تاریخ صدور
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// تاریخ سر رسید 
        /// </summary>
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// صادر کننده چک 
        /// </summary>
        public int? IssuerEmployeeId { get; set; }
        public string? Description { get; set; }

        /// <summary>
        /// آیا فعال است 
        /// </summary>
        public bool? IsActive { get; set; } = default!;

        public string? BranchName { get; set; }

        public int? OwnerChequeReferenceId { get; set; }
        public int? OwnerChequeReferenceGroupId { get; set; }

        public int? ReceiveChequeReferenceId { get; set; }
        public int? ReceiveChequeReferenceGroupId { get; set; }
        public bool? ApproveReceivedChequeSheet { get; set; }
        public int ChequeTypeBaseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ChequeSheet, _ChequeSheetModel>();
        }

    }
}
