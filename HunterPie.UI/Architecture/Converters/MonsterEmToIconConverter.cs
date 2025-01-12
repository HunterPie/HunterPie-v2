using HunterPie.Core.Client;
using HunterPie.Core.Remote;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters;

public class MonsterEmToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string monsterEm = (string)value;

        if (monsterEm is null || monsterEm.Length == 0)
            return null;

        bool isRise = monsterEm.StartsWith("Rise");

        if (!isRise)
            monsterEm += "_ID";

        string imageName = $"Monsters/Icons/{monsterEm}.png";

        string path = Path.Combine(ClientInfo.ClientPath, @$"Assets/{imageName}");

        if (!File.Exists(path))
            _ = CDN.GetMonsterIconUrl(monsterEm);

        return new ImageSourceConverter().ConvertFromString($"pack://siteoforigin:,,,/Assets/Monsters/Icons/{monsterEm}.png");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}