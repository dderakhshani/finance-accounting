using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class AccountReferencesQueries : IAccountReferences
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IRepository<Receipt> _receiptRepository;
        public AccountReferencesQueries(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , IRepository<Receipt> receiptRepository

            )
        {
            _mapper = mapper;
            _context = context;
            _receiptRepository = receiptRepository;
        }
        public async Task<PagedList<AccountReferenceViewModel>> GetAccountReferences(PaginatedQueryModel query,int?  AccountHeadId)
        {

            var entities =  (from reference in _context.AccountReferenceView 
                             
                             select new AccountReferenceViewModel
                                  {

                                      Code = reference.Code,
                                      Title = reference.Code + " 🔅 " + reference.Title,
                                      SearchTerm = reference.SearchTerm,
                                      AccountReferenceGroupId = reference.AccountReferenceGroupId,
                                      Id = reference.Id,
                                  }
                   ).FilterQuery(query.Conditions)
                   .Paginate(query.Paginator())
                   .Distinct();
            if(AccountHeadId>0)
            {
                entities = (from reference in entities
                            join rel in _context.AccountHeadRelReferenceGroup on reference.AccountReferenceGroupId equals rel.ReferenceGroupId
                            where (rel.AccountHeadId == AccountHeadId)
                            select reference
                            );

            }
            var result = new PagedList<AccountReferenceViewModel>()
            {
                Data = await entities.ToListAsync(),
                TotalCount = query.PageIndex <= 1
                            ? entities.Count()

                            : 0
            };
            return result;
        }
        public async Task<PagedList<AccountHead>> GetAccountHead(PaginatedQueryModel query)
        {
            
                var entities = await (from reference in _context.AccountHead.Where(a=>a.LastLevel && !a.IsDeleted && a.IsActive==true)
                                     select new AccountHead
                                     {
                                         Code = reference.Code,
                                         Title = reference.Title + " 🔅 " + reference.Code,
                                         Id = reference.Id,
                                         LastLevel = reference.LastLevel,
                                         IsActive=reference.IsActive,
                                         ParentId=reference.ParentId,
                                     }
                       )
                       .FilterQuery(query.Conditions)
                       .Paginate(query.Paginator())
                       .Distinct()
                       .OrderByMultipleColumns()
                       .ToListAsync();
                var result = new PagedList<AccountHead>()
                {
                    Data = entities,
                    TotalCount = query.PageIndex <= 1
                                ? entities.Count()

                                : 0
                };
                return result;
           
            
        }
        /// <summary>
        /// فهرست کارمندان
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<AccountReferenceViewModel>> GetAccountReferencesPerson(PaginatedQueryModel query)
        {
                var entities = await (from reference in _context.AccountReferenceEmployeeView
                                     select new AccountReferenceViewModel
                                     {
                                         Code = reference.Code,
                                         Title = reference.EmployeeCode + " 🔅 " + reference.Title,
                                         SearchTerm = reference.SearchTerm,
                                         Id = reference.Id,
                                     }
                       ).FilterQuery(query.Conditions)
                       .Paginate(query.Paginator())
                       .Distinct()
                       .ToListAsync();
                var result = new PagedList<AccountReferenceViewModel>()
                {
                    Data = entities,
                    TotalCount = query.PageIndex <= 1
                                ? entities.Count()

                                : 0
                };
                return result;
            
           
        }
        /// <summary>
        /// فهرست تامین کنندگان
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<AccountReferenceViewModel>> GetAccountReferencesProvider(PaginatedQueryModel query)
        {
            
                var entities = await (from reference in _context.AccountReferenceView.Where(a =>
                                a.AccountReferencesGroupsCode.StartsWith(ConstantValues.AccountReferenceGroup.ProviderCode) ||
                                a.AccountReferencesGroupsCode.StartsWith(ConstantValues.AccountReferenceGroup.ExternalProvider) ||
                                a.AccountReferencesGroupsCode.StartsWith(ConstantValues.AccountReferenceGroup.InternalProvider) ||
                                a.AccountReferencesGroupsCode.StartsWith(ConstantValues.AccountReferenceGroup.InternalProvider1)
                                )
                                     select new AccountReferenceViewModel
                                     {
                                         Code = reference.Code,
                                         Title = reference.Code + " 🔅 " + reference.Title,
                                         SearchTerm = reference.SearchTerm,
                                         AccountReferenceGroupId = reference.AccountReferenceGroupId,
                                         Id = reference.Id,
                                     }
                  ).FilterQuery(query.Conditions)
                  .Paginate(query.Paginator())
                  .Distinct()
                  .ToListAsync();
                var result = new PagedList<AccountReferenceViewModel>()
                {
                    Data = entities,
                    TotalCount = query.PageIndex <= 1
                                ? entities.Count()

                                : 0
                };
                return result;
           
        }

        /// <summary>
        /// لیست درخواست دهندگان در فرم فهرست رسید موقت
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<AccountReferenceModel>> GetRequesterReferenceWarhouse(int DocumentStatuesBaseValue, string FromDate, string ToDate, PaginatedQueryModel query)
        {
            
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            var Receipts = _context.ReceiptView.Where(a => a.DocumentStauseBaseValue == DocumentStatuesBaseValue
                                                   && (a.DocumentDate >= from || from == null)
                                                   && (a.DocumentDate <= to || to == null)).Select(a=>a.Id);

            var RequesterReference = await _context.DocumentHeadExtend.Where(a=> Receipts.Contains(a.DocumentHeadId)).Select(a => a.RequesterReferenceId).ToListAsync();

            var entities = await (from reference in _context.AccountReferences
                                 where RequesterReference.Contains(reference.Id)
                                 select new AccountReferenceModel
                                 {
                                     Code = reference.Code,
                                     Title = reference.Title,
                                     //AccountReferenceGroupId = accountReferencesGroup.Id,
                                     Id = reference.Id,
                                 }
                      )
                      .FilterQuery(query.Conditions)
                      .OrderByMultipleColumns()
                      .Paginate(query.Paginator())
                      .Distinct()
                      .ToListAsync();



            var result = new PagedList<AccountReferenceModel>()
            {
                Data = entities,
                TotalCount = query.PageIndex <= 1
                        ? entities.Count()

                        : 0
            };
            return result;
        }

        /// <summary>
        /// لیست فهرست تامین کنندگان در فرم ریالی کردن رسید مستقیم 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<AccountReferenceModel>> GetReferenceReceipt(PaginatedQueryModel query)
        {
            
            var RequesterReference = await _receiptRepository
                .GetAll()
                .Where(a => a.DocumentStauseBaseValue == (int)DocumentStateEnam.Direct)
                .Select(a => a.DebitAccountReferenceId)
                .ToListAsync();

            var entities = await (from reference in _context.AccountReferences
                                 where RequesterReference.Contains(reference.Id)
                                 select new AccountReferenceModel
                                 {
                                     Code = reference.Code,
                                     Title = reference.Title,
                                     //AccountReferenceGroupId = accountReferencesGroup.Id,
                                     Id = reference.Id,
                                 }
                      )
                      .FilterQuery(query.Conditions)
                      .OrderByMultipleColumns()
                      .Paginate(query.Paginator())
                      .Distinct()
                      .ToListAsync();



            var result = new PagedList<AccountReferenceModel>()
            {
                Data = entities,
                TotalCount = query.PageIndex <= 1
                        ? entities.Count()

                        : 0
            };
            return result;
        }
    }
    }
