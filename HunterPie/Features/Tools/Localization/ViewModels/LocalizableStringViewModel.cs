using HunterPie.UI.Architecture;

namespace HunterPie.Features.Tools.Localization.ViewModels;

internal class LocalizableStringViewModel : ViewModel
{
    private string _id;
    private string _original;
    private string _localized;

    public string Id { get => _id; set => SetValue(ref _id, value); }
    public string Original { get => _original; set => SetValue(ref _original, value); }
    public string Localized { get => _localized; set => SetValue(ref _localized, value); }
}