using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;
public class InsectGlaiveExtractChangeEventArgs : EventArgs
{
    public KinsectBuff Extract { get; }

    public InsectGlaiveExtractChangeEventArgs(KinsectBuff extract)
    {
        Extract = extract;
    }
}