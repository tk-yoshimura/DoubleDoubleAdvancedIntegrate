using DoubleDouble;
using DoubleDoubleAdvancedIntegrate.Utils;

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

        public static Curve3D Circle((ddouble x, ddouble y, ddouble z) normal) {
            ((ddouble x, ddouble y, ddouble z) a, (ddouble x, ddouble y, ddouble z) b) = VectorUtil.OrthoVector(normal);

            return new(
                t => {
                    ddouble c = ddouble.Cos(t), s = ddouble.Sin(t);

                    return (a.x * c + b.x * s, a.y * c + b.y * s, a.z * c + b.z * s);
                },
                t => {
                    ddouble c = ddouble.Cos(t), s = ddouble.Sin(t);

                    return (-a.x * s + b.x * c, -a.y * s + b.y * c, -a.z * s + b.z * c);
                }
            );
        }

        public static Curve3D Circle(ddouble radius, (ddouble x, ddouble y, ddouble z) normal) {
            ((ddouble x, ddouble y, ddouble z) a, (ddouble x, ddouble y, ddouble z) b) = VectorUtil.OrthoVector(normal);

            return new(
                t => {
                    ddouble c = radius * ddouble.Cos(t), s = radius * ddouble.Sin(t);

                    return (a.x * c + b.x * s, a.y * c + b.y * s, a.z * c + b.z * s);
                },
                t => {
                    ddouble c = radius * ddouble.Cos(t), s = radius * ddouble.Sin(t);

                    return (-a.x * s + b.x * c, -a.y * s + b.y * c, -a.z * s + b.z * c);
                }
            );
        }

        public static Curve3D Circle((ddouble x, ddouble y, ddouble z) center, (ddouble x, ddouble y, ddouble z) normal) {
            ((ddouble x, ddouble y, ddouble z) a, (ddouble x, ddouble y, ddouble z) b) = VectorUtil.OrthoVector(normal);

            return new(
                t => {
                    ddouble c = ddouble.Cos(t), s = ddouble.Sin(t);

                    return (center.x + a.x * c + b.x * s, center.y + a.y * c + b.y * s, center.z + a.z * c + b.z * s);
                },
                t => {
                    ddouble c = ddouble.Cos(t), s = ddouble.Sin(t);

                    return (-a.x * s + b.x * c, -a.y * s + b.y * c, -a.z * s + b.z * c);
                }
            );
        }

        public static Curve3D Circle((ddouble x, ddouble y, ddouble z) center, ddouble radius, (ddouble x, ddouble y, ddouble z) normal) {
            ((ddouble x, ddouble y, ddouble z) a, (ddouble x, ddouble y, ddouble z) b) = VectorUtil.OrthoVector(normal);

            return new(
                t => {
                    ddouble c = radius * ddouble.Cos(t), s = radius * ddouble.Sin(t);

                    return (center.x + a.x * c + b.x * s, center.y + a.y * c + b.y * s, center.z + a.z * c + b.z * s);
                },
                t => {
                    ddouble c = radius * ddouble.Cos(t), s = radius * ddouble.Sin(t);

                    return (-a.x * s + b.x * c, -a.y * s + b.y * c, -a.z * s + b.z * c);
                }
            );
        }
    }
}
