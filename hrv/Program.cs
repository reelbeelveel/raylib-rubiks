using Raylib_cs;
using hrv.ShellWrapper;
using hrv.Keybinds;
using hrv.Generator;
using System;

namespace HelloWorld
{
    static class Program
    {
        public static void Main()
        {
            Raylib.InitWindow(800, 480, "HRV");

            LCG lcg = new LCG();
            Color bg = lcg.GenColor();
            Raylib.SetExitKey(0);
            while (!Raylib.WindowShouldClose())
            {
                if(HRVKeybinds.Control_W())
                {
                    Raylib.CloseWindow();
                }
                Raylib.BeginDrawing();
                Raylib.ClearBackground(bg);

                Raylib.DrawText($"Hello, {string.Join("\n", new ShellProcess("whoami").getLines().ToArray())}", 12, 12, 20, Color.GREEN);
                Raylib.DrawText($"{lcg.Generate(10)}", 12, 32, 20, lcg.GenColor());

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
