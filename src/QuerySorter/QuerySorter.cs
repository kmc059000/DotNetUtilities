using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace QuerySorter
{
    public abstract class QuerySorter<T, TSortName> : IQuerySorter<T, TSortName>
    {
        protected abstract IEnumerable<SortExpression<T>> GetSortExpressions(TSortName sortname, ListSortDirection direction);

        public IOrderedQueryable<T> Sort(IQueryable<T> source, TSortName sortname, ListSortDirection direction)
        {
            var expressions = GetSortExpressions(sortname, direction).ToList();

            IOrderedQueryable<T> ordered = null;

            foreach (var expression in expressions)
            {
                ordered = ordered == null
                    ? expression.Apply(source)
                    : expression.Apply(ordered);
            }

            return ordered;
        }

        public IOrderedEnumerable<T> Sort(IEnumerable<T> source, TSortName sortname, ListSortDirection direction)
        {
            var expressions = GetSortExpressions(sortname, direction).ToList();

            IOrderedEnumerable<T> ordered = null;

            foreach (var expression in expressions)
            {
                ordered = ordered == null
                    ? expression.Apply(source)
                    : expression.Apply(ordered);
            }

            return ordered;
        }

        protected SortExpression<T> SortBy<TProperty>(Expression<Func<T, TProperty>> expression, ListSortDirection direction)
        {
            return new SortExpression<T, TProperty>
            {
                Expression = expression,
                Direction = direction
            };
        }

        protected ListSortDirection OppositeDirection(ListSortDirection direction)
        {
            switch (direction)
            {
                case ListSortDirection.Ascending:
                    return ListSortDirection.Descending;
                case ListSortDirection.Descending:
                    return ListSortDirection.Ascending;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        protected abstract class SortExpression<TSource>
        {
            public abstract IOrderedQueryable<TSource> Apply(IQueryable<TSource> source);
            public abstract IOrderedQueryable<TSource> Apply(IOrderedQueryable<TSource> source);

            public abstract IOrderedEnumerable<TSource> Apply(IEnumerable<TSource> source);
            public abstract IOrderedEnumerable<TSource> Apply(IOrderedEnumerable<TSource> source);
        }

        protected class SortExpression<TSource, TProperty> : SortExpression<TSource>
        {
            public ListSortDirection Direction { get; set; }
            public Expression<Func<TSource, TProperty>> Expression { get; set; }

            public override IOrderedQueryable<TSource> Apply(IQueryable<TSource> source)
            {
                switch (Direction)
                {
                    case ListSortDirection.Ascending:
                        return source.OrderBy(Expression);
                    case ListSortDirection.Descending:
                        return source.OrderByDescending(Expression);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public override IOrderedQueryable<TSource> Apply(IOrderedQueryable<TSource> source)
            {
                switch (Direction)
                {
                    case ListSortDirection.Ascending:
                        return source.ThenBy(Expression);
                    case ListSortDirection.Descending:
                        return source.ThenByDescending(Expression);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public override IOrderedEnumerable<TSource> Apply(IEnumerable<TSource> source)
            {
                switch (Direction)
                {
                    case ListSortDirection.Ascending:
                        return source.OrderBy(Expression.Compile());
                    case ListSortDirection.Descending:
                        return source.OrderByDescending(Expression.Compile());
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public override IOrderedEnumerable<TSource> Apply(IOrderedEnumerable<TSource> source)
            {
                switch (Direction)
                {
                    case ListSortDirection.Ascending:
                        return source.ThenBy(Expression.Compile());
                    case ListSortDirection.Descending:
                        return source.ThenByDescending(Expression.Compile());
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
