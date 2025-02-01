using HunterPie.Core.Client.Localization.Entity;

namespace HunterPie.Core.Client.Localization;

public interface IScopedLocalizationRepository
{
    public LocalizationData FindBy(string id);

    public string FindStringBy(string id);
}