using System.Collections.Generic;

namespace HunterPie.Update.Service.Entity;

internal record UpdateFileChecksums(
    Dictionary<string, string> Remote,
    Dictionary<string, string> Local
);