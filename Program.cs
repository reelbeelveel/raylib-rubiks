using Raylib_cs;
using static Raylib_cs.Raylib;
using hrv.ShellWrapper;
using hrv.Keybinds;
using hrv.Generator;
using hrv.HelpModule;
using System;
using System.Numerics;

namespace HelloWorld
{
    static class Program
    {
        public static void Main()
        {
            Raylib.InitWindow(800, 480, "HRV");

            LCG lcg = new LCG((uint)new Random().Next());
            Color bg = lcg.GenColor();

            Raylib.SetExitKey(0);
            SetTargetFPS(GetMonitorRefreshRate(GetCurrentMonitor()));

            Vector2 pos = new Vector2(12, 12);
            int size = 20;
            while (!Raylib.WindowShouldClose())
            {
                if(HRVKeybinds.CloseKey())
                {
                    Raylib.CloseWindow();
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(bg);

                if(HRVKeybinds.ScreenShotKey())
                {
                    // TODO Screenshot module
                }
                if(HRVKeybinds.FullScreenKey())
                {
                    Raylib.ToggleFullscreen();
                }
                if(HRVKeybinds.HelpKey()) {
                    hrv.HelpModule.HelpModule.ToggleHelp();
                }
                if(hrv.HelpModule.HelpModule.HelpActive) {
                    Raylib.DrawText("HELP SCREEN COCK AND BALLS", 10, 10, 20, Color.WHITE);
                } else {
                    if(IsKeyPressed(KeyboardKey.KEY_SPACE))
                    {
                        bg = lcg.GenColor();
                    }
                    if(IsKeyDown(KeyboardKey.KEY_KP_ADD) || IsKeyDown(KeyboardKey.KEY_EQUAL))
                    {
                        size++;
                    }
                    if(IsKeyDown(KeyboardKey.KEY_KP_SUBTRACT) || IsKeyDown(KeyboardKey.KEY_MINUS))
                    {
                        size--;
                        if(size < 1)
                        {
                            size = 1;
                        }
                    }
                    pos = pos + HRVKeybinds.InputVector();

                    Raylib.DrawText("COCK AND BALLS", (int)pos.X, (int)pos.Y, size, Color.WHITE);
                }
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
