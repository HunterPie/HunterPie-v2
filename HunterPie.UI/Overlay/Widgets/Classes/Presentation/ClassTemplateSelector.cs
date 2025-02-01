using HunterPie.Core.Game.Enums;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Overlay.Widgets.Classes.Presentation;

#nullable enable
public class ClassTemplateSelector : DataTemplateSelector
{
    public DataTemplate? InsectGlaiveTemplate { get; set; }
    public DataTemplate? ChargeBladeTemplate { get; set; }
    public DataTemplate? DualBladesTemplate { get; set; }
    public DataTemplate? SwitchAxeTemplate { get; set; }
    public DataTemplate? LongswordTemplate { get; set; }
    public DataTemplate? EmptyTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        return item is IClassViewModel viewModel
            ? viewModel.WeaponId switch
            {
                Weapon.InsectGlaive => InsectGlaiveTemplate,
                Weapon.ChargeBlade => ChargeBladeTemplate,
                Weapon.DualBlades => DualBladesTemplate,
                Weapon.SwitchAxe => SwitchAxeTemplate,
                Weapon.Longsword => LongswordTemplate,
                _ => throw new NotImplementedException($"Missing implementation for {viewModel.WeaponId}")
            }
            : EmptyTemplate;
    }
}