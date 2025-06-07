using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Services.Interfaces;
using Infrastructure.Data.Models;
using Infrastructure.Data.SqlServer;
using AutoMapper.QueryableExtensions;
using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;

namespace $rootnamespace$.$fileinputname$
{
    public class $fileinputname$Service : I$fileinputname$Service
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public $fileinputname$Service(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<$fileinputname$ServiceModel>> GetAll(Pagination pagination, CancellationToken cancellationToken)
        {
            return await _repository.GetAll<Identity.Data.Entities.$fileinputname$>(cfg =>
                cfg.Paginate(pagination))
                .ProjectTo<$fileinputname$ServiceModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<$fileinputname$ServiceModel> FindById(int id, CancellationToken cancellationToken)
        {
            return await _repository.Find<Identity.Data.Entities.$fileinputname$>(cfg => cfg.ObjectId(id))
                 .ProjectTo<$fileinputname$ServiceModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        }

        public async void Add($fileinputname$ServiceModel inpute)
        {
            await _repository.Insert(_mapper.Map<Identity.Data.Entities.$fileinputname$>(inpute));
        }

        public async void Update($fileinputname$ServiceModel inpute, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Identity.Data.Entities.$fileinputname$>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
                .Update<Identity.Data.Entities.$fileinputname$>(entity, _mapper.Map<Identity.Data.Entities.$fileinputname$>(inpute));

            _repository.Update(entity);
        }


        public async void SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Identity.Data.Entities.$fileinputname$>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            _repository.Delete(entity);
        }

        public async Task<List<$fileinputname$ServiceModel>> Search(Pagination pagination, List<SearchQueryItem> queries, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAll<Identity.Data.Entities.$fileinputname$>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Identity.Data.Entities.$fileinputname$>(queries))
                    .Paginate(pagination))
                .ProjectTo<$fileinputname$ServiceModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}