using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using Range = HunterPie.Core.Settings.Types.Range;

namespace HunterPie.UI.Architecture;

public class AutoVisibilityViewModel : ViewModel, IDisposable
{
    private const int MILLISECOND = 1000;
    private readonly Range _timeout;
    private readonly Timer _timer;

    private bool _isActive;
    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }

    public AutoVisibilityViewModel(Range timeout)
    {
        _timeout = timeout;
        _timer = new(_timeout.Current * MILLISECOND)
        {
            AutoReset = true,
        };

        SetupHooks();

        _timer.Start();
    }

    private void SetupHooks()
    {
        _timer.Elapsed += OnTimerElapsed;
        _timeout.PropertyChanged += OnTimeoutValueChanged;
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        if (!IsActive)
            return;

        IsActive = false;
    }

    protected void RefreshTimer()
    {
        if (_timer is null)
            return;

        IsActive = true;
        _timer.Stop();
        _timer.Start();
    }

    private void OnTimeoutValueChanged(object sender, PropertyChangedEventArgs e)
    {
        _timer.Interval = _timeout.Current * MILLISECOND;
        RefreshTimer();
    }

    protected void SetValueAndRefresh<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(property, value))
            return;

        RefreshTimer();
        SetValue(ref property, value, propertyName);
    }

    public virtual void Dispose()
    {
        _timer.Close();
        _timer.Elapsed -= OnTimerElapsed;
        _timer.Dispose();
        _timeout.PropertyChanged -= OnTimeoutValueChanged;
        GC.SuppressFinalize(this);
    }
}