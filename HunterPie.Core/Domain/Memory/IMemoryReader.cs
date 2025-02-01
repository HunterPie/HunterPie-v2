using System.Text;

namespace HunterPie.Core.Domain.Memory;

public interface IMemoryReader
{
    public string Read(long address, uint length, Encoding encoding = null);
    public T Read<T>(long address) where T : struct;
    public T[] Read<T>(long address, uint count) where T : struct;
    public long ReadPtr(long address, int[] offsets);
    public long Read(long address, int[] offsets);
    public T Deref<T>(long address, int[] offsets) where T : struct;
    public T DerefPtr<T>(long address, int[] offsets) where T : struct;
}