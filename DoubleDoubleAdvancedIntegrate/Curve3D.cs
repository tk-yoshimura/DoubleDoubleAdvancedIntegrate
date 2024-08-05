using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Curve3D {
        public Func<ddouble, (ddouble x, ddouble y, ddouble z)> Value { get; }
        public Func<ddouble, (ddouble dxdt, ddouble dydt, ddouble dzdt)> Diff { get; }

        public Curve3D(Func<ddouble, (ddouble x, ddouble y, ddouble z)> value, Func<ddouble, (ddouble dxdt, ddouble dydt, ddouble dzdt)> diff) {
            Value = value;
            Diff = diff;
        }

        public Curve3D(
            Func<ddouble, ddouble> x, Func<ddouble, ddouble> y, Func<ddouble, ddouble> z,
            Func<ddouble, ddouble> dxdt, Func<ddouble, ddouble> dydt, Func<ddouble, ddouble> dzdt) {

            Value = t => (x(t), y(t), z(t));
            Diff = t => (dxdt(t), dydt(t), dzdt(t));
        }

        public static Curve3D Line((ddouble x, ddouble y, ddouble z) v0, (ddouble x, ddouble y, ddouble z) v1) {
            ddouble dx = v1.x - v0.x, dy = v1.y - v0.y, dz = v1.z - v0.z;

            return new(
                t => (v0.x + t * dx, v0.y + t * dy, v0.z + t * dz),
                t => (dx, dy, dz)
            );
        }
    }
}
