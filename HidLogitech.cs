using HidSharp;
using System;
using System.Linq;

public static class HidLogitech
{
    private const int VID = 0x046D;
    private const int PID_WIRELESS = 0xC547; // G915 Lightspeed
    private const int PID_WIRED    = 0xC357; // G915 USB cable

    public static int? DebugLastRawG915 = null;

    public static string? GetBatteryG915()
    {
        if (IsWired())
            return "⚡ charging";

        int? raw = ReadBattery(PID_WIRELESS);
        if (!raw.HasValue) return null;

        return NormalizeG915ToDisplay(raw.Value);
    }

    private static bool IsWired()
    {
        return DeviceList.Local
            .GetHidDevices(VID, PID_WIRED)
            .Any();
    }

    public static string NormalizeG915ToDisplay(int raw)
    {
        if (raw >= 0x8F) return ">85%";
        if (raw >= 0x8E) return "~80%";
        if (raw >= 0x8C) return "~75%";
        if (raw >= 0x8A) return "~70%";
        if (raw >= 0x88) return "~65%";
        if (raw >= 0x86) return "~60%";
        if (raw == 0x80) return "~50%";
        if (raw >= 0x78) return "~40%";
        if (raw >= 0x70) return "~30%";
        if (raw >= 0x65) return "~20%";
        if (raw >= 0x58) return "~10%";
        return "<10%";
    }

    private static int? ReadBattery(int pid)
    {
        var devices = DeviceList.Local
            .GetHidDevices(VID, pid)
            .ToList();

        if (devices.Count == 0)
            return null;

        byte[] request = new byte[] { 0x10, 0x04 };
        byte[] buffer = new byte[64];

        foreach (var dev in devices)
        {
            try
            {
                using var stream = dev.Open();

                stream.Write(request, 0, request.Length);
                int read = stream.Read(buffer, 0, buffer.Length);

                if (read > 3 && buffer[0] == 0x10 && buffer[1] == 0x04)
                {
                    DebugLastRawG915 = buffer[2];
                    return buffer[2];
                }
            }
            catch { }
        }

        return null;
    }
}
