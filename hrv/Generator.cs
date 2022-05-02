using System.Collections;

namespace hrv.Generator {
    interface HRVGenerator {
        public uint Generate(uint range = 0);
        public byte GenByte(uint range = 255);
    }
}
