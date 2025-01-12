using System.Drawing;
using System.Windows.Forms;

namespace HunterPie.Internal.Tray;

public class TrayService
{
    private readonly ContextMenuStrip contextMenuStrip = new();
    private readonly NotifyIcon notifyIcon = new();
    private static TrayService _instance;

    private TrayService(
        string tooltip,
        string text,
        Icon icon
    )
    {
        notifyIcon.BalloonTipTitle = tooltip;
        notifyIcon.Text = text;
        notifyIcon.Icon = icon;
        notifyIcon.Visible = true;
        notifyIcon.ContextMenuStrip = contextMenuStrip;
    }

    internal static void Initialize(
        string tooltip,
        string text,
        Icon icon
    ) => _instance = new(tooltip, text, icon);

    internal static void Dispose()
    {
        if (_instance is null)
            return;

        _instance.contextMenuStrip.Dispose();
        _instance.notifyIcon.Dispose();

        _instance = null;
    }

    public static void AddDoubleClickHandler(MouseEventHandler callback) => _instance.notifyIcon.MouseDoubleClick += callback;

    public static ToolStripMenuItem AddItem(string name) => _instance.contextMenuStrip.Items.Add(name) as ToolStripMenuItem;

    public static ToolStripMenuItem AddMenuItem(string name, ToolStripMenuItem menu) => menu.DropDownItems.Add(name) as ToolStripMenuItem;
}