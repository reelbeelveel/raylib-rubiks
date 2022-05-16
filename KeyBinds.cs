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
        public static uint? NumKey() {
            for(uint i = 0; i < 10; i++)
                if(Any_NumPressed(i)) return i;
            return null;
        }

        public static bool Alt_Mod(KeyboardKey key)
            => IsKeyPressed(key) && Any_Alt();
        public static bool Control_Mod(KeyboardKey key)
            => IsKeyPressed(key) && Any_Control();
        public static bool Shift_Mod(KeyboardKey key)
            => IsKeyPressed(key) && Any_Shift();

        public static bool Any_NumPressed(uint num) {
            switch(num) {
                case 0: return IsKeyPressed(KEY_ZERO)
                        || IsKeyPressed(KEY_KP_0);
                case 1: return IsKeyPressed(KEY_ONE)
                        || IsKeyPressed(KEY_KP_1);
                case 2: return IsKeyPressed(KEY_TWO)
                        || IsKeyPressed(KEY_KP_2);
                case 3: return IsKeyPressed(KEY_THREE)
                        || IsKeyPressed(KEY_KP_3);
                case 4: return IsKeyPressed(KEY_FOUR)
                        || IsKeyPressed(KEY_KP_4);
                case 5: return IsKeyPressed(KEY_FIVE)
                        || IsKeyPressed(KEY_KP_5);
                case 6: return IsKeyPressed(KEY_SIX)
                        || IsKeyPressed(KEY_KP_6);
                case 7: return IsKeyPressed(KEY_SEVEN)
                        || IsKeyPressed(KEY_KP_7);
                case 8: return IsKeyPressed(KEY_EIGHT)
                        || IsKeyPressed(KEY_KP_8);
                case 9: return IsKeyPressed(KEY_NINE)
                        || IsKeyPressed(KEY_KP_9);
                default: return false;
            }
        }
        public static bool Any_Alt()
            => IsKeyDown(KEY_LEFT_ALT)
            || IsKeyDown(KEY_RIGHT_ALT);
        public static bool Any_Control()
            => IsKeyDown(KEY_LEFT_CONTROL)
            || IsKeyDown(KEY_RIGHT_CONTROL);
        public static bool Any_Shift()
            => IsKeyDown(KEY_LEFT_SHIFT)
            || IsKeyDown(KEY_RIGHT_SHIFT);

        public static Mvmt? InputMvmt() {
            KeyboardKey[] MvmtKeys = new KeyboardKey[]{
               KEY_X, KEY_Y, KEY_Z, KEY_F, KEY_R, KEY_U, KEY_D, KEY_B, KEY_L, KEY_M, KEY_E, KEY_S
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
