using Raylib_cs;
using static Raylib_cs.Raylib;
using hrv.ShellWrapper;
using hrv.Keybinds;
using hrv.Generator;
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
            Console.WriteLine(pos.X + " " + pos.Y);
            while (!Raylib.WindowShouldClose())
            {
                if(HRVKeybinds.Control_W())
                {
                    Raylib.CloseWindow();
                }
                if(IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    bg = lcg.GenColor();
                }
                if(IsKeyPressed(KeyboardKey.KEY_KP_ADD) || IsKeyPressed(KeyboardKey.KEY_EQUAL))
                {
                    size++;
                }
                if(IsKeyPressed(KeyboardKey.KEY_KP_SUBTRACT) || IsKeyPressed(KeyboardKey.KEY_MINUS))
                {
                    size--;
                    if(size < 1)
                    {
                        size = 1;
                    }
                }
                pos = pos + HRVKeybinds.InputVector();
                Raylib.BeginDrawing();
                Raylib.ClearBackground(bg);

                Raylib.DrawText($"Hello, {string.Join("\n", new ShellProcess("whoami").getLines().ToArray())}", (int)pos.X, (int)pos.Y, (size), Color.GREEN);
                Raylib.DrawText($"{lcg.Generate(10)}", (int)pos.X, (int)pos.Y+(size), size, lcg.GenColor());
                Raylib.DrawFPS((int)pos.X, (int)pos.Y+(2*size));

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
