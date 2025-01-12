using HunterPie.Core.Settings.Adapters;
using System;

namespace HunterPie.Core.Settings.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public class GameConfigurationAdapterAttribute : Attribute
{
    public ISettingAdapter Adapter { get; init; }

    public GameConfigurationAdapterAttribute(Type adapterType)
    {
        Adapter = (ISettingAdapter)Activator.CreateInstance(adapterType);
    }
}