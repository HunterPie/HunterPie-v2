using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace HunterPie.Core.Architecture.Collections;

public class ObservableCollectionRange<T> : ObservableCollection<T>
{
    public void Replace(IEnumerable<T> collection)
    {
        Items.Clear();
        var list = collection.ToList();

        foreach (T element in list)
            Items.Add(element);

        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        OnPropertyChanged(new PropertyChangedEventArgs("Items[]"));
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}