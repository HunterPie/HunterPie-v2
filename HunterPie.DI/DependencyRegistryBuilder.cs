using HunterPie.DI.Registry;

namespace HunterPie.DI;

public class DependencyRegistryBuilder
{
    public static DependencyRegistry Create()
    {
        return new DependencyRegistry();
    }
}