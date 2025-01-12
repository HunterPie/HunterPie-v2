using System;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Memory;

public interface IMemoryWriterAsync
{
    public Task WriteAsync<T>(IntPtr address, T[] data) where T : struct;
    public Task InjectAsmAsync(IntPtr address, byte[] asm);
    public Task InjectAsync(string dll);
}