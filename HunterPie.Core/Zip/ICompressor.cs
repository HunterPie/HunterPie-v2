using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Core.Zip;

public interface ICompressor
{
    public Task<bool> Compress(string path, IEnumerable<string> files);
}