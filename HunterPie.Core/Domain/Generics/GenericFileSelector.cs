using HunterPie.Core.Architecture;
using HunterPie.Core.Converters;
using HunterPie.Core.Settings.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HunterPie.Core.Domain.Generics;

[JsonConverter(typeof(FileSelectorConverter))]
public class GenericFileSelector : Bindable, IFileSelector
{
    private readonly string _filter;
    private readonly string _basePath;

    private string _current;
    public string Current
    {
        get => _current;
        set => SetValue(ref _current, value);
    }

    [JsonConstructor]
    public GenericFileSelector(string current)
    {
        Current = current;
    }

    public GenericFileSelector(string current, string filter, string basePath)
    {
        Current = current;
        _filter = filter;
        _basePath = basePath;
    }

    public IEnumerable<string> GetElements()
    {
        return !Directory.Exists(_basePath)
            ? Array.Empty<string>()
            : Directory.GetFiles(_basePath, _filter)
                .Select(it => Path.GetFileName(it)!);
    }

    public static implicit operator string(GenericFileSelector selector) => selector.Current;
}