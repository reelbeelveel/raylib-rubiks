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

            RubixCube cube = new RubixCube();
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
                float MWMove = Raylib.GetMouseWheelMove();
                if(Math.Abs(MWMove) > 0.1) {
                    // TODO Log scrolling
                    OrbitRadius -= MWMove;
                    OrbitRadius = (OrbitRadius < ZOOM_MIN_LIMIT) ? ZOOM_MIN_LIMIT : OrbitRadius;
                    OrbitRadius = (OrbitRadius > ZOOM_MAX_LIMIT) ? ZOOM_MAX_LIMIT : OrbitRadius;
                }
                if(HelpModule.HelpActive) {
                } else {
                    Raylib.BeginDrawing();

                    Raylib.ClearBackground(Colors.SystemPalette.bg);

                    uint? NumKey = Keybinds.NumKey();
                    if(NumKey != null) {
                        cube = new RubixCube((uint)((NumKey == 0) ? 10 : NumKey));
                    }
                    if(Keybinds.ShuffleKey()) {
                        if(!cube.Solved())
                            cube = new RubixCube();
                        cube.Shuffle();
                    }
                    if(Keybinds.UndoKey()) {
                        cube.Undo();
                    } else cube.Move(Keybinds.InputMvmt());
                    RbxWindow.SetOrbitRadius(OrbitRadius);
                    UpdateCamera(ref RbxWindow.camera);

                    Raylib.BeginMode3D(RbxWindow.camera);

                    cube.Draw3D();

                    Raylib.EndMode3D();

                    cube.DrawMiniMap();

                    Raylib.EndDrawing();
                }
            }
        }
    }
}
