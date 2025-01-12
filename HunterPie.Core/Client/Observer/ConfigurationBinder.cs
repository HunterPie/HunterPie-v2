using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace HunterPie.Core.Client.Observer;

#nullable enable
public static class ConfigurationBinder
{
    private static readonly IList CachedEmptyList = Array.Empty<object>();
    public delegate void ConfigurationBinderHandler();

    /// <summary>
    /// Binds the configuration properties to the specific action
    /// </summary>
    /// <param name="configuration">Configuration to be bound</param>
    /// <param name="handler">Action to be executed when an element dispatches any notification</param>
    public static void Bind(
        object configuration,
        ConfigurationBinderHandler handler
    )
    {
        void PropertyHandler(object? source, PropertyChangedEventArgs e) => handler();

        void CollectionHandler(object? source, NotifyCollectionChangedEventArgs e)
        {
            IList oldItems = e.OldItems ?? CachedEmptyList;
            IList newItems = e.NewItems ?? CachedEmptyList;

            foreach (object? oldElement in oldItems)
                if (oldElement is not null)
                    BindElement(oldElement, PropertyHandler, CollectionHandler, true);

            foreach (object? newElement in newItems)
                if (newElement is not null)
                    BindElement(newElement, PropertyHandler, CollectionHandler, false);
        }

        BindElement(configuration, PropertyHandler, CollectionHandler, false);
    }

    private static void BindElement(
        object element,
        PropertyChangedEventHandler propertyChangedHandler,
        NotifyCollectionChangedEventHandler collectionChangedHandler,
        bool shouldUnbind
    )
    {
        Type type = element.GetType();

        if (type.IsPrimitive || type.IsEnum || type == typeof(string))
            return;

        if (element is INotifyPropertyChanged observable)
            BindProperty(observable, propertyChangedHandler, shouldUnbind);

        if (element is INotifyCollectionChanged observableCollection)
        {
            BindCollection(observableCollection, collectionChangedHandler, propertyChangedHandler, shouldUnbind);
            return;
        }

        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.GetIndexParameters().Length > 0)
                continue;

            object? propertyValue = property.GetValue(element);

            if (propertyValue is null)
                continue;

            BindElement(propertyValue, propertyChangedHandler, collectionChangedHandler, shouldUnbind);
        }
    }

    private static void BindProperty(
        INotifyPropertyChanged observable,
        PropertyChangedEventHandler handler,
        bool shouldUnbind
    )
    {
        if (shouldUnbind)
            observable.PropertyChanged -= handler;
        else
            observable.PropertyChanged += handler;
    }

    private static void BindCollection(
        INotifyCollectionChanged observable,
        NotifyCollectionChangedEventHandler collectionHandler,
        PropertyChangedEventHandler propertyHandler,
        bool shouldUnbind
    )
    {
        if (shouldUnbind)
            observable.CollectionChanged -= collectionHandler;
        else
            observable.CollectionChanged += collectionHandler;

        if (observable is not IEnumerable enumerable)
            return;

        foreach (object? element in enumerable)
        {
            if (element is null)
                continue;

            BindElement(element, propertyHandler, collectionHandler, shouldUnbind);
        }
    }
}