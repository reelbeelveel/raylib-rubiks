using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.KeyboardKey;

namespace hrv.Keybinds {
    public static class HRVKeybinds {
        public static bool Control_W() {
            return IsKeyPressed(KEY_W) && (IsKeyDown(KEY_LEFT_CONTROL) || IsKeyDown(KEY_RIGHT_CONTROL));
        }
        public static Vector2 InputVector() {
            Vector2 vec = new Vector2(0, 0);
            if (IsKeyDown(KEY_W) || IsKeyDown(KEY_UP)) {
                vec = vec + new Vector2(0, -1);
            }
            if (IsKeyDown(KEY_S) || IsKeyDown(KEY_DOWN)) {
                vec = vec + new Vector2(0, 1);
            }
            if (IsKeyDown(KEY_D) || IsKeyDown(KEY_RIGHT)) {
                vec = vec + new Vector2(1, 0);
            }
            if (IsKeyDown(KEY_A) || IsKeyDown(KEY_LEFT)) {
                vec = vec + new Vector2(-1, 0);
            }
            if (vec.Length() == 0) {
                return vec;
            }
            return Vector2.Normalize(vec);
        }
    }
}
