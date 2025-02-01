namespace HunterPie.Integrations.Services.Exceptions;

public class UnsupportedGameException : Exception
{
    public UnsupportedGameException(string name) : base($"Unsupported game {name}") { }
}