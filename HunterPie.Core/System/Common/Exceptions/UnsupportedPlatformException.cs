using System;

namespace HunterPie.Core.System.Common.Exceptions;
public class UnsupportedPlatformException : Exception
{
    public UnsupportedPlatformException() : base("Unsupported platform") { }
}