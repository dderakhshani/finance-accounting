using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Commodity.Application.Queries.Measure
{
    public class MeasureQueries : IMeasureQueries
    {
        private readonly IRepository<Data.Entities.MeasureUnit> _repositoryMeasureUnit;
        private readonly IRepository<Data.Entities.MeasureUnitConversion> _repositoryMeasureUnitConversion;
        private readonly IMapper _mapper;

        public MeasureQueries(IRepository<Data.Entities.MeasureUnit> repositoryMeasureUnit,
            IRepository<Data.Entities.MeasureUnitConversion> repositoryMeasureUnitConversion,
            IMapper mapper)
        {
            _mapper = mapper;
            _repositoryMeasureUnit = repositoryMeasureUnit;
            _repositoryMeasureUnitConversion = repositoryMeasureUnitConversion;
        }

        public async Task<MeasureUnitModel> GetMeasureUnitById(int id)
        {
            var entity = await _repositoryMeasureUnit.Find(id);
            return _mapper.Map<MeasureUnitModel>(entity);
        }

        public async Task<PagedList<MeasureUnitModel>> GetMeasures(PaginatedQueryModel query)
        {
            var entitis = _repositoryMeasureUnit.GetAll()
                .ProjectTo<MeasureUnitModel>(_mapper.ConfigurationProvider)
                .FilterQuery(query.Conditions)
                .OrderByMultipleColumns(query.OrderByProperty);
            return new PagedList<MeasureUnitModel>()
            {
                Data = await entitis
                    .Paginate(query.Paginator())
                    .ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entitis
                        .CountAsync()
                    : 0
            };
        }

        public async Task<MeasureUnitConversionModel> GetConversionById(int id)
        {
            var entity = await _repositoryMeasureUnitConversion.Find(id);
            return _mapper.Map<MeasureUnitConversionModel>(entity);
        }

        public async Task<PagedList<MeasureUnitConversionModel>> GetConversionsByMeasureUnitId(int measureUnitId,
            PaginatedQueryModel query)
        {
            var join = (from muc in _repositoryMeasureUnitConversion.GetAll()
                join smu in _repositoryMeasureUnit.GetAll() on muc.SourceMeasureUnitId equals smu.Id
                join dmu in _repositoryMeasureUnit.GetAll() on muc.DestinationMeasureUnitId equals dmu.Id
                select new MeasureUnitConversionModel { });


            var entitis = _repositoryMeasureUnitConversion.GetAll()
                .Where(c => c.SourceMeasureUnitId == measureUnitId)
                .ProjectTo<MeasureUnitConversionModel>(_mapper.ConfigurationProvider);
            var data = await entitis
                .ToListAsync();
            return new PagedList<MeasureUnitConversionModel>()
            {
                Data = data,
                TotalCount = query.PageIndex <= 1
                    ? await entitis
                        .CountAsync()
                    : 0
            };
        }
    }
}