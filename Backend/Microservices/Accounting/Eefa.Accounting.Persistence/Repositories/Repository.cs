using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class Repository<T> : IRepository<T> where T : AuditableEntity
{
    private readonly ApplicationDbContext _context;
    private readonly IApplicationUser _applicationUser;
    private readonly AutoMapper.IConfigurationProvider _configuration;

    public Repository(ApplicationDbContext context, IApplicationUser applicationUser, AutoMapper.IConfigurationProvider mapperConfigurationProvider)
    {
        _context = context;
        _applicationUser = applicationUser;
        _configuration = mapperConfigurationProvider;
    }

    private IQueryable<T> GetQueryable(bool includeDeleted = false)

    {
        var query = this._context.Set<T>().AsQueryable();
        if (!includeDeleted) query = query.Where(x => !x.IsDeleted);
        return query;
    }

    #region Add
    public virtual void Add(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedById = _applicationUser.Id;
        entity.OwnerRoleId = _applicationUser.RoleId;

        _context.Set<T>().Add(entity);
    }
    public virtual void Add(IEnumerable<T> entities)
    {
        foreach (var entity in entities) this.Add(entity);
    }
    #endregion

    #region Update
    public virtual void Update(T entity)
    {
        entity.ModifiedAt = DateTime.UtcNow;
        entity.ModifiedById = _applicationUser.Id;

        _context.Set<T>().Update(entity);
    }
    public virtual void Update(IEnumerable<T> entities)
    {
        foreach (var entity in entities) this.Update(entity);

    }
    #endregion

    #region Delete
    public virtual void Delete(T entity)
    {
        entity.IsDeleted = true;
        entity.ModifiedAt = DateTime.UtcNow;
        entity.ModifiedById = _applicationUser.Id;

        _context.Set<T>().Update(entity);
    }
    public virtual void Delete(IEnumerable<T> entities)
    {
        foreach (var entity in entities) this.Delete(entity);
    }
    #endregion

    #region Get Single
    public async Task<T> GetAsync(Expression<Func<T, bool>> condition)

    {
        return await GetAsync(condition, null, false);
    }
    public async Task<T> GetAsync(Expression<Func<T, bool>> condition, bool asNoTracking)

    {
        return await GetAsync(condition, null, asNoTracking);
    }
    public async Task<T> GetAsync(
        Expression<Func<T, bool>> condition,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)

    {
        return await GetAsync(condition, includes, false);
    }
    public async Task<T> GetAsync(
        Expression<Func<T, bool>> condition,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
        bool asNoTracking)

    {
        IQueryable<T> query = this.GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        if (includes != null)
        {
            query = includes(query);
        }

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }
    public async Task<T> GetAsync(Specification<T> specification)

    {
        return await GetAsync(specification, false);
    }
    public async Task<T> GetAsync(Specification<T> specification, bool asNoTracking)

    {
        IQueryable<T> query = this.GetQueryable();

        if (specification != null)
        {
            query = query.GetSpecifiedQuery(specification);
        }

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }
    public async Task<TProjectedType> GetProjectedAsync<TProjectedType>(
       Expression<Func<T, bool>> condition)

    {
        IQueryable<T> query = this.GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        return await query.ProjectTo<TProjectedType>(_configuration).FirstOrDefaultAsync();
    }
    public async Task<TProjectedType> GetProjectedAsync<TProjectedType>(
        Expression<Func<T, bool>> condition,
        Expression<Func<T, TProjectedType>> selectExpression)

    {
        if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));

        IQueryable<T> query = this.GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        return await query.Select(selectExpression).FirstOrDefaultAsync();
    }
    public async Task<TProjectedType> GetProjectedAsync<TProjectedType>(
        Specification<T> specification)

    {
        IQueryable<T> query = this.GetQueryable();

        return await query.GetSpecifiedQuery<T, TProjectedType>(specification, mapperConfigurations: _configuration).FirstOrDefaultAsync();

    }
    public async Task<TProjectedType> GetProjectedAsync<TProjectedType>(
        Specification<T> specification,
        Expression<Func<T, TProjectedType>> selectExpression)

    {
        IQueryable<T> query = this.GetQueryable();

        if (specification == null) throw new ArgumentNullException(nameof(specification));
        if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));

        return await query.GetSpecifiedQuery<T, TProjectedType>(specification, selectExpression: selectExpression).FirstOrDefaultAsync();
    }
    #endregion

    #region Get By Id
    public async Task<T> GetByIdAsync(object id)

    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        T trackedEntity = await GetByIdAsync(id, false);
        return trackedEntity;
    }
    public async Task<T> GetByIdAsync(object id, bool asNoTracking)

    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        T trackedEntity = await GetByIdAsync(id, null, asNoTracking);
        return trackedEntity;
    }
    public async Task<T> GetByIdAsync(object id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)

    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        T trackedEntity = await GetByIdAsync(id, includes, false);
        return trackedEntity;
    }
    public async Task<T> GetByIdAsync(object id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes, bool asNoTracking = false)

    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        IEntityType entityType = _context.Model.FindEntityType(typeof(T));

        string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
        Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

        if (primaryKeyName == null || primaryKeyType == null) throw new ArgumentException("Entity does not have any primary key defined", nameof(id));

        object primayKeyValue = null;

        try
        {
            primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
        }

        ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
        MemberExpression me = Expression.Property(pe, primaryKeyName);
        ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
        BinaryExpression body = Expression.Equal(me, constant);
        Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

        IQueryable<T> query = this.GetQueryable();

        if (includes != null)
        {
            query = includes(query);
        }

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        T trackedEntity = await query.FirstOrDefaultAsync(expressionTree);
        return trackedEntity;
    }
    public async Task<TProjectedType> GetProjectedByIdAsync<TProjectedType>(
        object id)

    {
        if (id == null) throw new ArgumentNullException(nameof(id));


        IEntityType entityType = _context.Model.FindEntityType(typeof(T));

        string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
        Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

        if (primaryKeyName == null || primaryKeyType == null)
        {
            throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
        }

        object primayKeyValue = null;

        try
        {
            primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
        }

        ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
        MemberExpression me = Expression.Property(pe, primaryKeyName);
        ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
        BinaryExpression body = Expression.Equal(me, constant);
        Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

        IQueryable<T> query = this.GetQueryable();

        return await query.Where(expressionTree).ProjectTo<TProjectedType>(_configuration).FirstOrDefaultAsync();
    }
    public async Task<TProjectedType> GetProjectedByIdAsync<TProjectedType>(
        object id,
        Expression<Func<T, TProjectedType>> selectExpression)

    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));

        IEntityType entityType = _context.Model.FindEntityType(typeof(T));

        string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
        Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

        if (primaryKeyName == null || primaryKeyType == null)
        {
            throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
        }

        object primayKeyValue = null;

        try
        {
            primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
        }

        ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
        MemberExpression me = Expression.Property(pe, primaryKeyName);
        ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
        BinaryExpression body = Expression.Equal(me, constant);
        Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

        IQueryable<T> query = this.GetQueryable();

        return await query.Where(expressionTree).Select(selectExpression).FirstOrDefaultAsync();
    }
    #endregion

    #region List
    public async Task<List<T>> GetListAsync()

    {
        return await GetListAsync(false);
    }
    public async Task<List<T>> GetListAsync(bool asNoTracking)

    {
        Func<IQueryable<T>, IIncludableQueryable<T, object>> nullValue = null;
        return await GetListAsync(nullValue, asNoTracking);
    }
    public async Task<List<T>> GetListAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)

    {
        return await GetListAsync(includes, false);
    }
    public async Task<List<T>> GetListAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes, bool asNoTracking)

    {
        IQueryable<T> query = this.GetQueryable();

        if (includes != null)
        {
            query = includes(query);
        }

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        List<T> entities = await query.ToListAsync();

        return entities;
    }
    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> condition)

    {
        return await GetListAsync(condition, false);
    }
    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> condition, bool asNoTracking)

    {
        return await GetListAsync(condition, null, asNoTracking);
    }
    public async Task<List<T>> GetListAsync(
        Expression<Func<T, bool>> condition,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
        bool asNoTracking)

    {
        IQueryable<T> query = this.GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        if (includes != null)
        {
            query = includes(query);
        }

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        List<T> entities = await query.ToListAsync();

        return entities;
    }
    public async Task<List<T>> GetListAsync(Specification<T> specification)

    {
        return await GetListAsync(specification, false);
    }
    public async Task<List<T>> GetListAsync(Specification<T> specification, bool asNoTracking)

    {
        IQueryable<T> query = this.GetQueryable();

        if (specification != null)
        {
            query = query.GetSpecifiedQuery(specification);
        }

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync();
    }
    public async Task<PaginatedList<T>> GetPaginatedListAsync(Specification<T> specification)

    {
        return await GetPaginatedListAsync(specification, false);
    }
    public async Task<PaginatedList<T>> GetPaginatedListAsync(Specification<T> specification, bool asNoTracking)

    {
        IQueryable<T> query = this.GetQueryable();

        if (specification == null) throw new ArgumentNullException(nameof(specification));

        query = query.GetSpecifiedQuery(specification);
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return new PaginatedList<T>(
            await query.ToListAsync(),
            (specification.Conditions != null && specification.Conditions.Any()) || (specification.ApplicationConditions != null && specification.ApplicationConditions.Any()) ? await this.GetQueryable().GetSpecifiedQuery(specification, true).CountAsync() : await GetCountAsync(),
            specification.PageIndex,
            specification.PageSize
        );
    }
    public async Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>()

    {
        List<TProjectedType> entities = await this.GetQueryable().ProjectTo<TProjectedType>(_configuration).ToListAsync();

        return entities;
    }
    public async Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
        Expression<Func<T, TProjectedType>> selectExpression)

    {
        if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));

        List<TProjectedType> entities = await this.GetQueryable().Select(selectExpression).ToListAsync();

        return entities;
    }
    public async Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
      Expression<Func<T, bool>> condition)

    {
        IQueryable<T> query = this.GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        List<TProjectedType> projectedEntites = await query.ProjectTo<TProjectedType>(_configuration).ToListAsync();

        return projectedEntites;
    }
    public async Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
        Expression<Func<T, bool>> condition,
        Expression<Func<T, TProjectedType>> selectExpression)

    {
        if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));

        IQueryable<T> query = this.GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        List<TProjectedType> projectedEntites = await query.Select(selectExpression).ToListAsync();

        return projectedEntites;
    }
    public async Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
       Specification<T> specification)

    {
        IQueryable<T> query = this.GetQueryable();

        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var projectedQuery = query.GetSpecifiedQuery<T, TProjectedType>(specification, _configuration);

        return await projectedQuery.ToListAsync();
    }
    public async Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
        Specification<T> specification,
        Expression<Func<T, TProjectedType>> selectExpression)

    {
        if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));

        IQueryable<T> query = this.GetQueryable();

        if (specification != null)
        {
            query = query.GetSpecifiedQuery<T>(specification);
        }

        return await query.Select(selectExpression).ToListAsync();
    }
    public async Task<PaginatedList<TProjectedType>> GetPaginatedProjectedListAsync<TProjectedType>(
       Specification<T> specification)

    {

        IQueryable<T> query = this.GetQueryable();
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var projectedQuery = query.GetSpecifiedQuery<T, TProjectedType>(specification, mapperConfigurations: _configuration);

        return new PaginatedList<TProjectedType>(
            await projectedQuery.ToListAsync(),
            (specification.Conditions != null && specification.Conditions.Any()) || (specification.ApplicationConditions != null && specification.ApplicationConditions.Any()) ? await this.GetQueryable().GetSpecifiedQuery<T, TProjectedType>(specification, mapperConfigurations: _configuration).CountAsync() : await GetCountAsync(),
            specification.PageIndex,
            specification.PageSize);
    }
    public async Task<PaginatedList<TProjectedType>> GetPaginatedProjectedListAsync<TProjectedType>(
        Specification<T> specification,
        Expression<Func<T, TProjectedType>> selectExpression)

    {
        if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));

        IQueryable<T> query = this.GetQueryable();

        if (specification == null) throw new ArgumentNullException(nameof(specification));
        if (selectExpression == null) throw new ArgumentNullException(nameof(selectExpression));

        var projectedQuery = query.GetSpecifiedQuery<T, TProjectedType>(specification, selectExpression: selectExpression);

        return new PaginatedList<TProjectedType>(
           await projectedQuery.ToListAsync(),
           (specification.Conditions != null && specification.Conditions.Any()) || (specification.ApplicationConditions != null && specification.ApplicationConditions.Any()) ? await this.GetQueryable().GetSpecifiedQuery<T, TProjectedType>(specification, selectExpression: selectExpression).CountAsync() : await GetCountAsync(),
           specification.PageIndex,
           specification.PageSize);
    }
    #endregion

    #region Count
    public async Task<int> GetCountAsync()

    {
        int count = await GetQueryable().CountAsync();
        return count;
    }
    public async Task<int> GetCountAsync(params Expression<Func<T, bool>>[] conditions)

    {
        IQueryable<T> query = this.GetQueryable();

        if (conditions == null)
        {
            return await query.CountAsync();
        }

        foreach (Expression<Func<T, bool>> expression in conditions)
        {
            query = query.Where(expression);
        }

        return await query.CountAsync();
    }
    public async Task<int> GetCountAsync(List<Expression<Func<T, bool>>> conditions)

    {
        IQueryable<T> query = this.GetQueryable();

        if (conditions == null)
        {
            return await query.CountAsync();
        }

        foreach (Expression<Func<T, bool>> expression in conditions)
        {
            query = query.Where(expression);
        }

        return await query.CountAsync();
    }
    public async Task<long> GetLongCountAsync()

    {
        long count = await GetQueryable().LongCountAsync();
        return count;
    }
    public async Task<long> GetLongCountAsync(params Expression<Func<T, bool>>[] conditions)

    {
        IQueryable<T> query = this.GetQueryable();

        if (conditions == null)
        {
            return await query.LongCountAsync();
        }

        foreach (Expression<Func<T, bool>> expression in conditions)
        {
            query = query.Where(expression);
        }

        return await query.LongCountAsync();
    }
    public async Task<long> GetLongCountAsync(List<Expression<Func<T, bool>>> conditions)

    {
        IQueryable<T> query = this.GetQueryable();

        if (conditions == null)
        {
            return await query.LongCountAsync();
        }

        foreach (Expression<Func<T, bool>> expression in conditions)
        {
            query = query.Where(expression);
        }

        return await query.LongCountAsync();
    }
    #endregion

    #region
    public async Task<TProjectedType> MaxAsync<TProjectedType>(
        Expression<Func<T, TProjectedType>> selectExpression)
    {
        return await this.GetQueryable().Select(selectExpression).MaxAsync();
    }
    public async Task<TProjectedType> MaxAsync<TProjectedType>(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression)
    {
        return await this.GetQueryable().Where(condition).Select(selectExpression).MaxAsync();
    }
    #endregion

    #region Exists
    public async Task<bool> ExistsAsync()
    {
        return await ExistsAsync(null);
    }
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> condition)
    {
        IQueryable<T> query = this.GetQueryable();

        if (condition == null)
        {
            return await query.AnyAsync();
        }

        bool isExists = await query.AnyAsync(condition);
        return isExists;
    }
    #endregion
}
