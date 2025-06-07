using System.Linq;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Eefa.Bursary.Domain.Entities;
using System;
using System.Collections.Generic;
using Eefa.Bursary.Application.Models;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.Data;
using System.Threading;
using Eefa.Common.Data.Query;
using Eefa.Common.Exceptions;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using Eefa.Bursary.Domain.Entities.Definitions;

namespace Eefa.Bursary.Application.Queries.FinancialRequest
{
    public class FinancialRequestQueries : IFinancialRequestQueries
    {
        private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRequestRepository;
        private readonly IRepository<CodeVoucherGroups> _codeVoucherGroupRepository;
        private readonly IRepository<BaseValues> _baseValueRepository;
        private readonly IRepository<Domain.Entities.AccountHead> _accountHead;
        private readonly IRepository<AccountReferencesGroups> _accountReferenceGroup;
        private readonly IRepository<Domain.Entities.AccountReferences> _accountReference;
        private readonly IRepository<FinancialRequestDetails> _financialRequestDetailRepository;
        private readonly IRepository<Domain.Entities.ChequeSheets> _chequeSheetRepository;
        private readonly IRepository<FinancialRequestAttachments> _financialAttachmentRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<Years> _yearRepository;
        private readonly IRepository<VouchersHead> _voucherHeadRepository;
        private readonly IRepository<Users> _userRepository;
        private readonly IRepository<Persons> _personRepository;
        private readonly IRepository<Banks> _bankRepository;
        private readonly IRepository<MapDanaAndTadbir> _mapDanaAndTadbir;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserAccessor _currenUserAccessor;
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _bursaryUnitOfWork;
        private readonly ILogger<FinancialRequestQueries> _logger;

        public FinancialRequestQueries(IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRequestRepository,
            IRepository<BaseValues> baseValueRepository,
            IRepository<Domain.Entities.AccountHead> accountHead,
            IRepository<AccountReferencesGroups> accountReferenceGroup,
            IRepository<Domain.Entities.AccountReferences> accountReference,
            IRepository<FinancialRequestDetails> financialRequestDetailRepository,
            IMapper mapper,
            IRepository<Domain.Entities.ChequeSheets> chequeSheetRepository,
            IRepository<Years> yearRepository,
            IRepository<CodeVoucherGroups> codeVoucherGroupRepository,
            IRepository<FinancialRequestAttachments> financialAttachmentRepository,
            IRepository<Attachment> attachmentRepository,
            IRepository<VouchersHead> voucherHeadRepository,
            IRepository<Users> userRepository,
            IRepository<Persons> personRepository, IRepository<Banks> bankRepository,
            IRepository<MapDanaAndTadbir> mapDanaAndTadbir,
            IMemoryCache memoryCache,
            IConfiguration configuration,
            ICurrentUserAccessor currenUserAccessor,
            IBursaryUnitOfWork bursaryUnitOfWork, ILogger<FinancialRequestQueries> logger)
        {
            _financialRequestRepository = financialRequestRepository;
            _baseValueRepository = baseValueRepository;
            _accountHead = accountHead;
            _accountReferenceGroup = accountReferenceGroup;
            _accountReference = accountReference;
            _financialRequestDetailRepository = financialRequestDetailRepository;
            _mapper = mapper;
            _chequeSheetRepository = chequeSheetRepository;
            _codeVoucherGroupRepository = codeVoucherGroupRepository;
            _financialAttachmentRepository = financialAttachmentRepository;
            _attachmentRepository = attachmentRepository;
            _yearRepository = yearRepository;
            _voucherHeadRepository = voucherHeadRepository;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _bankRepository = bankRepository;
            _mapDanaAndTadbir = mapDanaAndTadbir;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _currenUserAccessor = currenUserAccessor;
            _bursaryUnitOfWork = bursaryUnitOfWork;
            _logger = logger;
        }


