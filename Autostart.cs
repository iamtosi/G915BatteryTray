using Microsoft.Win32;
using System;

public static class AutoStart
{
    public static void Enable()
    {
        string exe = Environment.ProcessPath!;
        using var key = Registry.CurrentUser.OpenSubKey(
            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        key.SetValue("LogiBattery", exe);
    }
}
