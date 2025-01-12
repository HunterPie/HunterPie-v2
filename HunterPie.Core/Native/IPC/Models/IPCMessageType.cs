namespace HunterPie.Core.Native.IPC.Models;

/// <summary>
/// IPC message types. See HunterPie.Native\Core\Server\Models\Messages.h for native definition.
/// </summary>
public enum IPCMessageType
{
    /// <summary>Requests IPC initialization. Wrapped by <see cref="RequestIPCInitializationMessage"/>.</summary>
    INIT_IPC_MEMORY_ADDRESSES,
    INIT_MH_HOOKS,
    GET_HUNT_STATISTICS,
    DELETE_HUNT_STATISTICS,
    CLEAR_HUNT_STATISTICS,
    UNKNOWN
}