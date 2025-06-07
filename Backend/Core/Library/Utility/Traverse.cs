using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.Common;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Utility
{
    public class Traverse : ITraverse
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public Traverse(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //public async Task<ICollection<T>> FindAllParents<T>(ICollection<T> allEnities, int? id, Expression<Func<T, bool>>? conditionExpression) where T : class, IHierarchical
        //{
        //    if (id == null)
        //    {
        //        return allEnities;
        //    }
        //    var parent = await _repository.Find<T>(x => x.ObjectId(id)).FirstOrDefaultAsync();

        //    if (allEnities == null)
        //    {
        //        allEnities = new List<T>();
        //    }

        //    if (parent == null)
        //        return allEnities;

        //    allEnities.Add(parent);
        //    if (parent.ParentId == null)
        //    {
        //        return allEnities;
        //    }
        //    return await FindAllParents(allEnities, parent.ParentId, conditionExpression);
        //}


      



    public async Task<ICollection<TOut>> FindAllParents2<TIn, TOut>(ICollection<TOut> allEnities, int? id, Expression<Func<TOut, bool>>? conditionExpression) where TIn : class, IHierarchicalBaseEntity,IBaseEntity where TOut : class, IHierarchicalBaseEntity
        {
            if (id == null)
            {
                return allEnities;
            }

            var query = _repository.Find<TIn>(x => x.ObjectId(id))
                .ProjectTo<TOut>(_mapper.ConfigurationProvider);
            var parent = await query.FirstOrDefaultAsync(CancellationToken.None);

            if (allEnities == null)
            {
                allEnities = new List<TOut>();
            }

            if (parent == null)
                return allEnities;

            allEnities.Add(parent);
            if (parent.ParentId == null)
            {
                return allEnities;
            }
            return await FindAllParents2<TIn,TOut>(allEnities, parent.ParentId, conditionExpression);
        }

    }
}