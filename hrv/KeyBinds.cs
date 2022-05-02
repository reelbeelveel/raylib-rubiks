using Raylib_cs;
using System;
using static Raylib_cs.Raylib;
using static Raylib_cs.KeyboardKey;

namespace hrv.Keybinds {
    public static class HRVKeybinds {
        public static bool Control_W() {
            return IsKeyPressed(KEY_W) && (IsKeyDown(KEY_LEFT_CONTROL) || IsKeyDown(KEY_RIGHT_CONTROL));
        }
    }
}