        public async Task<PagedList<FinancialRequestModel>> GetAll(Eefa.Common.Data.Query.PaginatedQueryModel query)
        {

             var financialIds =await _financialRequestRepository.GetAll().Select(x => x.Id).OrderByDescending(x=>x).Take(200).ToListAsync();
            
             var Exec = await ExecuteSpUpdateVoucherHead(financialIds);

            //var requestsQuery = (from fr in _financialRequestRepository.GetAll()
            //                     join Cusr in _userRepository.GetAll() on fr.CreatedById equals Cusr.Id
            //                     join Cprs in _personRepository.GetAll() on Cusr.PersonId equals Cprs.Id
            //                     join frd in _financialRequestDetailRepository.GetAll() on fr.Id equals frd.FinancialRequestId
            //                     join dtbv in _baseValueRepository.GetAll() on frd.DocumentTypeBaseId equals dtbv.Id
            //                     join carr in _accountReference.GetAll() on frd.CreditAccountReferenceId equals carr.Id into carNULL
            //                     from car in carNULL.DefaultIfEmpty()
            //                     join darr in _accountReference.GetAll() on frd.DebitAccountReferenceId equals darr.Id into darrNull
            //                     from dar in darrNull.DefaultIfEmpty()
            //                     join cargg in _accountReferenceGroup.GetAll() on frd.CreditAccountReferenceGroupId equals cargg.Id into carggNull
            //                     from carg in carggNull.DefaultIfEmpty()
            //                     join cv in _codeVoucherGroupRepository.GetAll() on fr.CodeVoucherGroupId equals cv.Id
            //                     join bs in _baseValueRepository.GetAll() on fr.PaymentTypeBaseId equals bs.Id
            //                     join vh in _voucherHeadRepository.GetAll() on fr.VoucherHeadId equals vh.Id into vhNull
            //                     from vHead in vhNull.DefaultIfEmpty()
            //                     orderby fr.Id descending
            //                     where fr.YearId == _currenUserAccessor.GetYearId() && frd.Description != null
            //                     select new FinancialRequestModel
            //                     {
            //                         Id = fr.Id,
            //                         DocumentNo = fr.DocumentNo,
            //                         Amount = frd.Amount,
            //                         Description = fr.Description,
            //                         DetailDescription = frd.Description,
            //                         CodeVoucherGroupId = cv.Id,
            //                         IssueDate = fr.IssueDate,
            //                         PaymentTypeBaseId = fr.PaymentTypeBaseId,
            //                         PaymentTypeBaseTitle = bs.Title,
            //                         DocumentDate = fr.DocumentDate,
            //                         VoucherHeadId = fr.VoucherHeadId,
            //                         CreditAccountReferenceTitle = car.Title,
            //                         CreditAccountReferenceCode = car.Code,
            //                         DebitAccountReferenceTitle = dar.Title,
            //                         DebitAccountReferenceCode = dar.Code,
            //                         CreditAccountReferenceGroupTitle = carg.Title,
            //                         VoucherHeadCode = vHead != null ? vHead.VoucherNo : null,
            //                         CreateName = Cprs.FirstName + " " + Cprs.LastName,
            //                         VoucherStateId = vHead.VoucherStateId,
            //                         TotalAmount = fr.TotalAmount,
            //                         documentTypeBaseTitle = dtbv.Title

            //                     }).FilterQuery(query.Conditions).OrderByMultipleColumns(query.OrderByProperty);


            //    var requestsQuery = _financialRequestRepository.GetAll().Include(x=>x.FinancialRequestDetails).ThenInclude(x=>x.CreditAccountReference).ProjectTo<FinancialRequestModel>(_mapper.ConfigurationProvider).FilterQuery(query.Conditions).OrderByMultipleColumns(query.OrderByProperty);

            //var count = requestsQuery.Select(x => x.Id).Distinct().Count();




            //   var requestsIds = requests.Select(x => x.Id).ToList();

            // get list of details


            var details =   (from frd in _financialRequestDetailRepository.GetAll()
                                 join fr in _financialRequestRepository.GetAll() on frd.FinancialRequestId equals fr.Id
                                 join bs in _baseValueRepository.GetAll() on fr.PaymentTypeBaseId equals bs.Id
                                 join vh in _voucherHeadRepository.GetAll() on fr.VoucherHeadId equals vh.Id into vhNull
                                 from vHead in vhNull.DefaultIfEmpty()
                                 join Cusr in _userRepository.GetAll() on frd.CreatedById equals Cusr.Id
                                 join Cprs in _personRepository.GetAll() on Cusr.PersonId equals Cprs.Id
                                 join dtbv in _baseValueRepository.GetAll() on frd.DocumentTypeBaseId equals dtbv.Id
                                 join Musr in _userRepository.GetAll() on frd.ModifiedById equals Musr.Id into MusrNULL
                                 from modifyUser in MusrNULL.DefaultIfEmpty()
                                 join Mprs in _personRepository.GetAll() on modifyUser.PersonId equals Mprs.Id into MprsNULL
                                 from modityPerson in MprsNULL.DefaultIfEmpty()
                                 join cah in _accountHead.GetAll() on frd.CreditAccountHeadId equals cah.Id
                                 join cargg in _accountReferenceGroup.GetAll() on frd.CreditAccountReferenceGroupId equals cargg.Id into carggNull
                                 from carg in carggNull.DefaultIfEmpty()
                                 join carr in _accountReference.GetAll() on frd.CreditAccountReferenceId equals carr.Id into carrNull
                                 from car in carrNull.DefaultIfEmpty()
                                 join dah in _accountHead.GetAll() on frd.DebitAccountHeadId equals dah.Id
                                 join dargg in _accountReferenceGroup.GetAll() on frd.DebitAccountReferenceGroupId equals dargg.Id into darggNull
                                 from darg in darggNull.DefaultIfEmpty()
                                 join darr in _accountReference.GetAll() on frd.DebitAccountReferenceId equals darr.Id into darrNull
                                 from dar in darrNull.DefaultIfEmpty()
                                 join frt in _baseValueRepository.GetAll() on frd.FinancialReferenceTypeBaseId equals frt.Id
                                 join cs in _chequeSheetRepository.GetAll() on frd.ChequeSheetId equals cs.Id into csNull
                                 from cqs in csNull.DefaultIfEmpty()
                                 where fr.YearId == _currenUserAccessor.GetYearId() && fr.CodeVoucherGroupId == 2259
                                 select new FinancialRequestDetailModel
                                 {
                                     DetailId = frd.Id,
                                     Id = fr.Id,
                                     ChequeSheetId = frd.ChequeSheetId,
                                     SheetUniqueNumber = cqs.SheetUniqueNumber,
                                     SheetSeqNumber = cqs.SheetSeqNumber,
                                     Amount = frd.Amount,
                                     PaymentTypeBaseTitle = bs.Title,
                                     DocumentNo = fr.DocumentNo,
                                     CreditAccountHeadId = frd.CreditAccountHeadId,
                                     CreditAccountReferenceGroupId = frd.CreditAccountReferenceGroupId,
                                     CreditAccountReferenceId = frd.CreditAccountReferenceId,
                                     CodeVoucherGroupId = fr.CodeVoucherGroupId,
                                     CreditAccountHeadTitle = cah.Title,
                                     CreditAccountReferenceGroupTitle = carg.Title,
                                     CreditAccountReferenceGroupCode = carg.Code,
                                     CreditAccountReferenceTitle = car.Title,
                                     CreditAccountReferenceCode = car.Code,
                                     VoucherHeadCode = vHead != null ? vHead.VoucherNo : null,
                                     CreateName = Cprs.FirstName + " " + Cprs.LastName,
                                     ModifyName = modityPerson.FirstName + " " + modityPerson.LastName,
                                     DebitAccountHeadId = frd.DebitAccountHeadId,
                                     DebitAccountReferenceGroupId = frd.DebitAccountReferenceGroupId,
                                     DebitAccountReferenceId = frd.DebitAccountReferenceId,
                                     VoucherHeadId = fr.VoucherHeadId,
                                     DebitAccountHeadTitle = dah.Title,
                                     DebitAccountReferenceGroupTitle = darg.Title,
                                     DebitAccountReferenceGroupCode = darg.Code,
                                     DebitAccountReferenceTitle = dar.Title,
                                     DebitAccountReferenceCode = dar.Code,
                                     IssueDate = fr.IssueDate,
                                     DocumentDate = fr.DocumentDate,
                                     DocumentTypeBaseId = frd.DocumentTypeBaseId,
                                     CurrencyTypeBaseId = frd.CurrencyTypeBaseId,
                                     CurrencyAmount = frd.CurrencyAmount,
                                     Description = fr.Description,
                                     CurrencyFee = frd.CurrencyFee,
                                     FinancialRequestId = frd.FinancialRequestId,
                                     DetailDescription = frd.Description,
                                     FinancialReferenceTypeBaseId = frd.FinancialReferenceTypeBaseId,
                                     FinancialReferenceTypeBaseTitle = frt.Title,
                                     IsRial = frd.IsRial,
                                     PaymentTypeBaseId = fr.PaymentTypeBaseId,
                                     NonRialStatus = frd.NonRialStatus,
                                     VoucherStateId = vHead.VoucherStateId,
                                     DocumentTypeBaseTitle = dtbv.Title,
                                     AutomateState = fr.AutomateState,
                                     BedCurrencyStatus = frd.BedCurrencyStatus,
                                     BesCurrencyStatus = frd.BesCurrencyStatus,
                                     
                                 }).FilterQuery(query.Conditions).OrderByMultipleColumns(query.OrderByProperty);


            var count = await details.Select(x => x.Id).Distinct().CountAsync();
            var summ = await details.SumAsync(x => x.Amount);


       
            var requests = await details.Paginate(query.Paginator()).ToListAsync();
                // کوئری اصلی شما


            requests.Distinct();

            var distinctIds = details.Select(x => x.Id).Distinct().ToList();
            //var sum = details.Where(x => distinctIds.Contains(x.Id)).Sum(x => x.Amount);

          


            var groupedItems = requests.Select(group =>
                   {
               
                       return new FinancialRequestModel
                       {
                           Amount = (decimal)group.Amount,
                           CreditAccountReferenceTitle = group.CreditAccountReferenceTitle,
                           DebitAccountReferenceTitle = group.DebitAccountReferenceTitle,
                           CreditAccountReferenceGroupTitle = group.CreditAccountReferenceGroupTitle,
                           Id = group.Id,
                           DocumentNo = (int)group.DocumentNo,
                           Description = group.Description,
                           DetailDescription = group.DetailDescription,
                           CodeVoucherGroupId = group.CodeVoucherGroupId,
                           IssueDate = group.IssueDate,
                           PaymentTypeBaseId = group.PaymentTypeBaseId,
                           PaymentTypeBaseTitle = group.PaymentTypeBaseTitle,
                           DocumentDate = group.DocumentDate,
                           VoucherHeadId = group.VoucherHeadId,
                           CreditAccountReferenceCode = group.CreditAccountReferenceCode,
                           DebitAccountReferenceCode = group.DebitAccountReferenceCode,
                           VoucherHeadCode = group.VoucherHeadCode,
                           CreateName = group.CreateName,
                           VoucherStateId = group.VoucherStateId,
                      //     FinancialRequestDetails = group.FinancialRequestDetails,
                           DocumentTypeBaseTitle = group.DocumentTypeBaseTitle,
                           CreditAccountReferenceGroupCode= group.CreditAccountReferenceGroupCode,
                           CurrencyTypeBaseId = group.CurrencyTypeBaseId,
                           CurrencyAmount = group.CurrencyAmount,
                           CurrencyFee=group.CurrencyFee,
                           AutomateState = group.AutomateState,
                           CreditAccountReferenceGroupId = group.CreditAccountReferenceGroupId,
                           CreditAccountHeadId = group.CreditAccountHeadId,
                           CreditAccountReferenceId = group.CreditAccountReferenceId,
                           DebitAccountReferenceId = group.DebitAccountReferenceId,
                           DebitAccountHeadId = group.DebitAccountHeadId,
                           DebitAccountReferenceGroupId = group.DebitAccountReferenceGroupId,
                           DocumentTypeBaseId = group.DocumentTypeBaseId,
                           BesCurrencyStatus = group.BesCurrencyStatus,
                           BedCurrencyStatus    = group.BedCurrencyStatus,
                       };
                   }).ToList();


            var baseValues = _baseValueRepository.GetAll().Where(x => x.BaseValueTypeId == 308).ToList();


            foreach (var item in groupedItems)
                item.CurrencyTypeBaseTitle = baseValues.Where(x => x.Id == item.CurrencyTypeBaseId).Select(x => x.Title).FirstOrDefault();
            #region This lines are 
            //var requestsDictionary = requests.ToDictionary(item => item.Id, _ => false);

            //foreach (var groupedItem in groupedItems)
            //{
            //    if (requestsDictionary.ContainsKey(groupedItem.Id))
            //    {
            //        requestsDictionary[groupedItem.Id] = true;
            //    }
            //}

            //var missingItems = requestsDictionary.Where(pair => !pair.Value).Select(pair => pair.Key).ToList();

            #endregion


            var sortGroupITems = new List<FinancialRequestModel>();
            if (query.OrderByProperty == "Id DESC")
            sortGroupITems.AddRange(groupedItems.OrderByDescending(x => x.DocumentDate));

            return new PagedList<FinancialRequestModel>()
            {
                Data = groupedItems,
                TotalCount = query.PageIndex <= 1 ?   count : 0,
                TotalSum = summ
            };
        
        }

