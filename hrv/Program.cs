using Raylib_cs;
using hrv.ShellWrapper;
using hrv.Generator;

namespace HelloWorld
{
    static class Program
    {
        public static void Main()
        {
            Raylib.InitWindow(800, 480, "Hello World");

            LCG lcg = new LCG();
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                Raylib.DrawText($"Hello, {string.Join("\n", new ShellProcess("whoami").getLines().ToArray())}", 12, 12, 20, Color.GREEN);
                Raylib.DrawText($"{lcg.Generate()}", 12, 32, 20, lcg.GenColor());

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
