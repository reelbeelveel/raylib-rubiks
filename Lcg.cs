using Raylib_cs;
using hrv.Generator;
using System;
using System.Collections.Specialized;
namespace hrv.Generator {
    class LCG : HRVGenerator {
        public LCG(uint? seed = null) : base(seed) {}
        protected override uint InternalGenerate(uint range) {
            return (uint)((state = (uint)((ulong)state * multiplier + increment) % uint.MaxValue) % range);
        }
        private uint multiplier = 1103515245;
        private uint increment = 12345;
    }
}
