using HunterPie.Core.Architecture;
using HunterPie.Core.Converters;
using HunterPie.Core.Settings.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace HunterPie.Core.Domain.Generics;

[JsonConverter(typeof(FileSelectorConverter))]
public class GenericFolderSelector : Bindable, IFileSelector
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
    public GenericFolderSelector(string current)
    {
        Current = current;
    }

    public GenericFolderSelector(string current, string filter, string basePath)
    {
        Current = current;
        _filter = filter;
        _basePath = basePath;
    }

    public IEnumerable<string> GetElements()
    {
        return !Directory.Exists(_basePath)
            ? Array.Empty<string>()
            : Directory.GetDirectories(_basePath, _filter)
                .Select(it => Path.GetFileName(it)!);
    }

    public static implicit operator string(GenericFolderSelector selector) => selector.Current;
}