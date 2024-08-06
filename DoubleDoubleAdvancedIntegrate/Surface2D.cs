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

        public static Surface2D Ortho => new(
            (u, v) => (u, v),
            (u, v) => ((1d, 0d), (0d, 1d))
        );

        public static Surface2D Circle() => new(
            (r, theta) => (r * ddouble.Cos(theta), r * ddouble.Sin(theta)),
            (r, theta) => (
                (ddouble.Cos(theta), ddouble.Sin(theta)),
                (-r * ddouble.Sin(theta), r * ddouble.Cos(theta))
            )
        );

        public static Surface2D Circle((ddouble x, ddouble y) center) => new(
            (r, theta) => (center.x + ddouble.Cos(theta) * r, center.y + ddouble.Sin(theta) * r),
            (r, theta) => (
                (ddouble.Cos(theta), ddouble.Sin(theta)),
                (-r * ddouble.Sin(theta), r * ddouble.Cos(theta))
            )
        );

        public static Surface2D Triangular((ddouble x, ddouble y) v0, (ddouble x, ddouble y) v1, (ddouble x, ddouble y) v2) {
            ddouble dx01 = v1.x - v0.x, dy01 = v1.y - v0.y; 
            ddouble dx02 = v2.x - v0.x, dy02 = v2.y - v0.y;

            return new(
                (u, v) => (
                    v0.x + u * dx01 + (1d - u) * v * dx02, 
                    v0.y + u * dy01 + (1d - u) * v * dy02
                ),
                (u, v) => (
                    (dx01 - v * dx02, dy01 - v * dy02),
                    ((1d - u) * dx02, (1d - u) * dy02)
                )
            );
        }
    }
}
