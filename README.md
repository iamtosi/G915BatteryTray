# G915-BatteryTray — Logitech G915 Battery Monitor without G HUB

![tray](https://i.imgur.com/2SbJWrm.png)

A small Windows utility that shows the battery level of the Logitech G915 **without installing** Logitech G HUB, its background services, or the unwanted audio “enhancements” that only make things worse.

Minimal features, maximum usefulness: clean tray icon, battery status, proper charging indication when connected via cable.


## Features

- Shows approximate G915 battery level when connected via Lightspeed (dongle, PID `046D:C547`)
- Shows ⚡ when the keyboard is charging via USB cable (PID `046D:C357`)
- 5 battery icons (drawn by myself):
  - `4.ico` → 80–100%
  - `3.ico` → ~75%
  - `2.ico` → ~50%
  - `1.ico` → ~25%
  - `0.ico` → charging (⚡)
- 0 CPU usage, 2–3 MB RAM
- Pure C# (.NET 8), no G HUB dependency
- Runs quietly and reliably in the system tray


## Why this exists

Logitech G HUB is a bloated application that:

- installs its own drivers without asking  
- forces audio enhancements that ruin the sound  
- spawns multiple background processes  
- sometimes doesn’t even show battery level  

Meanwhile, all we need is *one number*.  
This tool does exactly that — and nothing else.


## Installation

Build:

```bash
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
```

You’ll get a single `.exe` along with the **Icons** folder:

```
bin/Release/net8.0-windows/win-x64/publish/
```

Drop it into startup if you want.


## How it works

### **Lightspeed mode (dongle, PID `046D:C547`)**
The keyboard exposes **HID++ 2.0 Battery Feature Reports**.  
The application reads these directly — no G HUB needed.

### **USB wired mode (PID `046D:C357`)**
In wired mode HID++ is **disabled at hardware level**.  
The keyboard becomes a normal USB device and **does NOT expose battery percentage**.

Therefore, the tray only shows:

- "⚡"
- and the charging icon

This matches G HUB behavior, minus all the unnecessary baggage.


## Project structure

```
BatteryTray/
│
├── Program.cs
├── TrayIcon.cs
├── HidLogitech.cs
├── Autostart.cs
├── BatteryTray.csproj
│
└── Icons/
     ├── 0.ico   (charging)
     ├── 1.ico   (25%)
     ├── 2.ico   (50%)
     ├── 3.ico   (75%)
     └── 4.ico   (full)
```


## .gitignore

Recommended:

```
bin/
obj/
.vs/
*.user
*.pdb
*.db
*.cache
publish/
```


## License

MIT — do whatever you want.
