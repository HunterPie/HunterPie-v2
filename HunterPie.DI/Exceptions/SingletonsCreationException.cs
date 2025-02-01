namespace HunterPie.DI.Exceptions;

public class SingletonsCreationException : Exception
{

    public readonly Exception[] InnerExceptions;

    public SingletonsCreationException(IEnumerable<Type> types, Exception[] innerExceptions)
        : base($"Failed to create instances for {string.Join(", ", types.Select(it => it.Name))}")
    {
        InnerExceptions = innerExceptions;
    }
}