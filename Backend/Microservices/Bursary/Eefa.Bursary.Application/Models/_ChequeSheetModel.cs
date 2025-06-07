using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;

namespace Eefa.Bursary.Application 
{
   public class _ChequeSheetModel  :IMapFrom<ChequeSheets>
    {
        public int Id	 {get; set;}
        public int PayChequeId	 {get; set;}
        public int SheetSeqNumber	  {get; set;}
        public int SheetUniqueNumber	  {get; set;}
        public int SheetSeriNumber	  {get; set;}
        public decimal TotalCost	  {get; set;}
        public int AccountReferenceId	 {get; set;}
        public int BankBranchId	 {get; set;}
        public DateTime IssueDate	  {get; set;}
        public DateTime ReceiptDate	  {get; set;}
        public int IssuerEmployeeId	 {get; set;}
        public string Description	  {get; set;}
        public bool IsActive { get; set; }

        public void Mapping(Profile profile)
        {

            profile.CreateMap<Domain.Entities.ChequeSheets , _ChequeSheetModel >().IgnoreAllNonExisting();

        }

    }
}
