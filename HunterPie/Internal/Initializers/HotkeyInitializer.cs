using HunterPie.Core.Input;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Main.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace HunterPie.Internal.Initializers;

internal class HotkeyInitializer : IInitializer, IDisposable
{
    private readonly MainView _mainView;

    private static HwndSource? _source;
    private static IntPtr _hWnd;

    public HotkeyInitializer(MainView mainView)
    {
        _mainView = mainView;
    }

    public Task Init()
    {
        _hWnd = new WindowInteropHelper(_mainView)
            .EnsureHandle();

        _source = HwndSource.FromHwnd(_hWnd);

        _source?.AddHook(Hotkey.HwndHook);
        Hotkey.hWnd = _hWnd;

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _source?.RemoveHook(Hotkey.HwndHook);
        _source?.Dispose();
        _hWnd = IntPtr.Zero;
    }
}