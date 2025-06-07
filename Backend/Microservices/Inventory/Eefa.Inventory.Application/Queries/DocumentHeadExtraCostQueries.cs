using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;
using System;

namespace Eefa.Inventory.Application
{
    public class DocumentHeadExtraCostQueries : IDocumentHeadExtraCostQueries
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;

        public DocumentHeadExtraCostQueries(
              IMapper mapper
            , IInvertoryUnitOfWork context
            )
        {
            _mapper = mapper;
            _context = context;

        }
        public async Task<PagedList<DocumentHeadExtraCostModel>> GetAll(PaginatedQueryModel query)
            
        {

            var entities = queryable();

            var list = await entities.FilterQuery(query.Conditions)
                         .OrderByMultipleColumns()
                         .Paginate(query.Paginator())
                         .Distinct()
                         .ToListAsync();
            
            var result = new PagedList<DocumentHeadExtraCostModel>()
            {
                Data = list,
                TotalCount = query.PageIndex <= 1
                            ? entities.Count()

                            : 0
            };
            return result;
        }
        
        public async Task<DocumentHeadExtraCostModel> GetById(int Id)
        {
            
                var result =await queryable()
                            .Where(a => a.Id == Id)
                            .ProjectTo<DocumentHeadExtraCostModel>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync();
                
                return result;
            
            
        }

        
        private IQueryable<DocumentHeadExtraCostModel> queryable()
        {
            return (from cost in _context.DocumentHeadExtraCost
                    join head in _context.AccountHead on cost.ExtraCostAccountHeadId equals head.Id
                    join acc in _context.AccountReferenceView on cost.ExtraCostAccountReferenceId equals acc.Id
                    
                    where cost.IsDeleted == false && acc.AccountReferenceGroupId== cost.ExtraCostAccountReferenceGroupId

                    select new DocumentHeadExtraCostModel
                    {
                        Id = cost.Id,
                        ExtraCostAccountHeadTitle = head.Title,
                        ExtraCostAccountReferenceTitle = acc.Title,
                        ExtraCostAccountHeadId = cost.ExtraCostAccountHeadId,
                        ExtraCostAccountReferenceGroupId = cost.ExtraCostAccountReferenceGroupId,
                        ExtraCostAccountReferenceId = cost.ExtraCostAccountReferenceId,
                        DocumentHeadId = cost.DocumentHeadId,
                        ExtraCostAmount = cost.ExtraCostAmount,
                        ExtraCostDescription = cost.ExtraCostDescription,
                        ExtraCostCurrencyTypeBaseId = cost.ExtraCostCurrencyTypeBaseId,
                        ExtraCostCurrencyFee = cost.ExtraCostCurrencyFee,
                        ExtraCostCurrencyAmount = cost.ExtraCostCurrencyAmount,
                        FinancialOperationNumber = cost.FinancialOperationNumber,
                    }
                       ); 

        }

       public async Task<PagedList<DocumentHeadExtraCostModel>> GetByDocumentHeadId(List<int> documentHeadIds)
        {
            var entity = await _context.DocumentHeadExtraCost
                                .Where(a => documentHeadIds.Contains(a.DocumentHeadId) && !a.IsDeleted)
                                .ProjectTo<DocumentHeadExtraCostModel>(_mapper.ConfigurationProvider)
                                .ToListAsync();

            var result = new PagedList<DocumentHeadExtraCostModel>()
            {
                Data = entity,
                TotalCount = 0
            };
            return result;
            
        }
        public async Task<decimal> GetTotalExtraCost(List<int> documentHeadIds)
        {
            var total = await _context.DocumentHeadExtraCost
                                .Where(a => documentHeadIds.Contains(a.DocumentHeadId) && !a.IsDeleted)
                                .SumAsync(a=>a.ExtraCostAmount);
            
            
            return Convert.ToDecimal(total);

        }


    }

}
