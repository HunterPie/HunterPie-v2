namespace HunterPie.DI.Exceptions;

public class ManyPossibleDependenciesException : Exception
{
    public Type Type { get; }
    public Type[] Possibilities { get; }

    public ManyPossibleDependenciesException(Type type, Type[] possibilities) : base($"Too many possible options to satisfy dependency '{type.Name}'")
    {
        Type = type;
        Possibilities = possibilities;
    }
}