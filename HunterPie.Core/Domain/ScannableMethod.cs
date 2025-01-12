using System;

namespace HunterPie.Core.Domain;

/// <summary>
/// Indicates the method will be invoked by <see cref="ScanManager"/> repetitively.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ScannableMethod : Attribute
{

    public readonly Type? DtoType;

    public ScannableMethod(Type? dtoType = null)
    {
        DtoType = dtoType;
    }
}