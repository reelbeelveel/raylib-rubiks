using Raylib_cs;
using static Raylib_cs.Raylib;
using System;
using System.Numerics;
using rbx.Generator;

namespace rbx {
    public static class RbxWindow {
        public static void Init() {
            Raylib.InitWindow(width, height, title);
            camera.position = new Vector3(0.0f, 50.0f, -120.0f);
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

        public static Vector2 Center() => new Vector2(Width/2, Height/2);
        public static Vector2 MiniMapCenter() => new Vector2(Width - (MiniMapWidth / 2), (MiniMapHeight / 2));
        public static LCG rng { set; get; }
        public static Camera3D camera = new Camera3D();

        private static String title = "Raylib Rubix";
        private static int MiniMapHeight = 216;
        private static int MiniMapWidth = (int)(4*216/3);
        private static int width = 1920;
        private static int height = 1080;
    }
}


