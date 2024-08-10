using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Line4D {
        public virtual Func<ddouble, (ddouble x, ddouble y, ddouble z, ddouble w)> Value { get; }
        public virtual Func<ddouble, (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt)> Diff { get; }
        public Func<ddouble, ddouble> Ds { get; }

        public Line4D(
            Func<ddouble, (ddouble x, ddouble y, ddouble z, ddouble w)> value,
            Func<ddouble, (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt)> diff, 
            Func<ddouble, ddouble>? ds = null) {

            Value = value;
            Diff = diff;

            if (ds is not null) {
                Ds = ds;
            }
            else {
                Ds = t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt) = Diff(t);

                    return ddouble.Hypot(ddouble.Hypot(dxdt, dydt), ddouble.Hypot(dzdt, dwdt));
                };
            }
        }

        public Line4D(
            Func<ddouble, ddouble> x, Func<ddouble, ddouble> y, Func<ddouble, ddouble> z, Func<ddouble, ddouble> w,
            Func<ddouble, ddouble> dxdt, Func<ddouble, ddouble> dydt, Func<ddouble, ddouble> dzdt, Func<ddouble, ddouble> dwdt, 
            Func<ddouble, ddouble>? ds = null) 

            : this(t => (x(t), y(t), z(t), w(t)), t => (dxdt(t), dydt(t), dzdt(t), dwdt(t)), ds) { }

        public static Line4D Line((ddouble x, ddouble y, ddouble z, ddouble w) v0, (ddouble x, ddouble y, ddouble z, ddouble w) v1) {
            ddouble dx = v1.x - v0.x, dy = v1.y - v0.y, dz = v1.z - v0.z, dw = v1.w - v0.w;
            ddouble ds = ddouble.Hypot(ddouble.Hypot(dx, dy), ddouble.Hypot(dz, dw));

            return new(
                t => (v0.x + t * dx, v0.y + t * dy, v0.z + t * dz, v0.w + t * dw),
                t => (dx, dy, dz, dw),
                t => ds
            );
        }

        public static Line4D operator +(Line4D line, (ddouble x, ddouble y, ddouble z, ddouble w) translate) {
            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z, ddouble w) = line.Value(t);

                    return (x + translate.x, y + translate.y, z + translate.z, w + translate.w);
                },
                line.Diff,
                line.Ds
            );
        }

        public static Line4D operator *(Line4D line, ddouble scale) {
            ddouble abs_scale = ddouble.Abs(scale);

            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z, ddouble w) = line.Value(t);

                    return (x * scale, y * scale, z * scale, w * scale);
                },
                t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt) = line.Diff(t);

                    return (dxdt * scale, dydt * scale, dzdt * scale, dwdt * scale);
                },
                t => line.Ds(t) * abs_scale
            );
        }

        public static Line4D operator *(Line4D line, (ddouble x, ddouble y, ddouble z, ddouble w) scale) {
            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z, ddouble w) = line.Value(t);

                    return (x * scale.x, y * scale.y, z * scale.z, w * scale.w);
                },
                t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt) = line.Diff(t);

                    return (dxdt * scale.x, dydt * scale.y, dzdt * scale.z, dwdt * scale.w);
                }
            );
        }

        public static Line4D operator *(Line4D line, ddouble[,] matrix) {
            if (matrix.GetLength(0) != 4 || matrix.GetLength(1) != 5) {
                throw new ArgumentException("Invalid matrix size. expected: 4x5", nameof(matrix));
            }

            ddouble m00 = matrix[0, 0], m01 = matrix[0, 1], m02 = matrix[0, 2], m03 = matrix[0, 3], m04 = matrix[0, 4];
            ddouble m10 = matrix[1, 0], m11 = matrix[1, 1], m12 = matrix[1, 2], m13 = matrix[1, 3], m14 = matrix[1, 4];
            ddouble m20 = matrix[2, 0], m21 = matrix[2, 1], m22 = matrix[2, 2], m23 = matrix[2, 3], m24 = matrix[2, 4];
            ddouble m30 = matrix[3, 0], m31 = matrix[3, 1], m32 = matrix[3, 2], m33 = matrix[3, 3], m34 = matrix[3, 4];

            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z, ddouble w) = line.Value(t);

                    return (
                        x * m00 + y * m01 + z * m02 + w * m03 + m04,
                        x * m10 + y * m11 + z * m12 + w * m13 + m14,
                        x * m20 + y * m21 + z * m22 + w * m23 + m24,
                        x * m30 + y * m31 + z * m32 + w * m33 + m34
                    );
                },
                    t => {
                        (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt) = line.Diff(t);

                        return (
                            dxdt * m00 + dydt * m01 + dzdt * m02 + dwdt * m03,
                            dxdt * m10 + dydt * m11 + dzdt * m12 + dwdt * m13,
                            dxdt * m20 + dydt * m21 + dzdt * m22 + dwdt * m23,
                            dxdt * m30 + dydt * m31 + dzdt * m32 + dwdt * m33
                        );
                    }
            );
        }

        public static Line4D operator +(Line4D line) {
            return line;
        }

        public static Line4D operator -(Line4D line) {
            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z, ddouble w) = line.Value(t);

                    return (-x, -y, -z, -w);
                },
                t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt) = line.Diff(t);

                    return (-dxdt, -dydt, -dzdt, -dwdt);
                },
                line.Ds
            );
        }
    }
}
