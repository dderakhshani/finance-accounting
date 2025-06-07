using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Domain.Aggregates.DocumentHeadAggregate;
using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Queries.DocumentHead
{
    public class DocumentHeadQueries : IDocumentHeadQueries
    {
        private readonly IDocumentHeadRepository _docuemtHeadRepository;
        private readonly IFinancialRequestDocumentHeadRepository _financialRequestDocumentHeadRepository;
        private readonly IMapper _mapper;

        public DocumentHeadQueries(IDocumentHeadRepository docuemtHeadRepository, IMapper mapper, IFinancialRequestDocumentHeadRepository financialRequestDocumentHeadRepository)
        {
            _docuemtHeadRepository = docuemtHeadRepository;
            _mapper = mapper;
            _financialRequestDocumentHeadRepository = financialRequestDocumentHeadRepository;
        }


        public async Task<PagedList<DocumentHeadModel>> GetDocumentHeadsByReferenceId(PaginatedQueryModel query)
        {
            var documentHeads = _docuemtHeadRepository.GetAll()
                .Include(x=>x.Reference)
                .ProjectTo<DocumentHeadModel>(_mapper.ConfigurationProvider)
                .FilterQuery(query.Conditions)
                .OrderByMultipleColumns(query.OrderByProperty);

            //var invoices = _docuemtHeadRepository.GetAll()
            //    .Include(x => x.FinancialRequestDocumentHeads)
            //    .ProjectTo<DocumentHeadModel>(_mapper.ConfigurationProvider)
            //    .FilterQuery(query.Conditions)
            //    .OrderByMultipleColumns(query.OrderByProperty);



           var result = new PagedList<DocumentHeadModel>()
            {
                Data = await documentHeads.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1 ? await documentHeads.CountAsync(): 0
            };

            return result;
        }
    }
}
