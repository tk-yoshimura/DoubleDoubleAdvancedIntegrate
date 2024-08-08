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

        public static Surface2D Circle => new(
            (r, theta) => (r * ddouble.Cos(theta), r * ddouble.Sin(theta)),
            (r, theta) => (
                (ddouble.Cos(theta), ddouble.Sin(theta)),
                (-r * ddouble.Sin(theta), r * ddouble.Cos(theta))
            )
        );

        public static Surface2D Triangle((ddouble x, ddouble y) v0, (ddouble x, ddouble y) v1, (ddouble x, ddouble y) v2) {
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

        public static Surface2D operator +(Surface2D surface, (ddouble x, ddouble y) translate) {
            return new(
                (u, v) => {
                    (ddouble x, ddouble y) = surface.Value(u, v);

                    return (x + translate.x, y + translate.y);
                },
                surface.Diff
            );
        }

        public static Surface2D operator *(Surface2D surface, ddouble scale) {
            return new(
                (u, v) => {
                    (ddouble x, ddouble y) = surface.Value(u, v);

                    return (x * scale, y * scale);
                },
                (u, v) => {
                    ((ddouble dxdu, ddouble dydu), 
                     (ddouble dxdv, ddouble dydv)) = surface.Diff(u, v);

                    return (
                        (dxdu * scale, dydu * scale), 
                        (dxdv * scale, dydv * scale)
                    );
                }
            );
        }

        public static Surface2D operator *(Surface2D surface, (ddouble x, ddouble y) scale) {
            return new(
                (u, v) => {
                    (ddouble x, ddouble y) = surface.Value(u, v);

                    return (x * scale.x, y * scale.y);
                },
                (u, v) => {
                    ((ddouble dxdu, ddouble dydu), 
                     (ddouble dxdv, ddouble dydv)) = surface.Diff(u, v);

                    return (
                        (dxdu * scale.x, dydu * scale.y), 
                        (dxdv * scale.x, dydv * scale.y)
                    );
                }
            );
        }

        public static Surface2D Rotate(Surface2D surface, ddouble theta) {
            ddouble c = ddouble.Cos(theta), s = ddouble.Sin(theta);

            return new(
                (u, v) => {
                    (ddouble x, ddouble y) = surface.Value(u, v);

                    return (x * c - y * s, x * s + y * c);
                },
                (u, v) => {
                    ((ddouble dxdu, ddouble dydu), 
                     (ddouble dxdv, ddouble dydv)) = surface.Diff(u, v);

                    return (
                        (dxdu * c - dydu * s, dxdu * s + dydu * c),
                        (dxdv * c - dydv * s, dxdv * s + dydv * c)
                    );
                }
            );
        }

        public static Surface2D operator *(Surface2D surface, ddouble[,] matrix) {
            if (matrix.GetLength(0) != 2 || matrix.GetLength(1) != 3) {
                throw new ArgumentException("Invalid matrix size. expected: 2x3", nameof(matrix));
            }

            ddouble m00 = matrix[0, 0], m01 = matrix[0, 1], m02 = matrix[0, 2];
            ddouble m10 = matrix[1, 0], m11 = matrix[1, 1], m12 = matrix[1, 2];

            return new(
                (u, v) => {
                    (ddouble x, ddouble y) = surface.Value(u, v);

                    return (x * m00 + y * m01 + m02, x * m10 + y * m11 + m12);
                },
                (u, v) => {
                    ((ddouble dxdu, ddouble dydu), 
                     (ddouble dxdv, ddouble dydv)) = surface.Diff(u, v);

                    return (
                        (dxdu * m00 + dydu * m01, dxdu * m10 + dydu * m11),
                        (dxdv * m00 + dydv * m01, dxdv * m10 + dydv * m11)
                    );
                }
            );
        }
    }
}
