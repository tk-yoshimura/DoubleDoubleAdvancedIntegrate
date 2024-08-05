using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Surface2D {
        public Func<ddouble, ddouble, (ddouble x, ddouble y)> Value { get; }
        public Func<ddouble, ddouble, ((ddouble dx, ddouble dy) du, (ddouble dx, ddouble dy) dv)> Diff { get; }

        public Surface2D(
            Func<ddouble, ddouble, (ddouble x, ddouble y)> value,
            Func<ddouble, ddouble, ((ddouble dx, ddouble dy) du, (ddouble dx, ddouble dy) dv)> diff) {

            Value = value;
            Diff = diff;
        }

        public Surface2D(
            Func<ddouble, ddouble, ddouble> x, Func<ddouble, ddouble, ddouble> y,
            Func<ddouble, ddouble, ddouble> dxdu, Func<ddouble, ddouble, ddouble> dydu,
            Func<ddouble, ddouble, ddouble> dxdv, Func<ddouble, ddouble, ddouble> dydv) {

            Value = (u, v) => (x(u, v), y(u, v));
            Diff = (u, v) => ((dxdu(u, v), dydu(u, v)), (dxdv(u, v), dydv(u, v)));
        }
    }
}
