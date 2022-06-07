using Raylib_cs;
using static Raylib_cs.Raylib;

using rbx.Generator;
using rbx.Puzzle;

using static rbx.HelpModule;
using static rbx.Keybinds;
using static rbx.ScreenShot;
using static rbx.RbxWindow;
using static rbx.GfxConstants;
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

            RbxWindow.Cube = new RubixCube();
            float OrbitRadius = 5.0f;
            //cube.Shuffle();

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

                    uint? NumKey = Keybinds.NumKey();
                    if(NumKey != null) {
                        RbxWindow.Cube = new RubixCube((uint)((NumKey == 0) ? 10 : NumKey));
                    }
                    if(Keybinds.ShuffleKey()) {
                        if(!RbxWindow.Cube.Solved())
                            RbxWindow.Cube = new RubixCube();
                        RbxWindow.Cube.Shuffle();
                    }
                    if(Keybinds.UndoKey()) {
                        RbxWindow.Cube.Undo();
                    } else RbxWindow.Cube.Move(Keybinds.InputMvmt());

                    RbxWindow.UpdateCam();

                    RbxWindow.Cube.Draw3D();

                    Raylib.EndMode3D();

                    RbxWindow.Cube.DrawMiniMap();

                    RbxWindow.DrawCoords();

                    Raylib.EndDrawing();
                }
            }
        }
    }
}
