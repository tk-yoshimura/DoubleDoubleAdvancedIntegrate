using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate.Utils {
    static class VectorUtil {
        public static ((ddouble x, ddouble y, ddouble z) a, (ddouble x, ddouble y, ddouble z) b) OrthoVector((ddouble x, ddouble y, ddouble z) v) {
            if (v.x == 0d && v.y == 0d) {
                return ((1, 0, 0), (0, 1, 0));
            }

            (ddouble x, ddouble y, ddouble z) a = (-v.y, v.x, 0d);
            (ddouble x, ddouble y, ddouble z) b = (
                v.y * a.z - v.z * a.y,
                v.z * a.x - v.x * a.z,
                v.x * a.y - v.y * a.x
            );

            ddouble ra = ddouble.Hypot(a.x, a.y), rb = ddouble.Hypot(b.x, b.y, b.z);

            a = (a.x / ra, a.y / ra, 0d);
            b = (b.x / rb, b.y / rb, b.z / rb);

            return (a, b);
        }
    }
}
