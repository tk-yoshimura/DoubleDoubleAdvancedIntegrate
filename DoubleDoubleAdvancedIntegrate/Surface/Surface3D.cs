using DoubleDouble;

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

        public static Surface3D Circle => new(
            (r, theta) => (r * ddouble.Cos(theta), r * ddouble.Sin(theta), 0d),
            (r, theta) => (
                (ddouble.Cos(theta), ddouble.Sin(theta), 0d),
                (-r * ddouble.Sin(theta), r * ddouble.Cos(theta), 0d)
            )
        );

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

        public static Surface3D Rhombus((ddouble x, ddouble y, ddouble z) v0, (ddouble x, ddouble y, ddouble z) v1, (ddouble x, ddouble y, ddouble z) v2) {
            ddouble dx01 = v1.x - v0.x, dy01 = v1.y - v0.y, dz01 = v1.z - v0.z;
            ddouble dx02 = v2.x - v0.x, dy02 = v2.y - v0.y, dz02 = v2.z - v0.z;

            return new(
                (u, v) => (
                    v0.x + u * dx01 + v * dx02,
                    v0.y + u * dy01 + v * dy02,
                    v0.z + u * dz01 + v * dz02
                ),
                (u, v) => (
                    (dx01, dy01, dz01),
                    (dx02, dy02, dz02)
                )
            );
        }

        public static Surface3D Sphere => new(
            (theta, phi) => {
                ddouble cos_theta = ddouble.Cos(theta), sin_theta = ddouble.Sin(theta);
                ddouble cos_phi = ddouble.Cos(phi), sin_phi = ddouble.Sin(phi);

                return new(
                    sin_theta * cos_phi,
                    sin_theta * sin_phi,
                    cos_theta
                );
            },
            (theta, phi) => {
                ddouble cos_theta = ddouble.Cos(theta), sin_theta = ddouble.Sin(theta);
                ddouble cos_phi = ddouble.Cos(phi), sin_phi = ddouble.Sin(phi);

                return new(
                    (cos_theta * cos_phi, cos_theta * sin_phi, -sin_theta),
                    (-sin_theta * sin_phi, sin_theta * cos_phi, 0d)
                );
            }
        );

        public static Surface3D operator +(Surface3D surface, (ddouble x, ddouble y, ddouble z) translate) {
            return new(
                (u, v) => {
                    (ddouble x, ddouble y, ddouble z) = surface.Value(u, v);

                    return (x + translate.x, y + translate.y, z + translate.z);
                },
                surface.Diff
            );
        }

        public static Surface3D operator *(Surface3D surface, ddouble scale) {
            return new(
                (u, v) => {
                    (ddouble x, ddouble y, ddouble z) = surface.Value(u, v);

                    return (x * scale, y * scale, z * scale);
                },
                (u, v) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv)) = surface.Diff(u, v);

                    return (
                        (dxdu * scale, dydu * scale, dzdu * scale),
                        (dxdv * scale, dydv * scale, dzdv * scale)
                    );
                }
            );
        }

        public static Surface3D operator *(Surface3D surface, (ddouble x, ddouble y, ddouble z) scale) {
            return new(
                (u, v) => {
                    (ddouble x, ddouble y, ddouble z) = surface.Value(u, v);

                    return (x * scale.x, y * scale.y, z * scale.z);
                },
                (u, v) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv)) = surface.Diff(u, v);

                    return (
                        (dxdu * scale.x, dydu * scale.y, dzdu * scale.z),
                        (dxdv * scale.x, dydv * scale.y, dzdv * scale.z)
                    );
                }
            );
        }

        public static Surface3D Rotate(Surface3D surface, (ddouble x, ddouble y, ddouble z) axis, ddouble theta) {
            ddouble r = ddouble.Hypot(axis.x, axis.y, axis.z);
            (ddouble nx, ddouble ny, ddouble nz) = (axis.x / r, axis.y / r, axis.z / r);

            ddouble c = ddouble.Cos(theta), cm1 = 1d - c, s = ddouble.Sin(theta);

            ddouble m00 = nx * nx * cm1 + c, m01 = nx * ny * cm1 - nz * s, m02 = nx * nz * cm1 + ny * s;
            ddouble m10 = nx * ny * cm1 + nz * s, m11 = ny * ny * cm1 + c, m12 = ny * nz * cm1 - nx * s;
            ddouble m20 = nx * nz * cm1 - ny * s, m21 = nz * ny * cm1 + nx * s, m22 = nz * nz * cm1 + c;

            return new(
                (u, v) => {
                    (ddouble x, ddouble y, ddouble z) = surface.Value(u, v);

                    return (
                        x * m00 + y * m01 + z * m02,
                        x * m10 + y * m11 + z * m12,
                        x * m20 + y * m21 + z * m22
                    );
                },
                (u, v) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv)) = surface.Diff(u, v);

                    return (
                    (
                        dxdu * m00 + dydu * m01 + dzdu * m02,
                        dxdu * m10 + dydu * m11 + dzdu * m12,
                        dxdu * m20 + dydu * m21 + dzdu * m22
                    ),
                    (
                        dxdv * m00 + dydv * m01 + dzdv * m02,
                        dxdv * m10 + dydv * m11 + dzdv * m12,
                        dxdv * m20 + dydv * m21 + dzdv * m22
                    ));
                }
            );
        }

        public static Surface3D Rotate(Surface3D surface, (ddouble x, ddouble y, ddouble z) v0, (ddouble x, ddouble y, ddouble z) v1) {
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
                return surface;
            }

            ddouble theta = ddouble.Acos(v0.x * v1.x + v0.y * v1.y + v0.z * v1.z);

            return Rotate(surface, axis, theta);
        }

        public static Surface3D operator *(Surface3D surface, ddouble[,] matrix) {
            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 4) {
                throw new ArgumentException("Invalid matrix size. expected: 3x4", nameof(matrix));
            }

            ddouble m00 = matrix[0, 0], m01 = matrix[0, 1], m02 = matrix[0, 2], m03 = matrix[0, 3];
            ddouble m10 = matrix[1, 0], m11 = matrix[1, 1], m12 = matrix[1, 2], m13 = matrix[1, 3];
            ddouble m20 = matrix[2, 0], m21 = matrix[2, 1], m22 = matrix[2, 2], m23 = matrix[2, 3];

            return new(
                (u, v) => {
                    (ddouble x, ddouble y, ddouble z) = surface.Value(u, v);

                    return (
                        x * m00 + y * m01 + z * m02 + m03,
                        x * m10 + y * m11 + z * m12 + m13,
                        x * m20 + y * m21 + z * m22 + m23
                    );
                },
                (u, v) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv)) = surface.Diff(u, v);

                    return (
                    (
                        dxdu * m00 + dydu * m01 + dzdu * m02,
                        dxdu * m10 + dydu * m11 + dzdu * m12,
                        dxdu * m20 + dydu * m21 + dzdu * m22
                    ),
                    (
                        dxdv * m00 + dydv * m01 + dzdv * m02,
                        dxdv * m10 + dydv * m11 + dzdv * m12,
                        dxdv * m20 + dydv * m21 + dzdv * m22
                    ));
                }
            );
        }

        public static Surface3D operator +(Surface3D surface) {
            return surface;
        }

        public static Surface3D operator -(Surface3D surface) {
            return new(
                (u, v) => {
                    (ddouble x, ddouble y, ddouble z) = surface.Value(u, v);

                    return (-x, -y, -z);
                },
                (u, v) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv)) = surface.Diff(u, v);

                    return (
                        (-dxdu, -dydu, -dzdu),
                        (-dxdv, -dydv, -dzdv)
                    );
                }
            );
        }
    }
}
