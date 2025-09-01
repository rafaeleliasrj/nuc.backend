using System;

namespace Avvo.Core.Commons.Pagination;

public class PagedResultBase
{
    public long CurrentPage { get; set; }
    public long PageCount { get; set; }
    public long PageSize { get; set; }
    public long RowCount { get; set; }
    public long FirstRowOnPage
    {
        get { return (CurrentPage - 1) * PageSize + 1; }
    }
    public long LastRowOnPage
    {
        get { return Math.Min(CurrentPage * PageSize, RowCount); }
    }
}
