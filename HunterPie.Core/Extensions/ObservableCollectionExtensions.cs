using System;
using System.Collections.ObjectModel;

namespace HunterPie.Core.Extensions;

public static class ObservableCollectionExtensions
{
    public static void SortInPlace<T, O>(this ObservableCollection<T> collection, Func<T, O> selector) where O : IComparable<O>
    {
        for (int i = 1; i <= collection.Count; i++)
        {
            if (i > collection.Count - 1)
                break;

            if (selector(collection[i - 1]).CompareTo(selector(collection[i])) < 0)
                collection.Move(i - 1, i);
        }
    }
}