using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Curve2D {
        public Func<ddouble, (ddouble x, ddouble y)> Value { get; }
        public Func<ddouble, (ddouble dxdt, ddouble dydt)> Diff { get; }

        public Curve2D(Func<ddouble, (ddouble x, ddouble y)> value, Func<ddouble, (ddouble dxdt, ddouble dydt)> diff) {
            Value = value;
            Diff = diff;
        }

        public Curve2D(Func<ddouble, ddouble> x, Func<ddouble, ddouble> y, Func<ddouble, ddouble> dxdt, Func<ddouble, ddouble> dydt) {
            Value = t => (x(t), y(t));
            Diff = t => (dxdt(t), dydt(t));
        }

        public static Curve2D Line((ddouble x, ddouble y) v0, (ddouble x, ddouble y) v1) {
            ddouble dx = v1.x - v0.x, dy = v1.y - v0.y;

            return new(
                t => (v0.x + t * dx, v0.y + t * dy),
                t => (dx, dy)
            );
        }

        public static Curve2D Circle() => new(
            t => (ddouble.Cos(t), ddouble.Sin(t)),
            t => (-ddouble.Sin(t), ddouble.Cos(t))
        );

        public static Curve2D Circle(ddouble radius) => new(
            t => (radius * ddouble.Cos(t), radius * ddouble.Sin(t)),
            t => (-radius * ddouble.Sin(t), radius * ddouble.Cos(t))
        );

        public static Curve2D Circle((ddouble x, ddouble y) center) => new(
            t => (center.x + ddouble.Cos(t), center.y + ddouble.Sin(t)),
            t => (-ddouble.Sin(t), ddouble.Cos(t))
        );

        public static Curve2D Circle((ddouble x, ddouble y) center, ddouble radius) => new(
            t => (center.x + radius * ddouble.Cos(t), center.y + radius * ddouble.Sin(t)),
            t => (-radius * ddouble.Sin(t), radius * ddouble.Cos(t))
        );
    }
}
