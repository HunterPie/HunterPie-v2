using System;
using System.Collections.Generic;
using System.Reflection;

namespace HunterPie.Core.Architecture.Events;

public interface ISmartEvent : IDisposable
{
    public string Name { get; }
    public List<MethodInfo> References { get; }
}