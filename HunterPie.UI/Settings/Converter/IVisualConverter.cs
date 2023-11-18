﻿using HunterPie.Core.Domain.Enums;
using System.Reflection;
using System.Windows;

namespace HunterPie.UI.Settings.Converter;

public interface IVisualConverter
{
    public FrameworkElement Build(GameProcess? game, object parent, PropertyInfo childInfo);
}
