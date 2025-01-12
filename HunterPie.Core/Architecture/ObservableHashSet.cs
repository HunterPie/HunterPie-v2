using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HunterPie.Core.Architecture;

static class ObservableHashSetCache
{
    public static readonly NotifyCollectionChangedEventArgs ResetChangedEventArgs = new(NotifyCollectionChangedAction.Reset);
}

public sealed class ObservableHashSet<T> : ISet<T>, INotifyPropertyChanged, INotifyCollectionChanged
{
    private readonly HashSet<T> _set;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    public bool IsReadOnly => false;

    public ObservableHashSet()
    {
        _set = new HashSet<T>();
    }

    public ObservableHashSet(IEnumerable<T> collection)
    {
        _set = new HashSet<T>(collection);
    }

    public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _set.GetEnumerator();

    public void Add(T item) => AddIfNotPresent(item);

    void ICollection<T>.Add(T item) => AddIfNotPresent(item);

    bool ISet<T>.Add(T item) => AddIfNotPresent(item);

    public void Clear()
    {
        _set.Clear();

        Count = _set.Count;
        NotifyCollectionChanged(ObservableHashSetCache.ResetChangedEventArgs);
    }

    public bool Remove(T item)
    {
        bool status = _set.Remove(item);

        Count = _set.Count;
        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        return status;
    }

    public void ExceptWith(IEnumerable<T> other)
    {
        _set.ExceptWith(other);

        Count = _set.Count;
        NotifyCollectionChanged(ObservableHashSetCache.ResetChangedEventArgs);
    }

    public void IntersectWith(IEnumerable<T> other)
    {
        _set.IntersectWith(other);

        Count = _set.Count;
        NotifyCollectionChanged(ObservableHashSetCache.ResetChangedEventArgs);
    }

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        _set.SymmetricExceptWith(other);

        Count = _set.Count;
        NotifyCollectionChanged(ObservableHashSetCache.ResetChangedEventArgs);
    }

    public void UnionWith(IEnumerable<T> other)
    {
        _set.UnionWith(other);

        Count = _set.Count;
        NotifyCollectionChanged(ObservableHashSetCache.ResetChangedEventArgs);
    }

    public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

    public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

    public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);

    public bool Contains(T item) => _set.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _set.CopyTo(array, arrayIndex);

    private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(this, args);
    }

    private bool AddIfNotPresent(T item)
    {
        bool status = _set.Add(item);

        Count = _set.Count;
        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));

        return status;
    }

    private void SetValue<TField>(ref TField field, TField value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<TField>.Default.Equals(field, value))
            return;

        field = value;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}