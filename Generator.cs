using Raylib_cs;
using System;
using System.Collections;
using System.Collections.Specialized;

namespace rbx.Generator {
    public class GeneratorRangeException : Exception {
        public GeneratorRangeException(string message) : base(message) { }
    }
    public class HRVGenerator {
        public HRVGenerator(uint? seed = null) {
            this.seed = seed ?? (uint)DateTime.Now.Ticks;
            Console.WriteLine("Seed: " + this.seed);
            this.state = this.seed;
        }
        protected virtual uint InternalGenerate(uint range) {
            return (state = (uint)DateTime.Now.Ticks) % range;
        }
        public uint Generate(uint range = uint.MaxValue) {
            if (range == 0) {
                throw new GeneratorRangeException("Range cannot be 0");
            }
            return InternalGenerate(range);
        }
        public byte GenByte(uint range = 256) {
            if (range > 256) {
                throw new GeneratorRangeException("Range must be less than or equal to 256");
            }
            return (byte)this.Generate(range);
        }
        public Color GenColor(uint range = 256, uint opacity = 255) {
            if (range > 256) {
                throw new GeneratorRangeException("Range must be less than or equal to 256");
            }
            if (opacity > 255) {
                throw new GeneratorRangeException("Opacity must be less than 256");
            }
            return new Color(GenByte(range), GenByte(range), GenByte(range), (byte)opacity);
        }
        public rbx.Puzzle.Mvmt GenMvmt(bool noSlices = false) {
            return (rbx.Puzzle.Mvmt)this.Generate((uint)(noSlices ? 18 : 24));
        }
        public uint seed;
        protected uint state;
    }
}
