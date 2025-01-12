using HunterPie.Core.Logger;
using System.Diagnostics;

namespace HunterPie.Domain.Logger;

internal class LogTracer : TraceListener
{
    public override void Write(string message)
    {

    }

    public override void WriteLine(string message) => Log.Error(message);
}