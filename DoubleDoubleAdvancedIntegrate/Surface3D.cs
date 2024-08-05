using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Surface3D {
        public Func<ddouble, ddouble, (ddouble x, ddouble y, ddouble z)> Value { get; }
        public Func<ddouble, ddouble, ((ddouble dx, ddouble dy, ddouble dz) du, (ddouble dx, ddouble dy, ddouble dz) dv)> Diff { get; }

        public Surface3D(
            Func<ddouble, ddouble, (ddouble x, ddouble y, ddouble z)> value,
            Func<ddouble, ddouble, ((ddouble dx, ddouble dy, ddouble dz) du, (ddouble dx, ddouble dy, ddouble dz) dv)> diff) {

            Value = value;
            Diff = diff;
        }

        public Surface3D(
            Func<ddouble, ddouble, ddouble> x, Func<ddouble, ddouble, ddouble> y, Func<ddouble, ddouble, ddouble> z,
            Func<ddouble, ddouble, ddouble> dxdu, Func<ddouble, ddouble, ddouble> dydu, Func<ddouble, ddouble, ddouble> dzdu,
            Func<ddouble, ddouble, ddouble> dxdv, Func<ddouble, ddouble, ddouble> dydv, Func<ddouble, ddouble, ddouble> dzdv) {

            Value = (u, v) => (x(u, v), y(u, v), z(u, v));
            Diff = (u, v) => ((dxdu(u, v), dydu(u, v), dzdu(u, v)), (dxdv(u, v), dydv(u, v), dzdv(u, v)));
        }
    }
}
