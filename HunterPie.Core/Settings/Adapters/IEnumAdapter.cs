using HunterPie.Core.Domain.Enums;

namespace HunterPie.Core.Settings.Adapters;

public interface IEnumAdapter : ISettingAdapter
{
    public object[] GetValues(GameProcessType game);
}