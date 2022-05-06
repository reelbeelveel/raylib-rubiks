using System;
using System.Diagnostics;
using System.IO;
using static System.Convert;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace rbx.Colors {
    public struct HexColor {
        public HexColor(byte r, byte g, byte b) {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        
        public HexColor(Color c) {
            this.r = c.r;
            this.g = c.g;
            this.b = c.b;
        }

        public HexColor(string HexString) {
            string parsed = HexString.Replace("#", "").Replace("0x", "").ToLower();
            if(parsed.Length != 6) {
                throw new ArgumentException(String.Format("The hex string must have 6 digits: {0}", HexString));
            }
            string rString = parsed.Substring(0,2);
            string gString = parsed.Substring(2,2);
            string bString = parsed.Substring(4,2);

            this.r = (byte)ToInt32(rString, 16);
            this.g = (byte)ToInt32(gString, 16);
            this.b = (byte)ToInt32(bString, 16);
        }

        public static implicit operator Color(HexColor h) => new Color((byte)h.r, (byte)h.g, (byte)h.b, (byte)255);
        public static explicit operator HexColor(Color c) => new HexColor(c.r, c.g, c.b);

        public byte r;
        public byte g;
        public byte b;
    }


    public static class Miamineon {
           public static Color Purplest     = new HexColor("0xaf5fff");
           public static Color Purpler      = new HexColor("0xaf00ff");
           public static Color Purple       = new HexColor("0xaf00d7");
           public static Color HotPink      = new HexColor("0xd7005f");
           public static Color Concrete     = new HexColor("0xd7d7ff");
           public static Color Lime         = new HexColor("0x00ff00");
           public static Color Slime        = new HexColor("0x87ff00");
           public static Color ViceYellow   = new HexColor("0xd7ff00");
           public static Color Sunburn      = new HexColor("0xd75f00");
           public static Color Gulfstream   = new HexColor("0x5f5fff");

    }

    public static class SystemPalette {
        public static Color bg { get; set; }
        public static Color fg { get; set; } = new Color(0, 0, 0, 255);
        public static Color[] SideColor = {
            Miamineon.Concrete,
            Miamineon.HotPink,
            Miamineon.Sunburn,
            Miamineon.Slime,
            Miamineon.Gulfstream,
            Miamineon.Concrete
        };
    }
}
