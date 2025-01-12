using System.Runtime.CompilerServices;
using System.Windows;
#if DEBUG
using System.Windows.Markup;
#endif

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

#if DEBUG
[assembly: XmlnsDefinition("DEBUG_MODE", "HunterPie")]
#endif

[assembly: InternalsVisibleTo("HunterPie")]