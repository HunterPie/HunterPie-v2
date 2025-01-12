namespace HunterPie.DI.Registry;

internal record Dependency(
    Type Type,
    Func<object>? Activator
);