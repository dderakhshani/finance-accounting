using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Bursary.Application.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.Queries.ChequeSheet
{
    public class ChequeSheetQueries : IChequeSheetQueries
    {
        private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRequestRepository;
        private readonly IRepository<FinancialRequestDetails> _financialRequestDetailRepository;
        private readonly IRepository<Domain.Entities.ChequeSheets> _chequeSheetRepository;
        private readonly IRepository<AccountReferencesGroups> _accountReferenceGroup;
        private readonly IRepository<Domain.Entities.AccountReferences> _accountReference;
        private readonly IRepository<Domain.Entities.AccountHead> _accountHead;
 

        private readonly IRepository<PayCheque> _payChequeRepository;
        private readonly IRepository<BankAccounts> _bankAccountRepository;
        private readonly IRepository<BankBranches> _bankBranchRepository;
        private readonly IRepository<BaseValues> _baseValueRepository;
        private readonly IRepository<Banks> _bankRepository;
        private readonly IRepository<Users> _userRepository;
        private readonly IRepository<Persons> _personRepository;
     
        private readonly IMapper _mapper;

        public ChequeSheetQueries(IMapper mapper, IRepository<Domain.Entities.ChequeSheets> chequeSheetQueries,
            IRepository<PayCheque> payChequeRepository,
            IRepository<BankAccounts> bankAccountRepository,
            IRepository<BankBranches> bankBranchRepository, IRepository<BaseValues> baseValueRepository, IRepository<Banks> bankRepository, IRepository<Users> userRepository, IRepository<Persons> personRepository, IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRequestRepository, IRepository<FinancialRequestDetails> financialRequestDetailRepository, IRepository<AccountReferencesGroups> accountReferenceGroup, IRepository<AccountReferences> accountReference, IRepository<Domain.Entities.AccountHead> accountHead)
        {
            _mapper = mapper;
            _chequeSheetRepository = chequeSheetQueries;
            _payChequeRepository = payChequeRepository;
            _bankAccountRepository = bankAccountRepository;
            _bankBranchRepository = bankBranchRepository;
            _baseValueRepository = baseValueRepository;
            _bankRepository = bankRepository;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _financialRequestRepository = financialRequestRepository;
            _financialRequestDetailRepository = financialRequestDetailRepository;
            _accountReferenceGroup = accountReferenceGroup;
            _accountReference = accountReference;
            _accountHead = accountHead;
        }


        public async Task<PagedList<ChequeSheetModel>> GetAll(PaginatedQueryModel query)
        {

            var chequeSheets = await (from cs in _chequeSheetRepository.GetAll()
                                      join oar in _accountReference.GetAll() on cs.OwnerChequeReferenceId equals oar.Id into oarNull
                                      from oarr in oarNull.DefaultIfEmpty()
                                      join bv in _baseValueRepository.GetAll() on cs.ChequeTypeBaseId equals bv.Id
                                      join b in _bankRepository.GetAll() on cs.BankId equals b.Id
                                      where cs.IsDeleted != true
                                      select new ChequeSheetModel
                                      {
                                          Id = cs.Id,
                                          PayChequeId = cs.PayChequeId,
                                          SheetSeqNumber = cs.SheetSeqNumber,
                                          SheetUniqueNumber = cs.SheetUniqueNumber,
                                          SheetSeriNumber = cs.SheetSeriNumber,
                                          TotalCost = cs.TotalCost,
                                          BankBranchId = cs.BankBranchId,
                                          IssueDate = cs.IssueDate,
                                          ReceiptDate = cs.ReceiptDate,
                                          Description = cs.Description,
                                          ChequeTypeBaseId = cs.ChequeTypeBaseId,
                                          ChequeTypeTitle = bv.Title,
                                          IsActive = cs.IsActive,
                                          BankId = cs.BankId,
                                          BankTitle = b.Title,
                                          BranchName = cs.BranchName,
                                          AccountNumber = cs.AccountNumber,
                                          Title = cs.SheetSeqNumber + b.Title,
                                          IsUsed = cs.IsUsed,
                                          OwnerChequeReferenceId = oarr.Id,
                                          OwnerChequeReferenceTitle = oarr.Title,
                                          OwnerChequeReferenceCode = oarr.Code

                                      }).FilterQuery(query.Conditions).OrderByMultipleColumns(query.OrderByProperty).ToListAsync();


            var result = new PagedList<ChequeSheetModel>()
            {
                Data = chequeSheets,
                TotalCount = query.PageIndex <= 1 ? chequeSheets .Count() : 0
            };

            return result;
        }

        public async Task<PagedList<ChequeSheetModel>> GetUsedCheques(PaginatedQueryModel query)
        {
            //Stopwatch ts = new Stopwatch();
            //ts.Start();
            var chequeSheets = await (from cs in _chequeSheetRepository.GetAll()
                                      
                                      join frd in _financialRequestDetailRepository.GetAll() on cs.Id equals frd.ChequeSheetId
                                      join fr in _financialRequestRepository.GetAll() on frd.FinancialRequestId equals fr.Id

                                      join carr in _accountReference.GetAll() on frd.CreditAccountReferenceId equals carr.Id into carrNull
                                      from car in carrNull.DefaultIfEmpty()

                                      join darr in _accountReference.GetAll() on frd.DebitAccountReferenceId equals darr.Id into darrNull
                                      from dar in darrNull.DefaultIfEmpty()

                                      join bnkRc in _accountReference.GetAll() on cs.IssueReferenceBankId equals bnkRc.Id into bnkrcNULL
                                      from brc in bnkrcNULL.DefaultIfEmpty()

                                      join cargg in _accountReferenceGroup.GetAll() on frd.CreditAccountReferenceGroupId equals cargg.Id into carggNull
                                      from carg in carggNull.DefaultIfEmpty()

                                      join dargg in _accountReferenceGroup.GetAll() on frd.DebitAccountReferenceGroupId equals dargg.Id into darggNull
                                      from darg in darggNull.DefaultIfEmpty()

                                      join cah in _accountHead.GetAll() on frd.CreditAccountHeadId equals cah.Id
                                      join dah in _accountHead.GetAll() on frd.DebitAccountHeadId equals dah.Id

                                      join bv in _baseValueRepository.GetAll() on cs.ChequeTypeBaseId equals bv.Id
                                      join b in _bankRepository.GetAll() on cs.BankId equals b.Id
                                      join Cusr in _userRepository.GetAll() on cs.CreatedById equals Cusr.Id
                                      join Cprs in _personRepository.GetAll() on Cusr.PersonId equals Cprs.Id
                                      where cs.IsDeleted != true && frd.IsDeleted !=true && fr.IsDeleted != true
                                      select new ChequeSheetModel
                                      {
                                          VoucherHeadId = fr.VoucherHeadId,
                                          Id = cs.Id,
                                          PayChequeId = cs.PayChequeId,
                                          SheetSeqNumber = cs.SheetSeqNumber,
                                          SheetUniqueNumber = cs.SheetUniqueNumber,
                                          SheetSeriNumber = cs.SheetSeriNumber,
                                          TotalCost = cs.TotalCost,
                                          BankBranchId = cs.BankBranchId,
                                          IssueDate = cs.IssueDate,
                                          ReceiptDate = cs.ReceiptDate,
                                          Description = cs.Description,
                                          ChequeTypeBaseId = cs.ChequeTypeBaseId,
                                          ChequeTypeTitle = bv.Title,
                                          IsActive = cs.IsActive,
                                          BankId = cs.BankId,
                                          BankTitle = b.Title,
                                          BranchName = cs.BranchName,
                                          AccountNumber = cs.AccountNumber,
                                          Title = cs.SheetSeqNumber + b.Title,
                                          IsUsed = cs.IsUsed,

                                          CreditAccountReferenceTitle = car.Title,
                                          CreditAccountReferenceId = car.Id,
                                          CreditAccountReferenceCode = car.Code,

                                          CreditAccountReferenceGroupTitle = carg.Title,
                                          CreditAccountReferenceGroupId = carg.Id,
                                          CreditAccountReferenceGroupCode = carg.Code,

                                          CreditAccountHeadId = cah.Id,
                                          CreditAccountHeadTitle = cah.Code,
                                          CreditAccountHeadCode = cah.Title,


                                          DebitAccountReferenceTitle = dar.Title,
                                          DebitAccountReferenceId = dar.Id,
                                          DebitAccountReferenceCode = dar.Code,

                                          DebitAccountReferenceGroupId = darg.Id,
                                          DebitAccountReferenceGroupCode = darg.Code,
                                          DebitAccountReferenceGroupTitle = darg.Title,

                                          DebitAccountHeadId = dah.Id,
                                          DebitAccountHeadCode = dah.Code,
                                          DebitAccountHeadTitle = dah.Title,

                                          CreateName = Cprs.FirstName + " " + Cprs.LastName,
                                          FinancialRequestDescription = frd.Description,
                                          ChequeDocumentState = cs.ChequeDocumentState,
                                          FinancialRequestId = frd.FinancialRequestId,
                                          IssueReferenceBankTitle = brc.Title,
                                          IssueReferenceBankId = cs.IssueReferenceBankId

                                      }).FilterQuery(query.Conditions).OrderByMultipleColumns(query.OrderByProperty).ToListAsync();
            chequeSheets.ForEach(x => x.init());
            //ts.Stop();
            //Console.WriteLine(ts.ElapsedMilliseconds);
            var result = new PagedList<ChequeSheetModel>()
            {
                Data = chequeSheets,
                TotalCount = query.PageIndex <= 1 ? chequeSheets.Count() : 0,
                TotalSum = chequeSheets.Sum(x=>x.TotalCost)
            };
            return result;
        }
   
        public async Task<PagedList<ChequeSheetModel>> GetChequeSheetById(int chequeId)
        {

            var chequeSheets = await (from cs in _chequeSheetRepository.GetAll()
                                      join oar in _accountReference.GetAll() on cs.OwnerChequeReferenceId equals oar.Id into oarNull
                                      from oarr in oarNull.DefaultIfEmpty()
                                      join bv in _baseValueRepository.GetAll() on cs.ChequeTypeBaseId equals bv.Id
                                      join b in _bankRepository.GetAll() on cs.BankId equals b.Id
                                      where cs.Id == chequeId
                                      select new ChequeSheetModel
                                      {
                                          Id = cs.Id,
                                          PayChequeId = cs.PayChequeId,
                                          SheetSeqNumber = cs.SheetSeqNumber,
                                          SheetUniqueNumber = cs.SheetUniqueNumber,
                                          SheetSeriNumber = cs.SheetSeriNumber,
                                          TotalCost = cs.TotalCost,
                                          BankBranchId = cs.BankBranchId,
                                          IssueDate = cs.IssueDate,
                                          ReceiptDate = cs.ReceiptDate,
                                          Description = cs.Description,
                                          ChequeTypeBaseId = cs.ChequeTypeBaseId,
                                          ChequeTypeTitle = bv.Title,
                                          IsActive = cs.IsActive,
                                          BankId = cs.BankId,
                                          BankTitle = b.Title,
                                          BranchName = cs.BranchName,
                                          AccountNumber = cs.AccountNumber,
                                          Title = cs.SheetSeqNumber + b.Title,
                                          IsUsed = cs.IsUsed,
                                          OwnerChequeReferenceId = oarr.Id,
                                          OwnerChequeReferenceTitle = oarr.Title,
                                          OwnerChequeReferenceCode = oarr.Code

                                      }).ToListAsync();


            var result = new PagedList<ChequeSheetModel>()
            {
                Data = chequeSheets,
                TotalCount =   chequeSheets.Count() 
            };

            return result;



        }
    
    }
}

 





 