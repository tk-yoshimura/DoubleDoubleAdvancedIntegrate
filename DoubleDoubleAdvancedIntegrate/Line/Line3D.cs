using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Line3D {
        public virtual Func<ddouble, (ddouble x, ddouble y, ddouble z)> Value { get; }
        public virtual Func<ddouble, (ddouble dxdt, ddouble dydt, ddouble dzdt)> Diff { get; }
        public Func<ddouble, ddouble> Ds { get; }

        public Line3D(
            Func<ddouble, (ddouble x, ddouble y, ddouble z)> value,
            Func<ddouble, (ddouble dxdt, ddouble dydt, ddouble dzdt)> diff,
            Func<ddouble, ddouble>? ds = null) {

            Value = value;
            Diff = diff;

            if (ds is not null) {
                Ds = ds;
            }
            else {
                Ds = t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt) = Diff(t);

                    return ddouble.Hypot(dxdt, dydt, dzdt);
                };
            }
        }

        public static Line3D Line((ddouble x, ddouble y, ddouble z) v0, (ddouble x, ddouble y, ddouble z) v1) {
            ddouble dx = v1.x - v0.x, dy = v1.y - v0.y, dz = v1.z - v0.z;
            ddouble ds = ddouble.Hypot(dx, dy, dz);

            return new(
                t => (v0.x + t * dx, v0.y + t * dy, v0.z + t * dz),
                t => (dx, dy, dz),
                t => ds
            );
        }

        public static Line3D Circle = new(
            t => (ddouble.Cos(t), ddouble.Sin(t), 0d),
            t => (-ddouble.Sin(t), ddouble.Cos(t), 0d),
            t => 1d
        );

        public static Line3D Helix = new(
            t => (ddouble.Cos(t), ddouble.Sin(t), t),
            t => (-ddouble.Sin(t), ddouble.Cos(t), 1d),
            t => ddouble.Sqrt2
        );

        public static Line3D operator +(Line3D line, (ddouble x, ddouble y, ddouble z) translate) {
            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z) = line.Value(t);

                    return (x + translate.x, y + translate.y, z + translate.z);
                },
                line.Diff,
                line.Ds
            );
        }

        public static Line3D operator -(Line3D line, (ddouble x, ddouble y, ddouble z) translate) {
            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z) = line.Value(t);

                    return (x - translate.x, y - translate.y, z - translate.z);
                },
                line.Diff,
                line.Ds
            );
        }

        public static Line3D operator *(Line3D line, ddouble scale) {
            ddouble abs_scale = ddouble.Abs(scale);

            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z) = line.Value(t);

                    return (x * scale, y * scale, z * scale);
                },
                t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt) = line.Diff(t);

                    return (dxdt * scale, dydt * scale, dzdt * scale);
                },
                t => line.Ds(t) * abs_scale
            );
        }

        public static Line3D operator *(Line3D line, (ddouble x, ddouble y, ddouble z) scale) {
            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z) = line.Value(t);

                    return (x * scale.x, y * scale.y, z * scale.z);
                },
                t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt) = line.Diff(t);

                    return (dxdt * scale.x, dydt * scale.y, dzdt * scale.z);
                }
            );
        }

        public static Line3D Rotate(Line3D line, (ddouble x, ddouble y, ddouble z) axis, ddouble theta) {
            ddouble r = ddouble.Hypot(axis.x, axis.y, axis.z);
            (ddouble nx, ddouble ny, ddouble nz) = (axis.x / r, axis.y / r, axis.z / r);

            ddouble c = ddouble.Cos(theta), cm1 = 1d - c, s = ddouble.Sin(theta);

            ddouble m00 = nx * nx * cm1 + c, m01 = nx * ny * cm1 - nz * s, m02 = nx * nz * cm1 + ny * s;
            ddouble m10 = nx * ny * cm1 + nz * s, m11 = ny * ny * cm1 + c, m12 = ny * nz * cm1 - nx * s;
            ddouble m20 = nx * nz * cm1 - ny * s, m21 = nz * ny * cm1 + nx * s, m22 = nz * nz * cm1 + c;

            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z) = line.Value(t);

                    return (
                        x * m00 + y * m01 + z * m02,
                        x * m10 + y * m11 + z * m12,
                        x * m20 + y * m21 + z * m22
                    );
                },
                t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt) = line.Diff(t);

                    return (
                        dxdt * m00 + dydt * m01 + dzdt * m02,
                        dxdt * m10 + dydt * m11 + dzdt * m12,
                        dxdt * m20 + dydt * m21 + dzdt * m22
                    );
                },
                line.Ds
            );
        }

        public static Line3D Rotate(Line3D line, (ddouble x, ddouble y, ddouble z) v0, (ddouble x, ddouble y, ddouble z) v1) {
            ddouble rv0 = ddouble.Hypot(v0.x, v0.y, v0.z), rv1 = ddouble.Hypot(v1.x, v1.y, v1.z);
            v0 = (v0.x / rv0, v0.y / rv0, v0.z / rv0);
            v1 = (v1.x / rv1, v1.y / rv1, v1.z / rv1);

            (ddouble x, ddouble y, ddouble z) axis = (
                v0.y * v1.z - v0.z * v1.y,
                v0.z * v1.x - v0.x * v1.z,
                v0.x * v1.y - v0.y * v1.x
            );

            ddouble r = ddouble.Hypot(axis.x, axis.y, axis.z);

            if (!(r > 0d)) {
                return line;
            }

            ddouble theta = ddouble.Acos(v0.x * v1.x + v0.y * v1.y + v0.z * v1.z);

            return Rotate(line, axis, theta);
        }

        public static Line3D operator *(Line3D line, ddouble[,] matrix) {
            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 4) {
                throw new ArgumentException("Invalid matrix size. expected: 3x4", nameof(matrix));
            }

            ddouble m00 = matrix[0, 0], m01 = matrix[0, 1], m02 = matrix[0, 2], m03 = matrix[0, 3];
            ddouble m10 = matrix[1, 0], m11 = matrix[1, 1], m12 = matrix[1, 2], m13 = matrix[1, 3];
            ddouble m20 = matrix[2, 0], m21 = matrix[2, 1], m22 = matrix[2, 2], m23 = matrix[2, 3];

            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z) = line.Value(t);

                    return (
                        x * m00 + y * m01 + z * m02 + m03,
                        x * m10 + y * m11 + z * m12 + m13,
                        x * m20 + y * m21 + z * m22 + m23
                    );
                },
                t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt) = line.Diff(t);

                    return (
                        dxdt * m00 + dydt * m01 + dzdt * m02,
                        dxdt * m10 + dydt * m11 + dzdt * m12,
                        dxdt * m20 + dydt * m21 + dzdt * m22
                    );
                }
            );
        }

        public static Line3D operator +(Line3D line) {
            return line;
        }

        public static Line3D operator -(Line3D line) {
            return new(
                t => {
                    (ddouble x, ddouble y, ddouble z) = line.Value(t);

                    return (-x, -y, -z);
                },
                t => {
                    (ddouble dxdt, ddouble dydt, ddouble dzdt) = line.Diff(t);

                    return (-dxdt, -dydt, -dzdt);
                },
                line.Ds
            );
        }
    }
}
