using System;
using System.Windows.Forms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        AutoStart.Enable();

        var tray = new TrayIcon();
        Application.Run();
    }
}
