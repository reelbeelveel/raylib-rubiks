using Raylib_cs;
using static Raylib_cs.Raylib;
using System;
using System.Numerics;
using System.Collections.Generic;
using rbx.Generator;
using rbx.Puzzle;
using rbx.Colors;

using static rbx.GfxConstants;
using static rbx.Keybinds;
using static rbx.VectorUtils;
using static rbx.Config;

namespace rbx {
    public static class RbxWindow {
        public static void Init() {
            Raylib.InitWindow(width, height, title);
            CenterCam();
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

        private static void CenterCam() {
                camera.position = new Vector3(-10.0f, 15.0f, 20.0f);
                camera.target = new Vector3(0.0f, 0.0f, 0.0f);
                camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            SetOrbitRadius();
        }

        public static void UpdateCam() {
            if(Keybinds.CenterKey()) {
                CenterCam();
            } else {
                Vector3 panVector = VectorUtils.SmartNormalize(Keybinds.PanVector()) * GfxConstants.PAN_SPEED;
                panVector.Y = panVector.Y * -1.0f;
                float MWMove = Raylib.GetMouseWheelMove();
                if(Math.Abs(MWMove) > 0.1) {
                    // TODO Log scrolling
                    OrbitRadius -= MWMove;
                    OrbitRadius = (OrbitRadius < ZOOM_MIN_LIMIT) ? ZOOM_MIN_LIMIT : OrbitRadius;
                    OrbitRadius = (OrbitRadius > ZOOM_MAX_LIMIT) ? ZOOM_MAX_LIMIT : OrbitRadius;
                }
                SetOrbitRadius();
                SetMouseCursor(IsCursorOnScreen() ? MouseCursor.MOUSE_CURSOR_RESIZE_ALL : MouseCursor.MOUSE_CURSOR_DEFAULT);
                CamCorrect(panVector);
            }
            SetOrbitRadius();
            UpdateCamera(ref RbxWindow.camera);
            Raylib.BeginMode3D(RbxWindow.camera);
        }

        private static void CamCorrect(Vector3 panVector) {

            // determine if camera is closer to new face
            // snap to boundary + (normal to boundary)
            // make move
            // add panVector - (camera.position - boundary vector)

            Vector3 camPos = camera.position + panVector;
            Vector3 camVec = VectorUtils.SmartNormalize(camPos);

            float XCosSim = Vector3.Dot(camVec, Cube.X.Normal);
            float YCosSim = Vector3.Dot(camVec, Cube.Y.Normal);
            float YOppCosSim = Vector3.Dot(camVec, Cube.YOpp.Normal);
            float ZCosSim = Vector3.Dot(camVec, Cube.Z.Normal);
            float ZOppCosSim = Vector3.Dot(camVec, Cube.ZOpp.Normal);
            List<float> cosSims = new List<float>() { XCosSim, YCosSim, YOppCosSim, ZCosSim, ZOppCosSim };
            cosSims.Sort();
            for(int i = cosSims.Count - 1; i >= 0; i--) {
                String face = "?   ";
                uint id = 7;
                if(cosSims[i] == XCosSim) {
                    id = Cube.X.Id;
                    face = "X   ";
                } else if (cosSims[i] == YCosSim) {
                    id = Cube.Y.Id;
                    face = "Y   ";
                } else if (cosSims[i] == YOppCosSim) {
                    id = Cube.YOpp.Id;
                    face = "YOpp";
                } else if (cosSims[i] == ZCosSim) {
                    id = Cube.Z.Id;
                    face = "Z   ";
                } else if (cosSims[i] == ZOppCosSim) {
                    id = Cube.ZOpp.Id;
                    face = "ZOpp";
                }

                Raylib.DrawText($"{face} {cosSims[i]}", width - 400, height - (((i + 1) * 70) + 30), 70, SystemPalette.SideColor[id]);
            }
            cosSims.Reverse();

            if(cosSims[0] != XCosSim && !Keybinds.PauseKey()) {
                Vector3 boundary, snap, newNormal;
                Console.WriteLine($"{Cube.X.Id}\n");
                Console.WriteLine($"{XCosSim} | {cosSims[0]} | {cosSims[1]}\n\n");
                if(cosSims[0] == YCosSim) {
                    Console.WriteLine("Attempt to snap to Y");
                    boundary = BoundaryVector(Cube, Cube.Y);
                    newNormal = Cube.Y.Normal;
                    snap = BoundaryVector(Cube, Cube.YOpp);
                    Cube.Move(Mvmt.Zp);
                } else if(cosSims[0] == YOppCosSim) {
                    Console.WriteLine("Attempt to snap to YOpp");
                    boundary = BoundaryVector(Cube, Cube.YOpp);
                    newNormal = Cube.YOpp.Normal;
                    snap = BoundaryVector(Cube, Cube.Y);
                    Cube.Move(Mvmt.Z);
                } else if(cosSims[0] == ZCosSim) {
                    Console.WriteLine("Attempt to snap to Z");
                    boundary = BoundaryVector(Cube, Cube.Z);
                    newNormal = Cube.Z.Normal;
                    snap = BoundaryVector(Cube, Cube.ZOpp);
                    Cube.Move(Mvmt.Y);
                } else if(cosSims[0] == ZOppCosSim) {
                    Console.WriteLine("Attempt to snap to ZOpp");
                    boundary = BoundaryVector(Cube, Cube.ZOpp);
                    newNormal = Cube.ZOpp.Normal;
                    snap = BoundaryVector(Cube, Cube.Z);
                    Cube.Move(Mvmt.Yp);
                } else {
                    camera.position = camPos;
                    return;
                }
                boundary = boundary * OrbitRadius;
                snap = snap * OrbitRadius;
                Vector3 snapDelta = boundary - camera.position;
                Vector3 remain = panVector - snapDelta;
                Vector3 o1 = (camPos - (Cube.X.Normal * Vector3.Dot(camPos, Cube.X.Normal)));
                Vector3 ortho = o1 - (Vector3.Dot(o1, newNormal) * newNormal);
                if(
                        Vector3.Dot(
                            VectorUtils.SmartNormalize(remain),
                            VectorUtils.SmartNormalize(boundary))
                        > Vector3.Dot(
                            VectorUtils.SmartNormalize(remain),
                            VectorUtils.SmartNormalize(snap))) {
                    camera.position = snap + ortho;
                } else {
                    camera.position = snap + remain;
                }
                if(Config.EasyMove) CenterCam();
                return;
            }
            camera.position = camPos;
        }

        private static void SetOrbitRadius() {
            camera.position = Vector3.Normalize(camera.position) * OrbitRadius;
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public static void DrawCoords() {
            Raylib.DrawText($"X {camera.position.X}", 10, height - 220, 70, Color.RED);
            Raylib.DrawText($"Y {camera.position.Y}", 10, height - 150, 70, Color.GREEN);
            Raylib.DrawText($"Z {camera.position.Z}", 10, height - 80, 70, Color.BLUE);
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


