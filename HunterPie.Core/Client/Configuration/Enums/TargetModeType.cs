using HunterPie.Core.Client.Localization;

namespace HunterPie.Core.Client.Configuration.Enums;

public enum TargetModeType
{
    [Localization("//Strings/Client/Enums/Enum[@Id='TARGET_MODE_LOCK_ON_STRING']")]
    LockOn,

    [Localization("//Strings/Client/Enums/Enum[@Id='TARGET_MODE_MAP_PIN_STRING']")]
    MapPin,

    [Localization("//Strings/Client/Enums/Enum[@Id='TARGET_MODE_AUTO_QUEST_STRING']")]
    AutoQuest,

    [Localization("//Strings/Client/Enums/Enum[@Id='TARGET_MODE_INFER_STRING']")]
    Infer
}