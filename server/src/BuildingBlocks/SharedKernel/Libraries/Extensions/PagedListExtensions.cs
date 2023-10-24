using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;

namespace SharedKernel.Libraries;

public static class PagedListExtensions
{
    public static IPagedList<TEntity> ToPagedList<TEntity>(
        this IEnumerable<TEntity> source,
        int pageIndex,
        int pageSize,
        int indexFrom = 1) where TEntity : class
    {
        if (indexFrom > pageIndex)
        {
            pageIndex = indexFrom;
        }

        var totalCount = source.Count();
        var items = source.Skip((pageIndex - indexFrom) * pageSize).Take(pageSize).ToList();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedList<TEntity>(pageIndex, pageSize, indexFrom, totalCount, totalPages, items);
    }

    public static async Task<IPagedList<TEntity>> ToPagedListAsync<TEntity>(
        this IQueryable<TEntity> source,
        int pageIndex,
        int pageSize,
        int indexFrom = 1,
        CancellationToken cancellationToken = default) where TEntity : class
    {
        if (indexFrom > pageIndex)
        {
            pageIndex = indexFrom;
        }

        int totalCount = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await source.Skip((pageIndex - indexFrom) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedList<TEntity>(pageIndex, pageSize, indexFrom, totalCount, totalPages, items);
    }
    
    public static bool NotNullOrEmpty<T>(this IPagedList<T> list)
    {
        if (list == null || list.Items == null)
            return false;

        return true;
    }
}