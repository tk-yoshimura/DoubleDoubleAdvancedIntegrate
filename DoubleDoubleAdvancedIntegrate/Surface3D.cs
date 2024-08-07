using DoubleDouble;
using DoubleDoubleAdvancedIntegrate.Utils;

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

        public static Surface3D Triangle((ddouble x, ddouble y, ddouble z) v0, (ddouble x, ddouble y, ddouble z) v1, (ddouble x, ddouble y, ddouble z) v2) {
            ddouble dx01 = v1.x - v0.x, dy01 = v1.y - v0.y, dz01 = v1.z - v0.z;
            ddouble dx02 = v2.x - v0.x, dy02 = v2.y - v0.y, dz02 = v2.z - v0.z;

            return new(
                (u, v) => (
                    v0.x + u * dx01 + (1d - u) * v * dx02,
                    v0.y + u * dy01 + (1d - u) * v * dy02,
                    v0.z + u * dz01 + (1d - u) * v * dz02
                ),
                (u, v) => (
                    (dx01 - v * dx02, dy01 - v * dy02, dz01 - v * dz02),
                    ((1d - u) * dx02, (1d - u) * dy02, (1d - u) * dz02)
                )
            );
        }

        public static Surface3D Circle((ddouble x, ddouble y, ddouble z) normal) {
            ((ddouble x, ddouble y, ddouble z) a, (ddouble x, ddouble y, ddouble z) b) = VectorUtil.OrthoVector(normal);

            return new(
                (r, theta) => {
                    ddouble c = r * ddouble.Cos(theta), s = r * ddouble.Sin(theta);

                    return (a.x * c + b.x * s, a.y * c + b.y * s, a.z * c + b.z * s);
                },
                (r, theta) => {
                    ddouble c = ddouble.Cos(theta), s = ddouble.Sin(theta);

                    return (
                        (a.x * c + b.x * s, a.y * c + b.y * s, a.z * c + b.z * s),
                        (r * (-a.x * s + b.x * c), r * (-a.y * s + b.y * c), r * (-a.z * s + b.z * c))
                    );
                }
            );
        }

        public static Surface3D Circle((ddouble x, ddouble y, ddouble z) center, (ddouble x, ddouble y, ddouble z) normal) {
            ((ddouble x, ddouble y, ddouble z) a, (ddouble x, ddouble y, ddouble z) b) = VectorUtil.OrthoVector(normal);

            return new(
                (r, theta) => {
                    ddouble c = r * ddouble.Cos(theta), s = r * ddouble.Sin(theta);

                    return (center.x + a.x * c + b.x * s, center.y + a.y * c + b.y * s, center.z + a.z * c + b.z * s);
                },
                (r, theta) => {
                    ddouble c = ddouble.Cos(theta), s = ddouble.Sin(theta);

                    return (
                        (a.x * c + b.x * s, a.y * c + b.y * s, a.z * c + b.z * s),
                        (r * (-a.x * s + b.x * c), r * (-a.y * s + b.y * c), r * (-a.z * s + b.z * c))
                    );
                }
            );
        }
    }
}
