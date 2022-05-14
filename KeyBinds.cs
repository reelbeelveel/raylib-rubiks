using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.KeyboardKey;
using rbx.Puzzle;

namespace rbx{
    public static class Keybinds {
        public static bool CloseKey() {
            return Control_Mod(KEY_W);
        }
        public static bool ScreenShotKey() {
            return Control_Mod(KEY_S);
        }
        public static bool HelpKey() {
            return Shift_Mod(KEY_SLASH);
        }
        public static bool FullScreenKey() {
            return IsKeyPressed(KEY_F11);
        }
        public static bool UndoKey() {
            return IsKeyDown(KEY_Z) && (IsKeyDown(KEY_LEFT_CONTROL) || IsKeyDown(KEY_RIGHT_CONTROL));
        }
        public static bool ShuffleKey() {
            return IsKeyPressed(KEY_SPACE);
        }
        public static bool Control_Mod(KeyboardKey key) {
            return IsKeyPressed(key) && (IsKeyDown(KEY_LEFT_CONTROL) || IsKeyDown(KEY_RIGHT_CONTROL));
        }
        public static bool Shift_Mod(KeyboardKey key) {
            return IsKeyPressed(key) && (IsKeyDown(KEY_LEFT_SHIFT) || IsKeyDown(KEY_RIGHT_SHIFT));
        }
        public static bool Alt_Mod(KeyboardKey key) {
            return IsKeyPressed(key) && (IsKeyDown(KEY_LEFT_ALT) || IsKeyDown(KEY_RIGHT_ALT));
        }
        public static Mvmt? InputMvmt() {
            KeyboardKey[] MvmtKeys = new KeyboardKey[]{
               KEY_X, KEY_Y, KEY_Z, KEY_F, KEY_R, KEY_U, KEY_D, KEY_B, KEY_L, KEY_M, KEY_S, KEY_E
            };
            for(int i = 0; i < 12; i++) {
                if(IsKeyPressed(MvmtKeys[i])) {
                    if(IsKeyDown(KEY_LEFT_SHIFT) || IsKeyDown(KEY_RIGHT_SHIFT))
                        return (Mvmt)(i * 2) + 1;
                    return (Mvmt)(i * 2);
                }
            }
            return null;
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
