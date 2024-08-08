using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Line2D {
        public virtual Func<ddouble, (ddouble x, ddouble y)> Value { get; }
        public virtual Func<ddouble, (ddouble dxdt, ddouble dydt)> Diff { get; }

        public Line2D(Func<ddouble, (ddouble x, ddouble y)> value, Func<ddouble, (ddouble dxdt, ddouble dydt)> diff) {
            Value = value;
            Diff = diff;
        }

        public Line2D(Func<ddouble, ddouble> x, Func<ddouble, ddouble> y, Func<ddouble, ddouble> dxdt, Func<ddouble, ddouble> dydt) {
            Value = t => (x(t), y(t));
            Diff = t => (dxdt(t), dydt(t));
        }

        public static Line2D Line((ddouble x, ddouble y) v0, (ddouble x, ddouble y) v1) {
            ddouble dx = v1.x - v0.x, dy = v1.y - v0.y;

            return new(
                t => (v0.x + t * dx, v0.y + t * dy),
                t => (dx, dy)
            );
        }

        public static Line2D Circle => new(
            t => (ddouble.Cos(t), ddouble.Sin(t)),
            t => (-ddouble.Sin(t), ddouble.Cos(t))
        );

        public static Line2D operator +(Line2D line, (ddouble x, ddouble y) translate) {
            return new(
                t => {
                    (ddouble x, ddouble y) = line.Value(t);

                    return (x + translate.x, y + translate.y);
                },
                line.Diff
            );
        }

        public static Line2D operator *(Line2D line, ddouble scale) {
            return new(
                t => {
                    (ddouble x, ddouble y) = line.Value(t);

                    return (x * scale, y * scale);
                },
                t => {
                    (ddouble dxdt, ddouble dydt) = line.Diff(t);

                    return (dxdt * scale, dydt * scale);
                }
            );
        }

        public static Line2D operator *(Line2D line, (ddouble x, ddouble y) scale) {
            return new(
                t => {
                    (ddouble x, ddouble y) = line.Value(t);

                    return (x * scale.x, y * scale.y);
                },
                t => {
                    (ddouble dxdt, ddouble dydt) = line.Diff(t);

                    return (dxdt * scale.x, dydt * scale.y);
                }
            );
        }

        public static Line2D Rotate(Line2D line, ddouble theta) {
            ddouble c = ddouble.Cos(theta), s = ddouble.Sin(theta);

            return new(
                t => {
                    (ddouble x, ddouble y) = line.Value(t);

                    return (x * c - y * s, x * s + y * c);
                },
                t => {
                    (ddouble dxdt, ddouble dydt) = line.Diff(t);

                    return (dxdt * c - dydt * s, dxdt * s + dydt * c);
                }
            );
        }

        public static Line2D operator *(Line2D line, ddouble[,] matrix) {
            if (matrix.GetLength(0) != 2 || matrix.GetLength(1) != 3) {
                throw new ArgumentException("Invalid matrix size. expected: 2x3", nameof(matrix));
            }

            ddouble m00 = matrix[0, 0], m01 = matrix[0, 1], m02 = matrix[0, 2];
            ddouble m10 = matrix[1, 0], m11 = matrix[1, 1], m12 = matrix[1, 2];

            return new(
                t => {
                    (ddouble x, ddouble y) = line.Value(t);

                    return (x * m00 + y * m01 + m02, x * m10 + y * m11 + m12);
                },
                t => {
                    (ddouble dxdt, ddouble dydt) = line.Diff(t);

                    return (dxdt * m00 + dydt * m01, dxdt * m10 + dydt * m11);
                }
            );
        }
    }
}
