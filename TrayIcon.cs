using System;
using System.Drawing;
using System.Windows.Forms;

public class TrayIcon : IDisposable
{
    private readonly NotifyIcon _icon;
    private readonly System.Windows.Forms.Timer _timer;

    private string _lastState = "";

    private Icon ico0; // charging
    private Icon ico1; // 25%
    private Icon ico2; // 50%
    private Icon ico3; // 75%
    private Icon ico4; // full

    public TrayIcon()
    {
        ico0 = new Icon("Icons/0.ico");
        ico1 = new Icon("Icons/1.ico");
        ico2 = new Icon("Icons/2.ico");
        ico3 = new Icon("Icons/3.ico");
        ico4 = new Icon("Icons/4.ico");

        _icon = new NotifyIcon
        {
            Icon = ico4,
            Visible = true,
            Text = "BatteryTray"
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add("Exit", null, (_, _) => Application.Exit());
        _icon.ContextMenuStrip = menu;

        _timer = new System.Windows.Forms.Timer();
        _timer.Interval = 5000;
        _timer.Tick += (_, _) => UpdateBattery();
        _timer.Start();

        UpdateBattery();
    }

    private void UpdateBattery()
    {
        string? state = HidLogitech.GetBatteryG915();

        if (state == null)
        {
            _icon.Icon = ico1;
            _icon.Text = "G915: —";
            _lastState = "";
            return;
        }

        _icon.Text = $"G915: {state}";

        if (state == "⚡")
        {
            _icon.Icon = ico0;

            if (_lastState != state)
            {
                _icon.ShowBalloonTip(1200, "G915", "⚡ Charging", ToolTipIcon.Info);
            }

            _lastState = state;
            return;
        }

        // ---------- Percentage-based icons ----------
        Icon chosenIcon = ico1;

        if (state.StartsWith(">") || state.StartsWith("~80") || state.StartsWith("~75") || state.StartsWith("~85"))
            chosenIcon = ico4;     // full
        else if (state.Contains("~50"))
            chosenIcon = ico2;     // 50
        else if (state.Contains("~40") || state.Contains("~30"))
            chosenIcon = ico3;     // 75? (your set has 75 for mid-high)
        else if (state.Contains("~20") || state.Contains("~10") || state.Contains("<10"))
            chosenIcon = ico1;     // 25

        _icon.Icon = chosenIcon;

        // low battery warnings
        if ((_lastState != state) && (state.Contains("~10") || state.Contains("<10")))
        {
            _icon.ShowBalloonTip(
                2000,
                "G915 Battery Low",
                "G915 Battery Low",
                ToolTipIcon.Warning
            );
        }

        _lastState = state;
    }

    public void Dispose()
    {
        ico0?.Dispose();
        ico1?.Dispose();
        ico2?.Dispose();
        ico3?.Dispose();
        ico4?.Dispose();

        _icon?.Dispose();
        _timer?.Dispose();
    }
}
