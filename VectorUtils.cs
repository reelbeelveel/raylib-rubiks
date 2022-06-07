using System;
using System.Numerics;
using System.Collections.Generic;
using rbx.Puzzle;

namespace rbx {
    public static class VectorUtils {
        public static Vector3 SmartNormalize(Vector3 v) {
            return ((v.Length() != 0.0f) ? Vector3.Normalize(v) : Vector3.Zero);
        }
        public static Vector3 BoundaryVector(Puzzle.RubixCube c, Puzzle.RubixCube.Face f)
            => SmartNormalize((c.X.Normal + f.Normal));
    }
}
