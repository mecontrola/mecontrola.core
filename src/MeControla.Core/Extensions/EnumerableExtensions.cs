﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MeControla.Core.Extensions
{
    public static class EnumerableExtensions
    {
#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
            => enumerable == null || !enumerable.Any();

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static bool IsNotNullAndAny<T>(this IEnumerable<T> enumerable)
            => enumerable != null && enumerable.Any();

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static IList<T> ToListOrEmpty<T>(this IEnumerable<T> enumerable)
            => enumerable.IsNullOrEmpty() ? [] : enumerable.ToList();

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static IList<TResult> SelectToListOrEmpty<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
            => source.IsNullOrEmpty() ? [] : source.Select(selector).ToList();

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static IList<TResult> SelectToListOrEmpty<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
            => source.IsNullOrEmpty() ? [] : source.Select(selector).ToList();

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
                action(item);
        }

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static IEnumerable<int> FindIndexAll<T>(this IEnumerable<T> data, Predicate<T> match)
            => Enumerable.Range(0, data.Count())
                         .Where(i => match(data.ElementAt(i)));

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
            => source.Select(selector)
                     .Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static TimeSpan Average<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
            => source.Sum(selector) / source.Count();

#if !DEBUG
        [System.Diagnostics.DebuggerStepThrough]
#endif
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            return new ObservableCollection<T>(source);
        }
    }
}