using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace QuerySorter
{
    public interface IQuerySorter<T, TSortName>
    {
        IOrderedQueryable<T> Sort(IQueryable<T> source, TSortName sortname, ListSortDirection direction);
        IOrderedEnumerable<T> Sort(IEnumerable<T> source, TSortName sortname, ListSortDirection direction);
    }
}