using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IRepository<T> where T : AuditableEntity
{
    #region Add
    void Add(T entity);
    void Add(IEnumerable<T> entities);
    #endregion

    #region Update
    void Update(T entity);
    void Update(IEnumerable<T> entities);
    #endregion

    #region Delete
    void Delete(T entity);
    void Delete(IEnumerable<T> entities);
    #endregion

    #region Get Single
    Task<T> GetAsync(Expression<Func<T, bool>> condition);
    Task<T> GetAsync(Expression<Func<T, bool>> condition, bool asNoTracking);
    Task<T> GetAsync(
        Expression<Func<T, bool>> condition,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
    Task<T> GetAsync(
        Expression<Func<T, bool>> condition,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
        bool asNoTracking);
    Task<T> GetAsync(Specification<T> specification);
    Task<T> GetAsync(Specification<T> specification, bool asNoTracking);
    Task<TProjectedType> GetProjectedAsync<TProjectedType>(
       Expression<Func<T, bool>> condition);
    Task<TProjectedType> GetProjectedAsync<TProjectedType>(
        Expression<Func<T, bool>> condition,
        Expression<Func<T, TProjectedType>> selectExpression);
    Task<TProjectedType> GetProjectedAsync<TProjectedType>(
        Specification<T> specification);
    Task<TProjectedType> GetProjectedAsync<TProjectedType>(
        Specification<T> specification,
        Expression<Func<T, TProjectedType>> selectExpression);
    #endregion

    #region Get By Id
    Task<T> GetByIdAsync(object id);
    Task<T> GetByIdAsync(object id, bool asNoTracking);
    Task<T> GetByIdAsync(object id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
    Task<T> GetByIdAsync(object id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes, bool asNoTracking);
    Task<TProjectedType> GetProjectedByIdAsync<TProjectedType>(
       object id);
    Task<TProjectedType> GetProjectedByIdAsync<TProjectedType>(
        object id,
        Expression<Func<T, TProjectedType>> selectExpression);
    #endregion

    #region List

    Task<List<T>> GetListAsync();
    Task<List<T>> GetListAsync(bool asNoTracking);
    Task<List<T>> GetListAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
    Task<List<T>> GetListAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes, bool asNoTracking);
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> condition);
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> condition, bool asNoTracking);
    Task<List<T>> GetListAsync(
        Expression<Func<T, bool>> condition,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
        bool asNoTracking = false);
    Task<List<T>> GetListAsync(Specification<T> specification);
    Task<List<T>> GetListAsync(Specification<T> specification, bool asNoTracking);
    Task<PaginatedList<T>> GetPaginatedListAsync(Specification<T> specification);
    Task<PaginatedList<T>> GetPaginatedListAsync(Specification<T> specification, bool asNoTracking);
    Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>();
    Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
       Expression<Func<T, TProjectedType>> selectExpression);
    Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
      Expression<Func<T, bool>> condition);
    Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
        Expression<Func<T, bool>> condition,
        Expression<Func<T, TProjectedType>> selectExpression);
    Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
        Specification<T> specification);
    Task<List<TProjectedType>> GetProjectedListAsync<TProjectedType>(
       Specification<T> specification,
       Expression<Func<T, TProjectedType>> selectExpression);
    Task<PaginatedList<TProjectedType>> GetPaginatedProjectedListAsync<TProjectedType>(
      Specification<T> specification);
    Task<PaginatedList<TProjectedType>> GetPaginatedProjectedListAsync<TProjectedType>(
       Specification<T> specification,
       Expression<Func<T, TProjectedType>> selectExpression);
    #endregion

    #region Count
    Task<int> GetCountAsync();
    Task<int> GetCountAsync(params Expression<Func<T, bool>>[] conditions);
    Task<long> GetLongCountAsync();
    Task<long> GetLongCountAsync(params Expression<Func<T, bool>>[] conditions);
    #endregion

    #region Max
    Task<TProjectedType> MaxAsync<TProjectedType>(
           Expression<Func<T, TProjectedType>> selectExpression);
    Task<TProjectedType> MaxAsync<TProjectedType>(
           Expression<Func<T, bool>> condition,
           Expression<Func<T, TProjectedType>> selectExpression);
    #endregion

    #region Exists
    Task<bool> ExistsAsync();
    Task<bool> ExistsAsync(Expression<Func<T, bool>> condition);
    #endregion
}