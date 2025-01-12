using HunterPie.Core.Client.Localization;
using System.Diagnostics.CodeAnalysis;

namespace HunterPie.Integrations.Poogie.Common.Models;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal enum PoogieErrorCode
{
    NOT_ERROR,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_GENERIC']")]
    ERROR_GENERIC,
    INVALID_PAYLOAD,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_INVALID_USERNAME']")]
    INVALID_USERNAME,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_INVALID_PASSWORD']")]
    INVALID_PASSWORD,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_INVALID_EMAIL']")]
    INVALID_EMAIL,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_INVALID_CREDENTIALS']")]
    INVALID_CREDENTIALS,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_USER_NOT_FOUND']")]
    USER_NOT_FOUND,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_USER_ALREADY_EXISTS']")]
    USER_ALREADY_EXISTS,
    INVALID_SESSION_TOKEN,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_UNVERIFIED_ACCOUNT']")]
    UNVERIFIED_ACCOUNT,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_INVALID_IMAGE']")]
    INVALID_IMAGE,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_FAILED_TO_UPDATE_AVATAR']")]
    FAILED_TO_UPDATE_AVATAR,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_AVATAR_SIZE_TOO_LARGE']")]
    AVATAR_SIZE_TOO_LARGE,

    INVALID_BACKUP_UPLOAD,
    BACKUP_TOO_LARGE,
    BACKUP_RATE_LIMIT,
    NOT_FOUND,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_GENERIC']")]
    INTERNAL_ERROR,

    [Localization("//Strings/Client/Integrations/Poogie[@Id='ERROR_INVALID_PASSWORD_RESET_CODE']")]
    INVALID_PASSWORD_RESET_CODE,

    UNKNOWN_ERROR
}