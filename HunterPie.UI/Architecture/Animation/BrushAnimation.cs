using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using BrushColors = System.Windows.Media.Brushes;

namespace HunterPie.UI.Architecture.Animation;

// Animates brushes
// Credits: https://stackoverflow.com/questions/8096852/brush-to-brush-animation
public class BrushAnimation : AnimationTimeline
{
    public override Type TargetPropertyType => typeof(Brush);

    public override object GetCurrentValue(object defaultOriginValue,
                                           object defaultDestinationValue,
                                           AnimationClock animationClock)
    {
        return GetCurrentValue(defaultOriginValue as Brush,
                               defaultDestinationValue as Brush,
                               animationClock);
    }
    public object GetCurrentValue(Brush defaultOriginValue,
                                  Brush defaultDestinationValue,
                                  AnimationClock animationClock)
    {
        if (!animationClock.CurrentProgress.HasValue)
            return BrushColors.Transparent;

        //use the standard values if From and To are not set 
        //(it is the value of the given property)
        defaultOriginValue = From ?? defaultOriginValue;
        defaultDestinationValue = To ?? defaultDestinationValue;

        return animationClock.CurrentProgress.Value switch
        {
            0 => defaultOriginValue,
            1 => defaultDestinationValue,
            _ => new VisualBrush(new Border()
            {
                Width = 1,
                Height = 1,
                Background = defaultOriginValue,
                Child = new Border()
                {
                    Background = defaultDestinationValue,
                    Opacity = animationClock.CurrentProgress.Value,
                }
            })
        };
    }

    protected override Freezable CreateInstanceCore() => new BrushAnimation();

    //we must define From and To, AnimationTimeline does not have this properties
    public Brush From
    {
        get => (Brush)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }
    public Brush To
    {
        get => (Brush)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public static readonly DependencyProperty FromProperty =
        DependencyProperty.Register("From", typeof(Brush), typeof(BrushAnimation));
    public static readonly DependencyProperty ToProperty =
        DependencyProperty.Register("To", typeof(Brush), typeof(BrushAnimation));
}