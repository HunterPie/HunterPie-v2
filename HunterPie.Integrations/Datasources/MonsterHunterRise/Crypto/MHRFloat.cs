namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Crypto;

public static class MHRCrypto
{

    public static float DecodeHealth(uint value, uint key)
    {
        Span<byte> decoded = stackalloc byte[4];

        byte edx, eax;
        uint ecx, r8d;

        edx = (byte)(value & 0xFF);
        ecx = key + (key * 2);
        edx += (byte)ecx;
        eax = (byte)ecx;
        r8d = ecx;
        ecx = eax * 0x3Fu;

        r8d += 4;
        key = r8d;
        r8d += 4;

        // First byte
        eax = (byte)(key >> 8);
        key = (key & 0x00FFFFFF) | (r8d & 0xFF000000);
        r8d += 4;
        edx ^= (byte)ecx;
        ecx = eax * 0x3Fu;
        decoded[0] = edx;

        // Second byte
        edx = (byte)(value >> 8);
        edx += eax;
        eax = (byte)(key >> 16);
        key = r8d;
        r8d = 0;
        edx ^= (byte)ecx;
        ecx = eax * 0x3Fu;
        decoded[1] = edx;

        // Third byte
        edx = (byte)(value >> 16);
        edx += eax;
        eax = (byte)(key >> 24);
        edx ^= (byte)ecx;
        ecx = eax * 0x3Fu;
        decoded[2] = edx;

        // Fourth byte
        edx = (byte)(value >> 24);
        edx += eax;
        edx ^= (byte)ecx;
        decoded[3] = edx;

        return BitConverter.ToSingle(decoded);
    }
}