using System.Runtime.InteropServices;

namespace HunterPie.Platforms.Windows.Api.Adv;
internal class AdvApi32
{
    private const string ADV_API_32 = "advapi32.dll";

    public enum CredentialType
    {
        Generic = 1,
        DomainPassword,
        DomainCertificate,
        DomainVisiblePassword,
        GenericCertificate,
        Maximum,
        MaximumEx = Maximum + 1000,
    }

    public enum CredentialPersistence : uint
    {
        Session = 1,
        LocalMachine,
        Enterprise
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Credential
    {
        public int Flags;
        public CredentialType Type;
        public IntPtr TargetName;
        public IntPtr Comment;
        public long LastWritten;
        public uint CredentialBlobSize;
        public IntPtr CredentialBlob;
        public uint Persist;
        public uint AttributeCount;
        public IntPtr Attributes;
        public IntPtr TargetAlias;
        public IntPtr Username;
    }

    [DllImport(ADV_API_32, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool CredReadW(
        string target,
        CredentialType type,
        int reservedFlag,
        out IntPtr credentialPtr
    );

    [DllImport(ADV_API_32, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool CredDeleteW(
        string target,
        CredentialType type,
        int flags
    );

    [DllImport(ADV_API_32, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool CredWriteW(
        [In] ref Credential credential,
        [In] uint flags
    );

    [DllImport(ADV_API_32, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool CredEnumerate(
        string filter,
        int flag,
        out int count,
        out IntPtr pCredentials
    );

    [DllImport(ADV_API_32, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool CredFree([In] IntPtr credential);
}