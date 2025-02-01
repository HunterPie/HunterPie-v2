using System;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Memory;

public interface IMemoryReaderAsync
{
    public Task<string> ReadAsync(
        IntPtr address,
        int length,
        Encoding? encoding = null);

    public Task<T> ReadAsync<T>(IntPtr address) where T : struct;

    public Task<T[]> ReadAsync<T>(IntPtr address, int count) where T : struct;

    public Task<IntPtr> ReadPtrAsync(IntPtr address, int[] offsets);

    public Task<IntPtr> ReadAsync(IntPtr address, int[] offsets);

    public Task<T> DerefAsync<T>(IntPtr address, int[] offsets) where T : struct;

    public Task<T> DerefPtrAsync<T>(IntPtr address, int[] offsets) where T : struct;
}