        public async Task<FinancialRequestModel> GetById(int id)
        {
            var request = await _financialRequestRepository.GetAll().Where(x => x.Id == id).Include(x => x.VoucherHead).SingleAsync();
            var attachments = await (from fa in _financialAttachmentRepository.GetAll()
                                     join att in _attachmentRepository.GetAll() on fa.AttachmentId equals att.Id
                                     where fa.FinancialRequestId == id
                                     select new FinancialAttachmentModel
                                     {
                                         AddressUrl = att.Url,
                                         AttachmentId = fa.AttachmentId,
                                         Id = fa.Id,
                                         FinancialRequestId = fa.FinancialRequestId,
                                         isVerified = fa.IsVerified
                                     }).ToListAsync();

            var details = _financialRequestDetailRepository.GetAll().Where(x => x.FinancialRequestId == id);
            
            request.FinancialRequestDetails = await details.ToListAsync();

            var chequeSheets = new List<ChequeSheetModel>();

            foreach (var item in request.FinancialRequestDetails)
                if (item.ChequeSheetId != null)
                      chequeSheets.Add(GetChequeDetailByChequeId((int)item.ChequeSheetId));
                 
           

            var result = _mapper.Map<FinancialRequestModel>(request);
            if (chequeSheets.Count>0)
            foreach (var item in result.FinancialRequestDetails)
                item.ChequeSheet = chequeSheets.Where(x => x.Id == item.ChequeSheetId).First();

            result.FinancialRequestAttachments = attachments;
            return result;
        }

