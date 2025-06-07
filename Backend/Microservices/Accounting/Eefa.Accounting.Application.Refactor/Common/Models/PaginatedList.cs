using System.Collections.Generic;
using System;

public class PaginatedList<T>
{
    public List<T> Data { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PaginatedList(List<T> data, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        PageSize = pageSize;
        Data = data;
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;


}