using Raylib_cs;
using static Raylib_cs.Raylib;
using System;
using System.Numerics;
using rbx.Generator;

namespace rbx {
    public static class RbxWindow {
        public static void Init() {
            Raylib.InitWindow(width, height, title);
        }
        public static String Title { set { title = value; Raylib.SetWindowTitle(value); } get { return title; } }
        public static int Height { set { height = value; Raylib.SetWindowSize(width, height); } get { return height; } }
        public static int Width  { set { width  = value; Raylib.SetWindowSize(width, height); } get { return width; } }

        public static Vector2 Center() => new Vector2(Width/2, Height/2);
        public static LCG rng { set; get; }

        private static String title = "Raylib Rubix";
        private static int height = 1080;
        private static int width = 1920;
    }
}


