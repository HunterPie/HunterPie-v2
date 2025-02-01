namespace HunterPie.DI.Exceptions;

public class DependencyNotRegisteredException : Exception
{
    public DependencyNotRegisteredException(Type type) : base($"Type {type.Name} has not been registered as dependency") { }
}