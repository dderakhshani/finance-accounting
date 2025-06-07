using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Exceptions;
using Eefa.Logistic.Domain;
using Eefa.Logistic.Domain.Common;
using Eefa.Logistic.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Logistic.Application
{
    public class PrehensionQueries : IPrehensionQueries
    {
        private readonly IMapper _mapper;
        private readonly LogisticContext _context;
        private readonly SamaTowzinContext _contextSama;

        public PrehensionQueries(
              IMapper mapper
            , LogisticContext context
            , SamaTowzinContext contextSama

            )
        {
            _mapper = mapper;
            _context = context;
            _contextSama = contextSama;


        }
        public async Task<PagedList<Prehension>> GetAll(PaginatedQueryModel query)

        {
            try
            {
                var result = (List<Prehension>)await _contextSama.Prehension
                           .FilterQuery(query.Conditions)
                           .Paginate(query.Paginator())
                           .ToListAsync();

                return new PagedList<Prehension>()
                {
                    Data = (IEnumerable<Prehension>)result,
                    TotalCount = 0

                };
            }
            catch(Exception ex)
            {

            }
            return null;
           
            

        }
        public async Task<PagedList<string>> GetGroupByCode(PaginatedQueryModel query)
        {
            var result = (List<string>) await _contextSama.Prehension.Where(a=> !a.TheAccountTitle.Contains("...")).Select(a=>a.TheAccountTitle).FilterQuery(query.Conditions)
                           .Distinct().ToListAsync();

            return new PagedList<string>()
            {
                Data = (IEnumerable<string>)result,
                TotalCount = 0

            };

        }
        public async Task<Prehension> GetById(int id)
        {
            var entity = await _contextSama.Prehension.Where(a => a.Id == id ).FirstOrDefaultAsync();

            
            return entity;
        }
        
    }
}
