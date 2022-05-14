using Raylib_cs;
using static Raylib_cs.Raylib;

using rbx.Generator;
using rbx.Puzzle;

using static rbx.HelpModule;
using static rbx.Keybinds;
using static rbx.ScreenShot;
using static rbx.RbxWindow;
using rbx.Colors;

using System;
using System.Numerics;

namespace rbx
{
    static class Program
    {
        public static void Main()
        {
            RbxWindow.Init();
            RbxWindow.rng = new LCG((uint)new Random().Next());
            Colors.SystemPalette.bg = RbxWindow.rng.GenColor();

            Raylib.SetExitKey(0);
            SetTargetFPS(GetMonitorRefreshRate(GetCurrentMonitor()));

            RubixCube cube = new RubixCube();
            //cube.Shuffle();
            float size = 20;

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
                float MWMove = Raylib.GetMouseWheelMove();
                if(Math.Abs(MWMove) > 0.1) {
                    size += MWMove;
                }
                if(HelpModule.HelpActive) {
                } else {
                    Raylib.BeginDrawing();

                    Raylib.ClearBackground(Colors.SystemPalette.bg);

                    int? NumKey = Keybinds.NumKey();
                    if(NumKey != null) {
                        cube = new RubixCube((uint)NumKey);
                    }
                    if(Keybinds.ShuffleKey()) {
                        if(!cube.Solved())
                            cube = new RubixCube();
                        cube.Shuffle();
                    }
                    if(Keybinds.UndoKey()) {
                        cube.Undo();
                    } else cube.Move(Keybinds.InputMvmt());
                    cube.Draw(size);

                    Raylib.EndDrawing();
                }
            }
        }
    }
}