        public async Task<PagedList<FinancialRequestDetailModel>> GetDetailsByFinancialRequestId(int financialRequestId)
        {

            var details = await (from frd in _financialRequestDetailRepository.GetAll()
                                 join cah in _accountHead.GetAll() on frd.CreditAccountHeadId equals cah.Id
                                 join carg in _accountReferenceGroup.GetAll() on frd.CreditAccountReferenceGroupId equals carg.Id
                                 join car in _accountReference.GetAll() on frd.CreditAccountReferenceId equals car.Id
                                 join dah in _accountHead.GetAll() on frd.DebitAccountHeadId equals dah.Id
                                 join darg in _accountReferenceGroup.GetAll() on frd.DebitAccountReferenceGroupId equals darg.Id
                                 join dar in _accountReference.GetAll() on frd.DebitAccountReferenceId equals dar.Id
                                 join frt in _baseValueRepository.GetAll() on frd.FinancialReferenceTypeBaseId equals frt.Id
                                 join cs in _chequeSheetRepository.GetAll() on frd.ChequeSheetId equals cs.Id into csNull
                                 from cqs in csNull.DefaultIfEmpty()

                                 where financialRequestId == frd.FinancialRequestId
                                 select new FinancialRequestDetailModel
                                 {
                                     Id = frd.Id,
                                     ChequeSheetId = frd.ChequeSheetId,
                                     SheetUniqueNumber = cqs.SheetUniqueNumber,
                                     Amount = frd.Amount,

                                     CreditAccountHeadId = frd.CreditAccountHeadId,
                                     CreditAccountReferenceGroupId = frd.CreditAccountReferenceGroupId,
                                     CreditAccountReferenceId = frd.CreditAccountReferenceId,

                                     CreditAccountHeadTitle = cah.Title,
                                     CreditAccountReferenceGroupTitle = carg.Title,
                                     CreditAccountReferenceGroupCode = carg.Code,
                                     CreditAccountReferenceTitle = car.Title,
                                     CreditAccountReferenceCode = car.Code,


                                     DebitAccountHeadId = frd.DebitAccountHeadId,
                                     DebitAccountReferenceGroupId = frd.DebitAccountReferenceGroupId,
                                     DebitAccountReferenceId = frd.DebitAccountReferenceId,

                                     DebitAccountHeadTitle = dah.Title,
                                     DebitAccountReferenceGroupTitle = darg.Title,
                                     DebitAccountReferenceGroupCode = darg.Code,
                                     DebitAccountReferenceTitle = dar.Title,
                                     DebitAccountReferenceCode = dar.Code,


                                     DocumentTypeBaseId = frd.DocumentTypeBaseId,
                                     CurrencyTypeBaseId = frd.CurrencyTypeBaseId,
                                     CurrencyAmount = frd.CurrencyAmount,

                                     CurrencyFee = frd.CurrencyFee,
                                     FinancialRequestId = frd.FinancialRequestId,
                                     Description = frd.Description,
                                     FinancialReferenceTypeBaseId = frd.FinancialReferenceTypeBaseId,
                                     FinancialReferenceTypeBaseTitle = frt.Title,
                                     IsRial = frd.IsRial,
                                     NonRialStatus = frd.NonRialStatus

                                 }).ToListAsync();

            return new PagedList<FinancialRequestDetailModel>()
            {
                Data = details,
                TotalCount = details.Count(),
                TotalSum = details.Sum(x => x.Amount)
            };

        }
   
