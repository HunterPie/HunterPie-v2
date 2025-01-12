namespace HunterPie.DI.Exceptions;

public class DependencyArgumentException : Exception
{
    public DependencyArgumentException(Type type, Exception innerException) : base(
        $"Failed to create instance of {type.Name} due to argument exception", innerException)
    { }
}