using Raylib_cs;
using static Raylib_cs.Raylib;
using System;
using System.Numerics;
using System.Collections.Generic;
using rbx.Generator;
using rbx.Puzzle;

using static rbx.GfxConstants;
using static rbx.Keybinds;

namespace rbx {
    public static class RbxWindow {
        public static void Init() {
            Raylib.InitWindow(width, height, title);
            camera.position = new Vector3(-10.0f, 15.0f, 20.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 30.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;
        }
        public static String Title { set { title = value; Raylib.SetWindowTitle(value); } get { return title; } }
        public static int Height { set { height = value; Raylib.SetWindowSize(width, height); } get { return height; } }
        public static int Width  { set { width  = value; Raylib.SetWindowSize(width, height); } get { return width; } }
        public static float MiniMapUnitSize(float size) { return (((float)MiniMapHeight/4)/size); }
        public static Rectangle MiniMapRect() {
            return new Rectangle(
                ((float)width - (float)MiniMapWidth),
                0.0f,
                MiniMapWidth,
                MiniMapHeight
            );
        }

        public static Vector3 SmartNormalize(Vector3 v) {
            return ((v.Length() != 0.0f) ? Vector3.Normalize(v) : Vector3.Zero);
        }
        public static void UpdateCam() {
            Vector3 panVector = SmartNormalize(Keybinds.PanVector()) * GfxConstants.PAN_SPEED;
            //Console.WriteLine(camera.position);
            camera.position = camera.position + panVector;
            //Console.WriteLine(camera.position);
            float MWMove = Raylib.GetMouseWheelMove();
            if(Math.Abs(MWMove) > 0.1) {
                // TODO Log scrolling
                OrbitRadius -= MWMove;
                OrbitRadius = (OrbitRadius < ZOOM_MIN_LIMIT) ? ZOOM_MIN_LIMIT : OrbitRadius;
                OrbitRadius = (OrbitRadius > ZOOM_MAX_LIMIT) ? ZOOM_MAX_LIMIT : OrbitRadius;
            }
            SetMouseCursor(IsCursorOnScreen() ? MouseCursor.MOUSE_CURSOR_RESIZE_ALL : MouseCursor.MOUSE_CURSOR_DEFAULT);
            UpdateCamera(ref RbxWindow.camera);
            SetOrbitRadius(OrbitRadius);
            CamCorrect();
            Raylib.BeginMode3D(RbxWindow.camera);
        }

        private static void CamCorrect() {
            Vector3 camPos = camera.position;
            Vector3 camVec = SmartNormalize(camPos);

            float XCosSim = Vector3.Dot(camVec, Cube.X.Normal);
            float YCosSim = Vector3.Dot(camVec, Cube.Y.Normal);
            float YOppCosSim = Vector3.Dot(camVec, Cube.YOpp.Normal);
            float ZCosSim = Vector3.Dot(camVec, Cube.Z.Normal);
            float ZOppCosSim = Vector3.Dot(camVec, Cube.ZOpp.Normal);
            List<float> cosSims = new List<float>() { XCosSim, YCosSim, YOppCosSim, ZCosSim, ZOppCosSim };
            cosSims.Sort();
            cosSims.Reverse();
            if(Math.Abs(XCosSim - cosSims[0]) < 0.1) return;
            if(YCosSim == cosSims[0]) {
                Cube.Move(Mvmt.Zp);
                camera.position = new Vector3(camPos.X * -1, camPos.Y, camPos.Z);
                return;
            } else if(YOppCosSim == cosSims[0]) {
                Cube.Move(Mvmt.Z);
                camera.position = new Vector3(camPos.X * -1, camPos.Y, camPos.Z);
                return;
            } else if(ZCosSim == cosSims[0]) {
                Cube.Move(Mvmt.Y);
                camera.position = new Vector3(camPos.X, camPos.Y * -1, camPos.Z);
                return;
            } else if(ZOppCosSim == cosSims[0]) {
                Cube.Move(Mvmt.Yp);
                camera.position = new Vector3(camPos.X, camPos.Y * -1, camPos.Z);
                return;
            }
        }

        private static void SetOrbitRadius(float radius) {
            camera.position = Vector3.Normalize(camera.position) * radius;
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public static Vector2 Center() => new Vector2(Width/2, Height/2);
        public static Vector2 MiniMapCenter() => new Vector2(Width - (MiniMapWidth / 2), (MiniMapHeight / 2));
        public static LCG rng { set; get; }
        public static Camera3D camera = new Camera3D();

        private static String title = "Raylib Rubix";
        private static int MiniMapHeight = 216;
        private static int MiniMapWidth = (int)(4*216/3);
        private static int width = 1920;
        private static int height = 1080;
        private static float OrbitRadius = 5.0f;
        public static RubixCube Cube = new RubixCube();
    }
}