        public async Task<PagedList<FinancialAttachmentModel>>GetAttachmentsByFinancialRequestId(int financialRequestId)
        {
            var attachemtns = await (from fa in _financialAttachmentRepository.GetAll()
                                     join att in _attachmentRepository.GetAll() on fa.AttachmentId equals att.Id
                                     where  fa.FinancialRequestId == financialRequestId
                                     select new FinancialAttachmentModel
                                     {
                                         Id = fa.Id,
                                         AddressUrl = att.Url,
                                         AttachmentId = fa.AttachmentId,
                                         FinancialRequestId = fa.FinancialRequestId,
                                         isVerified = fa.IsVerified,
                                         
                                     }).ToListAsync();

            return new PagedList<FinancialAttachmentModel>()
            {
                Data = attachemtns,
                TotalCount = attachemtns.Count,
                TotalSum = 0
            };
           
        }

        public async Task<FinancialRequestModel> GetLastReceiptInfo()
        {
            var lastReceipt = await (from fr in _financialRequestRepository.GetAll()
                                     join u in _userRepository.GetAll() on fr.CreatedById equals u.Id
                                     join p in _personRepository.GetAll() on u.PersonId equals p.Id
                                     join fd in _financialRequestDetailRepository.GetAll() on fr.Id equals fd.FinancialRequestId
                                     join ar in _accountReference.GetAll() on fd.CreditAccountReferenceId equals ar.Id
                                     join vh in _voucherHeadRepository.GetAll() on fr.VoucherHeadId equals vh.Id
                                     where fr.VoucherHeadId != null && fr.IsDeleted != true
                                     orderby fd.Id descending
                                     select new FinancialRequestModel
                                     {
                                         CreditAccountReferenceTitle = ar.Title,
                                         Amount = fd.Amount,
                                         VoucherHeadCode = vh.VoucherNo,
                                         CreateName = p.FirstName + " "+ p.LastName,

                                     }).FirstAsync();
            return lastReceipt;
        }

