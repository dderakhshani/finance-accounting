using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;
using AutoMapper.QueryableExtensions;

namespace Eefa.Inventory.Application
{
    public class PersonsDebitedCommoditiesQueries : IPersonsDebitedCommoditiesQueries
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _contex;

        public PersonsDebitedCommoditiesQueries(
              IMapper mapper
            , IInvertoryUnitOfWork contex
            )
        {
            _mapper = mapper;
            _contex = contex;

        }
        public async Task<PagedList<PersonsDebitedCommoditiesModel>> GetAll(string FromDate,string ToDate, PaginatedQueryModel query)
           
        {
                var entitis = _contex.PersonsDebitedCommoditiesView;

                var list = await entitis.FilterQuery(query.Conditions)
                             .OrderByMultipleColumns()
                             .Paginate(query.Paginator())
                             .ProjectTo<PersonsDebitedCommoditiesModel>(_mapper.ConfigurationProvider)
                             .Distinct()
                             .ToListAsync();
                var result = new PagedList<PersonsDebitedCommoditiesModel>()
                {
                    Data = list,
                    TotalCount = query.PageIndex <= 1
                                ? entitis.Count()

                                : 0
                };
                return result;
            
          
        }

        public async Task<PagedList<PersonsDebitedCommoditiesModel>> GetByDocumentId(int DocumentItemId, int CommodityId, PaginatedQueryModel query)
        {

            var entitis = _contex.PersonsDebitedCommoditiesView.Where(a=>a.DocumentItemId== DocumentItemId && a.CommodityId== CommodityId);

            var list = await entitis.FilterQuery(query.Conditions)
                         .Paginate(query.Paginator())
                         .ProjectTo<PersonsDebitedCommoditiesModel>(_mapper.ConfigurationProvider)
                         .Distinct()
                         .OrderByMultipleColumns()
                         .ToListAsync();
            var result = new PagedList<PersonsDebitedCommoditiesModel>()
            {
                Data = list,
                TotalCount = query.PageIndex <= 1
                            ? entitis.Count()

                            : 0
            };
            return result;
        }

        public async Task<PersonsDebitedCommoditiesModel> GetById(int id)
        {
            var entity = await _contex.PersonsDebitedCommoditiesView.Where(a=>a.Id==id).FirstOrDefaultAsync();
            return _mapper.Map<PersonsDebitedCommoditiesModel>(entity);
        }
    }
}
