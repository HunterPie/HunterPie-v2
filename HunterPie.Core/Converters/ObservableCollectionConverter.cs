using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;

namespace HunterPie.Core.Converters;

public class ObservableCollectionConverter<T> : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(ObservableCollection<T>);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var existingCollection = (ObservableCollection<T>)existingValue;
        var collection = (ObservableCollection<T>)JsonSerializer.CreateDefault().Deserialize(reader, objectType);

        if (collection.Count < existingCollection.Count)
            for (int i = existingCollection.Count - 1; i >= collection.Count; i--)
                existingCollection.RemoveAt(i);

        if (existingCollection.Count > 0 && collection.Count >= existingCollection.Count)
            for (int i = 0; i < existingCollection.Count; i++)
                CopyToExistingRecursively(existingCollection[i], collection[i]);

        if (collection.Count > existingCollection.Count)
            for (int i = existingCollection.Count; i < collection.Count; i++)
                existingCollection.Add(collection[i]);

        return existingCollection;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => JsonSerializer.CreateDefault().Serialize(writer, value);

    private void CopyToExistingRecursively(object existingObject, object newObject)
    {
        Type objectType = existingObject.GetType();

        if (existingObject is ICollection)
            return;

        foreach (PropertyInfo property in objectType.GetProperties())
        {
            Type propertyType = property.PropertyType;
            if (property.CanWrite && (propertyType.IsPrimitive || propertyType == typeof(string)))
            {
                object value = property.GetValue(newObject);
                property.SetValue(existingObject, value);

                continue;
            }

            object existingValue = property.GetValue(existingObject);
            object newValue = property.GetValue(newObject);
            CopyToExistingRecursively(existingValue, newValue);
        }
    }
}