using Eefa.Ticketing.Domain.Core;
using Eefa.Ticketing.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Infrastructure
{

    public class BaseRepository<TEntity, TKey> where TEntity : BaseEntity
    {
        private readonly EefaTicketingContext _context;
        private DbSet<TEntity> entities;
        public BaseRepository(EefaTicketingContext dbContext)
        {
            _context = dbContext;
            entities = _context.Set<TEntity>();
        }
        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await entities.Where(predicate).OrderByDescending(a => a.Id).FirstOrDefaultAsync(cancellationToken);

        }
        public async Task<TEntity> Get(CancellationToken cancellationToken)
        {
            return await entities.OrderByDescending(u => u.Id).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<List<TEntity>> GetList(CancellationToken cancellationToken)
        {
            return await entities.OrderByDescending(u => u.Id).ToListAsync(cancellationToken);
        }
        public async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await entities.Where(predicate).OrderByDescending(u => u.Id).ToListAsync(cancellationToken);
        }
        public void Delete(TEntity obj, CancellationToken cancellationToken)
        {
            entities.Remove(obj);
        }

        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            var obj = await entities.FindAsync(Id);
            if (obj != null)
            {
                entities.Remove(obj);
            }
            return Id;
        }
        public TEntity Edit(TEntity obj, CancellationToken cancellationToken)
        {
            _context.Update(obj);


            return obj;
        }
        public TEntity Add(TEntity obj, CancellationToken cancellationToken)
        {
            entities.Add(obj);
            return obj;
        }

        //public IQueryable<TEntity> AsQueryable(ICurrentUserAccessor currentUserAccessor)
        //{
        //    return QueryableExtensions.GetQuery<TEntity>(_context, currentUserAccessor);
        //}


    }
}
