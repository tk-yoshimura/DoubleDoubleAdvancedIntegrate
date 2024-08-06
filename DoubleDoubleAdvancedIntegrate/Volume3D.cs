using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Volume3D {
        public Func<ddouble, ddouble, ddouble, (ddouble x, ddouble y, ddouble z)> Value { get; }
        public Func<ddouble, ddouble, ddouble,
            ((ddouble dx, ddouble dy, ddouble dz) du, (ddouble dx, ddouble dy, ddouble dz) dv, (ddouble dx, ddouble dy, ddouble dz) dw)> Diff { get; }

        public Volume3D(
            Func<ddouble, ddouble, ddouble, (ddouble x, ddouble y, ddouble z)> value,
            Func<ddouble, ddouble, ddouble, ((ddouble dx, ddouble dy, ddouble dz) du, (ddouble dx, ddouble dy, ddouble dz) dv, (ddouble dx, ddouble dy, ddouble dz) dw)> diff) {

            Value = value;
            Diff = diff;
        }

        public Volume3D(
            Func<ddouble, ddouble, ddouble, ddouble> x, Func<ddouble, ddouble, ddouble, ddouble> y, Func<ddouble, ddouble, ddouble, ddouble> z,
            Func<ddouble, ddouble, ddouble, ddouble> dxdu, Func<ddouble, ddouble, ddouble, ddouble> dydu, Func<ddouble, ddouble, ddouble, ddouble> dzdu,
            Func<ddouble, ddouble, ddouble, ddouble> dxdv, Func<ddouble, ddouble, ddouble, ddouble> dydv, Func<ddouble, ddouble, ddouble, ddouble> dzdv,
            Func<ddouble, ddouble, ddouble, ddouble> dxdw, Func<ddouble, ddouble, ddouble, ddouble> dydw, Func<ddouble, ddouble, ddouble, ddouble> dzdw) {

            Value = (u, v, w) => (x(u, v, w), y(u, v, w), z(u, v, w));
            Diff = (u, v, w) => (
                (dxdu(u, v, w), dydu(u, v, w), dzdu(u, v, w)),
                (dxdv(u, v, w), dydv(u, v, w), dzdv(u, v, w)),
                (dxdw(u, v, w), dydw(u, v, w), dzdw(u, v, w))
            );
        }
    }
}
