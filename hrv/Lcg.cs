using Raylib_cs;
using hrv.Generator;
using System;
using System.Collections.Specialized;
namespace hrv.Generator {
    class LCG : HRVGenerator {

        public LCG(uint? seed = null) {
            this.seed = seed ?? (uint)DateTime.Now.Ticks;
        }

        public uint Generate(uint range = uint.MaxValue) {
            return ((state = (state * multiplier + increment)) % range);
        }
        public byte GenByte(uint range = 255) {
            return (byte)((state = (state * multiplier + increment)) % range);
        }
        public Color GenColor(uint range = 255) {
            return new Color(GenByte(range), GenByte(range), GenByte(range), (byte)255);
        }

        public uint seed;
        private uint state;
        private uint multiplier = 1103515245;
        private uint increment = 12345;
    }
}