        public async Task<List<int>> SetDocumentsForBursaryPaymentArticles(SendDocument model)
        {

            var accountReferenceCacheKey = "AccountReferenceCacheKey";
            var accountReferenceGroupCacheKey = "AccountReferenceGroupCacheKey";
            var accountHeadCacheKey = "AccountHeadCacheKey";

            if (!_memoryCache.TryGetValue(accountReferenceCacheKey, out List<AccountReferences> accountReferences))
            {
                accountReferences = await  _accountReference.GetAll().ToListAsync();
                _memoryCache.Set(accountReferenceCacheKey, accountReferences, TimeSpan.FromHours(2));

            }
            if (!_memoryCache.TryGetValue(accountReferenceGroupCacheKey, out List<AccountReferencesGroups> accountReferenceGroups))
            {
                accountReferenceGroups = await _accountReferenceGroup.GetAll().ToListAsync();
                _memoryCache.Set(accountReferenceGroupCacheKey, accountReferenceGroups, TimeSpan.FromHours(2));

            }
            if (!_memoryCache.TryGetValue(accountHeadCacheKey, out List<MapDanaAndTadbir> accountHeads))
            {
                accountHeads = await _mapDanaAndTadbir.GetAll().ToListAsync();
                _memoryCache.Set(accountHeadCacheKey, accountHeads, TimeSpan.FromHours(2));
            }

            var dataList = new List<BursaryDocumentModel>();
            foreach (var item in model.dataList)
            {



                var payment = new BursaryDocumentModel();

                payment.DebitAccountReferenceId = accountReferences.Where(x => x.Code.Equals(item.DebitAccountReferenceId)).Select(x => x.Id).SingleOrDefault();

                if (payment.DebitAccountReferenceId == null)
                    _logger.LogInformation("تفصیل بدهکار وجود ندارد " + item.DebitAccountReferenceId.ToString() + " موجود نیست ");

                if (!item.AccountIsFloat)
                {
                    payment.DebitAccountReferenceGroupId = accountReferenceGroups.Where(x => x.Code.Equals(item.DebitAccountReferenceId.Substring(0, 5))).Select(x => x.Id).SingleOrDefault(); // payment.DebitAccountReferenceId;
                    
                    if (payment.DebitAccountReferenceGroupId == null)
                        _logger.LogInformation(" گروه تفصیل بدهکار در سطح معین وجود ندارد " + item.DebitAccountReferenceId.Substring(0, 5).ToString() + " موجود نیست ");


                    payment.DebitAccountHeadId = accountHeads.Where(x => x.TadbirCode.Equals(item.DebitAccountReferenceId.Substring(0, 5))).Select(x => x.DanaID).SingleOrDefault();

                    if (payment.DebitAccountHeadId == null || payment.DebitAccountHeadId == 0)
                        _logger.LogInformation(" سرفصل بدهکار در سط معین وجود ندارد " + item.DebitAccountReferenceId.Substring(0, 5).ToString() + " موجود نیست ");


                }
                else { 
                    payment.DebitAccountReferenceGroupId = accountReferenceGroups.Where(x => x.Code.Equals(item.DebitAccountReferenceGroupId.Substring(0, 5))).Select(x => x.Id).SingleOrDefault();
                    if (payment.DebitAccountReferenceGroupId == null)
                        _logger.LogInformation(" گروه تفصیل بدهکار وجود ندارد " + item.DebitAccountReferenceId.Substring(0, 5).ToString() + " موجود نیست ");


                    payment.DebitAccountHeadId = accountHeads.Where(x => x.TadbirCode.Equals(item.DebitAccountReferenceGroupId.Substring(0, 5))).Select(x => x.DanaID).SingleOrDefault();
                    if (payment.DebitAccountHeadId == null || payment.DebitAccountHeadId == 0)
                        _logger.LogInformation(" گروه تفصیل بدهکار وجود ندارد " + item.DebitAccountReferenceGroupId.Substring(0, 5).ToString() + " موجود نیست ");


                }

                payment.CreditAccountReferenceId = accountReferences.Where(x => x.Code.Equals(item.CreditAccountReferenceId)).Select(x => x.Id).SingleOrDefault();
                payment.CreditAccountReferenceGroupId = accountReferenceGroups.Where(x => x.Code.Equals(item.CreditAccountReferenceId.Substring(0,5))).Select(x => x.Id).SingleOrDefault();
                payment.CreditAccountHeadId = accountHeads.Where(x => x.TadbirCode.Equals(item.CreditAccountReferenceId.Substring(0, 5))).Select(x => x.DanaID).SingleOrDefault();
                payment.Amount = item.Amount;
                payment.ChequeSheetId = item.ChequeSheetId;
                payment.CodeVoucherGroupId = item.CodeVoucherGroupId;
                payment.CurrencyAmount = item.CurrencyAmount;
                payment.CurrencyFee = item.CurrencyFee;
                payment.CurrencyTypeBaseId = GetCurrencyTypeBaseId(item.CurrencyTypeBaseId);
                payment.Description = item.Description ;
                payment.DocumentDate = item.DocumentDate;
                payment.DocumentId = item.DocumentId;
                payment.DocumentNo = item.DocumentNo;
                payment.DocumentTypeBaseId = GetDocumentTypeBaseId(item.CurrencyTypeBaseId);
                payment.IsRial = item.IsRial;
                payment.NonRialStatus = item.NonRialStatus;
                payment.ReferenceCode = item.DebitAccountReferenceId;
                payment.ReferenceName = "";
                payment.SheetUniqueNumber = "";
                payment.TotalAmount += item.Amount;


                if (item.AccountIsFloat == true && (payment.DebitAccountReferenceId == null || payment.DebitAccountReferenceId == 0))
                {
                    throw new ValidationError("کد تفصیل بدهکار پیدا نشد. لطفا با واحد نرم افزار تماس بگیرید");
                }

                if (payment.DebitAccountReferenceGroupId == null || payment.DebitAccountReferenceGroupId == 0)
                {
                    throw new ValidationError("گروه بدهکار پیدا نشد. لطفا با واحد نرم افزار تماس بگیرید");
                }
                if (payment.DebitAccountHeadId == null || payment.DebitAccountHeadId == 0)
                {
                    throw new ValidationError("سرفصل بدهکار پیدا نشد. لطفا با واحد نرم افزار تماس بگیرید");
                }



                if (payment.CreditAccountReferenceId == null || payment.CreditAccountReferenceId == 0)
                {
                    throw new ValidationError("کد تفصیل بستانکار پیدا نشد. لطفا با واحد نرم افزار تماس بگیرید");
                }

                if (payment.CreditAccountReferenceGroupId == null || payment.CreditAccountReferenceGroupId == 0)
                {
                    throw new ValidationError("گروه بستانکار پیدا نشد. لطفا با واحد نرم افزار تماس بگیرید");
                }
                if (payment.CreditAccountHeadId == null || payment.CreditAccountHeadId == 0)
                {
                    throw new ValidationError("سرفصل بستانکار پیدا نشد. لطفا با واحد نرم افزار تماس بگیرید");
                }

                dataList.Add(payment);

            }



            var settings = _configuration.GetSection("AccountingConnectionDetails");
            var token = settings.GetValue<string>("token");

            if (token == null)
                _logger.LogInformation("توکن خالی است");

            if (settings == null)
                _logger.LogInformation("لیست ادرس ها خالی است");

            var urlAddress = settings.GetValue<string>("add_url");
            _logger.LogInformation("آدرس : "+ urlAddress);
            if (urlAddress == null)
                _logger.LogInformation(" ادرس خالی است");

#if DEBUG
            urlAddress = settings.GetValue<string>("add_url_dev");
#endif

            string danaToken = token;



            using (var httpClient = new HttpClient())
            {
               var voucherResponse = new List<int>();
                try
                {
                    var requestUri = new Uri(urlAddress);


                    var requestModel = new AddAutoVoucherModel
                    {
                        dataList = dataList,
                        menueId = 0
                   
                    };
                    var response = new HttpResponseMessage();
                    try { 

                    var jsonRequestData = JsonSerializer.Serialize(requestModel);

                    var httpContent = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", danaToken);

                      response = await httpClient.PostAsync(requestUri, httpContent);


                    var result = await response.Content.ReadAsStringAsync();
                    var jObj = JObject.Parse(result);

                    if (response.IsSuccessStatusCode)
                        voucherResponse = jObj.SelectToken("objResult.voucherHeadId").ToObject<List<int>>();
                    else
                        voucherResponse = new List<int>();

                    }
                    catch(Exception e)
                    {
                        _logger.LogInformation(e.Message);
                    }
                    if (response != null && response.IsSuccessStatusCode)
                    {
 
                        if (voucherResponse != null)
                        {
                            
                            if (voucherResponse.Count>0) return voucherResponse;
                            else {
                                _logger.LogInformation("سند حسابداری هیچ خروجی ندارد");
                                throw new ValidationError("Error: Result Count is 0");
                            
                            }
                        }
                        else
                        {
                            _logger.LogInformation("سند حسابداری هیچ خروجی ندارد");
                            throw new ValidationError("Response content was empty");
                       
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Call to API failed with status code {response?.StatusCode}");
                        throw new ValidationError($"Call to API failed with status code {response?.StatusCode}");
                     
                    }
                }
                catch (ArgumentNullException ex)
                {
                    _logger.LogInformation("Error: URL address is null or empty");
                    throw new ValidationError("Error: URL address is null or empty");
                }
                catch (UriFormatException ex)
                {
                    _logger.LogInformation("Error: Invalid URL format");
                    throw new ValidationError("Error: Invalid URL format");
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogInformation($"Error: {ex.Message}");
                    throw new ValidationError($"Error: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    _logger.LogInformation($"Error: {ex.Message}");
                    throw new ValidationError($"Error: {ex.Message}");
                }
            }
 

        }

        public async Task<int> GetReqeustCountByStatus(int status)
        {
            var count = await (from frd in _financialRequestDetailRepository.GetAll()
                               join fr in _financialRequestRepository.GetAll() on frd.FinancialRequestId equals fr.Id
                               where fr.AutomateState == status && fr.VoucherHeadId == null && fr.YearId == _currenUserAccessor.GetYearId()
                               select frd).CountAsync();


            return count;
        }

        private int GetCurrencyTypeBaseId(int typeId)
        {
            if (typeId == 1)
                return (int)CurrencyTypeBases.Rial;
            else if (typeId == 2)
                return (int)CurrencyTypeBases.Dollar;
            else
                return (int)CurrencyTypeBases.Euro;
        }

        private int GetDocumentTypeBaseId(int typeId)
        {
            if (typeId == 1)
                return (int)DocumentTypeBases.CashFee;
            else if (typeId == 2)
                return (int)DocumentTypeBases.CashFee;
            else
                return (int)DocumentTypeBases.Remittance;
        }

        public async Task<bool> ExecuteSpUpdateVoucherHead(List<int> financialRequestIds)
        {
            //var model = new spUpdateWarehouseLayoutQuantitiesParam() { CommodityId = CommodityId, WarehouseLayoutId = WarehouseLayoutId, };
            //var parameters = model.EntityToSqlParameters();

            //await _context.ExecuteSqlQueryAsync<object>($"EXEC [inventory].[spUpdateWarehouseLayoutQuantities]  {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());

            //return 1;

            string requestIds = "";
            foreach(var item in financialRequestIds)
            {
                requestIds += item.ToString() + ",";
            }

            var model = new SpCheckVoucherIdInFinancialRequestAndUpdateVoucherIdInFinancialRequestParam() { FinancialRequestIds = requestIds, };
      
            var parameters = model.EntityToSqlParameters();
      
            await _bursaryUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [bursary].[CheckVoucherIdInFinancialRequestAndUpdateVoucherIdInFinancialRequest] {Eefa.Common.Data.Query.QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return true;

        }

        private ChequeSheetModel GetChequeDetailByChequeId(int chequeId)
        {
            var chequeSheets =   (from cs in _chequeSheetRepository.GetAll()
                                      join bv in _baseValueRepository.GetAll() on cs.ChequeTypeBaseId equals bv.Id
                                      join b in _bankRepository.GetAll() on cs.BankId equals b.Id
                                      where  cs.Id == chequeId
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
                                          Title = cs.SheetSeqNumber + b.Title

                                      }).SingleOrDefault();

            return chequeSheets;
        }
        //public async Task<PagedList<FinancialRequestModel>> GetAllDetails(PaginatedQueryModel query)
        //{
        //    var details = (from frd in _financialRequestDetailRepository.GetAll()
        //                   join fr in _financialRequestRepository.GetAll() on frd.FinancialRequestId equals fr.Id
        //                   join Cusr in _userRepository.GetAll() on frd.CreatedById equals Cusr.Id
        //                   join Cprs in _personRepository.GetAll() on Cusr.PersonId equals Cprs.Id
        //                   from modifyUser in MusrNULL.DefaultIfEmpty()
        //                   join Mprs in _personRepository.GetAll() on modifyUser.PersonId equals Mprs.Id into MprsNULL
        //                   from modityPerson in MprsNULL.DefaultIfEmpty()
        //                   join cah in _accountHead.GetAll() on frd.CreditAccountHeadId equals cah.Id
                           
        //                   )
        //}
    
    }


    public enum CurrencyTypeBases
    {
        Rial = 28306,
        Dollar = 28309,
        Euro = 28310,
        IraqDinar = 28311,
        Yuan = 28312,
        Derham = 28698
    }

    public enum DocumentTypeBases
    {
        Remittance = 28509,
        PayCheque = 28511,
        CashFee = 28513
    }

}
