using Raylib_cs;
using static Raylib_cs.Raylib;

using rbx.Generator;

using static rbx.HelpModule;
using static rbx.Keybinds;
using static rbx.ScreenShot;
using rbx.Colors;

using System;
using System.Numerics;

namespace rbx
{
    static class Program
    {
        public static void Main()
        {
            Raylib.InitWindow(800, 480, "Raylib Rubiks");

            LCG lcg = new LCG((uint)new Random().Next());
            Colors.SystemPalette.bg = lcg.GenColor();

            Raylib.SetExitKey(0);
            SetTargetFPS(GetMonitorRefreshRate(GetCurrentMonitor()));

            while (!Raylib.WindowShouldClose())
            {
                if(Keybinds.CloseKey())
                {
                    Raylib.CloseWindow();
                }
                if(Keybinds.ScreenShotKey())
                {
                    ScreenShot.Capture();
                }
                if(Keybinds.FullScreenKey())
                {
                    Raylib.ToggleFullscreen();
                }
                if(Keybinds.HelpKey()) {
                    HelpModule.ToggleHelp();
                }
                if(HelpModule.HelpActive) {
                } else {
                    Raylib.BeginDrawing();

                    Raylib.ClearBackground(Colors.SystemPalette.bg);

                    Raylib.DrawText("Cock", 20, 20, 20, Colors.SystemPalette.fg);

                    Raylib.EndDrawing();
                }
            }
        }
    }
}
