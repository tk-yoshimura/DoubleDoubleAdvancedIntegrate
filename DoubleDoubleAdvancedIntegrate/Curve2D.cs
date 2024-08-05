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
