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
using Eefa.Logistic.Application;

using Eefa.Logistic.Domain.Common;
using Eefa.Logistic.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Logistic.Application
{
    public class MapSamatozinToDanaQueries : IMapSamatozinToDanaQueries
    {
        private readonly IMapper _mapper;
        private readonly LogisticContext _context;
        private readonly SamaTowzinContext _contextSama;

        public MapSamatozinToDanaQueries(
              IMapper mapper
            , LogisticContext context
            , SamaTowzinContext contextSama

            )
        {
            _mapper = mapper;
            _context = context;
            _contextSama = contextSama;


        }
        public async Task<PagedList<MapSamatozinToDanaModel>> GetAll(PaginatedQueryModel query)

        {
            var entities = queryable();

            var list = await entities.FilterQuery(query.Conditions)
                         .OrderByMultipleColumns()
                         .Paginate(query.Paginator())
                         .Distinct()
                         .ToListAsync();

            var result = new PagedList<MapSamatozinToDanaModel>()
            {
                Data = list,
                TotalCount = query.PageIndex <= 1
                            ? entities.Count()

                            : 0
            };
            return result;

        }
        public async Task<MapSamatozinToDanaModel> GetById(int id)
        {
            var entity = await queryable().Where(a => a.Id == id ).FirstOrDefaultAsync();

            
            return entity;
        }
        private  IQueryable<MapSamatozinToDanaModel> queryable()
        {

            //var Prehension =  _contextSama.Prehension.Where(a=>a.PrehensionDateTime>'').ToList();

            return (from acc in _context.AccountReferenceView
                    join map in _context.MapSamatozinToDana on acc.Id equals map.AccountReferenceId
                    //join sama in Prehension on map.SamaTozinCode equals sama.TheAccountTitle
                    where map.IsDeleted == false

                    select new MapSamatozinToDanaModel()
                    {
                        Id=map.Id,
                        AccountReferenceId = acc.Id,
                        AccountReferenceCode = acc.Code,
                        AccountReferencesTitle = acc.Title,
                        AccountReferencesGroupsCode = acc.AccountReferencesGroupsCode,
                        AccountReferenceGroupId = acc.AccountReferenceGroupId,
                        AccountReferenceGoupCodeId = acc.AccountReferenceGoupCodeId,
                        SamaTozinCode = map.SamaTozinCode,
                        SamaTozinTitle = map.SamaTozinTitle,
                        

                    }
                       );

        }

    }
}
