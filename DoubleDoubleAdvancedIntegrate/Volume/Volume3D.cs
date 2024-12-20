﻿using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Volume3D {
        public virtual Func<ddouble, ddouble, ddouble, (ddouble x, ddouble y, ddouble z)> Value { get; }
        public virtual Func<ddouble, ddouble, ddouble,
            ((ddouble dx, ddouble dy, ddouble dz) du, (ddouble dx, ddouble dy, ddouble dz) dv, (ddouble dx, ddouble dy, ddouble dz) dw)> Diff { get; }
        public Func<ddouble, ddouble, ddouble, ddouble> Ds { get; }

        public Volume3D(
            Func<ddouble, ddouble, ddouble, (ddouble x, ddouble y, ddouble z)> value,
            Func<ddouble, ddouble, ddouble, ((ddouble dx, ddouble dy, ddouble dz) du, (ddouble dx, ddouble dy, ddouble dz) dv, (ddouble dx, ddouble dy, ddouble dz) dw)> diff,
            Func<ddouble, ddouble, ddouble, ddouble>? ds = null) {

            Value = value;
            Diff = diff;

            if (ds is not null) {
                Ds = ds;
            }
            else {
                Ds = (u, v, w) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv),
                     (ddouble dxdw, ddouble dydw, ddouble dzdw)) = Diff(u, v, w);

                    return ddouble.Abs(
                        dxdu * (dydv * dzdw - dydw * dzdv) -
                        dxdv * (dydu * dzdw - dydw * dzdu) +
                        dxdw * (dydu * dzdv - dydv * dzdu)
                    );
                };
            }
        }

        public static Volume3D Ortho => new(
            (u, v, w) => (u, v, w),
            (u, v, w) => ((1d, 0d, 0d), (0d, 1d, 0d), (0d, 0d, 1d)),
            (u, v, w) => 1d
        );

        public static Volume3D InfinityOrtho => new(
            (u, v, w) => (InfSCurve.Value(u), InfSCurve.Value(v), InfSCurve.Value(w)),
            (u, v, w) => ((InfSCurve.Diff(u), 0d, 0d), (0d, InfSCurve.Diff(v), 0d), (0d, 0d, InfSCurve.Diff(w))),
            (u, v, w) => InfSCurve.Diff(u) * InfSCurve.Diff(v) * InfSCurve.Diff(w)
        );

        public static Volume3D Tetrahedron(
            (ddouble x, ddouble y, ddouble z) v0,
            (ddouble x, ddouble y, ddouble z) v1,
            (ddouble x, ddouble y, ddouble z) v2,
            (ddouble x, ddouble y, ddouble z) v3) {

            ddouble dx01 = v1.x - v0.x, dy01 = v1.y - v0.y, dz01 = v1.z - v0.z;
            ddouble dx02 = v2.x - v0.x, dy02 = v2.y - v0.y, dz02 = v2.z - v0.z;
            ddouble dx03 = v3.x - v0.x, dy03 = v3.y - v0.y, dz03 = v3.z - v0.z;

            ddouble ds = ddouble.Abs(
                dx01 * (dy02 * dz03 - dy03 * dz02) -
                dx02 * (dy01 * dz03 - dy03 * dz01) +
                dx03 * (dy01 * dz02 - dy02 * dz01)
            );

            return new(
                (u, v, w) => (
                    v0.x + u * dx01 + (1d - u) * v * dx02 + (1d - u) * (1d - v) * w * dx03,
                    v0.y + u * dy01 + (1d - u) * v * dy02 + (1d - u) * (1d - v) * w * dy03,
                    v0.z + u * dz01 + (1d - u) * v * dz02 + (1d - u) * (1d - v) * w * dz03
                ),
                (u, v, w) => (
                    (dx01 - v * dx02 - (1u - v) * w * dx03, dy01 - v * dy02 - (1u - v) * w * dy03, dz01 - v * dz02 - (1u - v) * w * dz03),
                    ((1d - u) * (dx02 - w * dx03), (1d - u) * (dy02 - w * dy03), (1d - u) * (dz02 - w * dz03)),
                    ((1d - u) * (1d - v) * dx03, (1d - u) * (1d - v) * dy03, (1d - u) * (1d - v) * dz03)
                ),
                (u, v, w) => ds * ddouble.Abs(ddouble.Square(1d - u) * (1d - v))
            );
        }

        public static Volume3D Parallelepiped(
            (ddouble x, ddouble y, ddouble z) v0,
            (ddouble x, ddouble y, ddouble z) v1,
            (ddouble x, ddouble y, ddouble z) v2,
            (ddouble x, ddouble y, ddouble z) v3) {

            ddouble dx01 = v1.x - v0.x, dy01 = v1.y - v0.y, dz01 = v1.z - v0.z;
            ddouble dx02 = v2.x - v0.x, dy02 = v2.y - v0.y, dz02 = v2.z - v0.z;
            ddouble dx03 = v3.x - v0.x, dy03 = v3.y - v0.y, dz03 = v3.z - v0.z;

            ddouble ds = ddouble.Abs(
                dx01 * (dy02 * dz03 - dy03 * dz02) -
                dx02 * (dy01 * dz03 - dy03 * dz01) +
                dx03 * (dy01 * dz02 - dy02 * dz01)
            );

            return new(
                (u, v, w) => (
                    v0.x + u * dx01 + v * dx02 + w * dx03,
                    v0.y + u * dy01 + v * dy02 + w * dy03,
                    v0.z + u * dz01 + v * dz02 + w * dz03
                ),
                (u, v, w) => (
                    (dx01, dy01, dz01),
                    (dx02, dy02, dz02),
                    (dx03, dy03, dz03)
                ),
                (u, v, w) => ds
            );
        }

        public static Volume3D Sphere => new(
            (r, theta, phi) => {
                ddouble cos_theta = ddouble.Cos(theta), sin_theta = ddouble.Sin(theta);
                ddouble cos_phi = ddouble.Cos(phi), sin_phi = ddouble.Sin(phi);

                return new(
                    r * sin_theta * cos_phi,
                    r * sin_theta * sin_phi,
                    r * cos_theta
                );
            },
            (r, theta, phi) => {
                ddouble cos_theta = ddouble.Cos(theta), sin_theta = ddouble.Sin(theta);
                ddouble cos_phi = ddouble.Cos(phi), sin_phi = ddouble.Sin(phi);

                return new(
                    (sin_theta * cos_phi, sin_theta * sin_phi, cos_theta),
                    (r * cos_theta * cos_phi, r * cos_theta * sin_phi, -r * sin_theta),
                    (-r * sin_theta * sin_phi, r * sin_theta * cos_phi, 0d)
                );
            },
            (r, theta, phi) => ddouble.Abs(r * r * ddouble.Sin(theta))
        );

        public static Volume3D InfinitySphere => new(
            (r, theta, phi) => {
                ddouble cos_theta = ddouble.Cos(theta), sin_theta = ddouble.Sin(theta);
                ddouble cos_phi = ddouble.Cos(phi), sin_phi = ddouble.Sin(phi);
                ddouble v = InfSCurve.Value(r);

                return new(
                    v * sin_theta * cos_phi,
                    v * sin_theta * sin_phi,
                    v * cos_theta
                );
            },
            (r, theta, phi) => {
                ddouble cos_theta = ddouble.Cos(theta), sin_theta = ddouble.Sin(theta);
                ddouble cos_phi = ddouble.Cos(phi), sin_phi = ddouble.Sin(phi);
                ddouble v = InfSCurve.Value(r), d = InfSCurve.Diff(r);

                return new(
                    (d * sin_theta * cos_phi, d * sin_theta * sin_phi, d * cos_theta),
                    (v * cos_theta * cos_phi, v * cos_theta * sin_phi, -v * sin_theta),
                    (-v * sin_theta * sin_phi, v * sin_theta * cos_phi, 0d)
                );
            },
            (r, theta, phi) => ddouble.Abs(ddouble.Square(InfSCurve.Value(r)) * InfSCurve.Diff(r) * ddouble.Sin(theta))
        );

        public static Volume3D Cylinder => new(
            (r, theta, z) => {
                ddouble cos_theta = ddouble.Cos(theta), sin_theta = ddouble.Sin(theta);

                return new(
                    r * cos_theta,
                    r * sin_theta,
                    z
                );
            },
            (r, theta, z) => {
                ddouble cos_theta = ddouble.Cos(theta), sin_theta = ddouble.Sin(theta);

                return new(
                    (cos_theta, sin_theta, 0d),
                    (-r * sin_theta, r * cos_theta, 0d),
                    (0d, 0d, 1d)
                );
            },
            (r, theta, z) => ddouble.Abs(r)
        );

        public static Volume3D TrigonalPrism((ddouble x, ddouble y) v0, (ddouble x, ddouble y) v1, (ddouble x, ddouble y) v2) {
            ddouble dx01 = v1.x - v0.x, dy01 = v1.y - v0.y;
            ddouble dx02 = v2.x - v0.x, dy02 = v2.y - v0.y;

            ddouble ds = ddouble.Abs(dx01 * dy02 - dx02 * dy01);

            return new(
                (u, v, z) => (
                    v0.x + u * dx01 + (1d - u) * v * dx02,
                    v0.y + u * dy01 + (1d - u) * v * dy02,
                    z
                ),
                (u, v, z) => (
                    (dx01 - v * dx02, dy01 - v * dy02, 0d),
                    ((1d - u) * dx02, (1d - u) * dy02, 0d),
                    (0d, 0d, 1d)
                ),
                (u, v, z) => ds * ddouble.Abs(1d - u)
            );
        }

        public static Volume3D operator +(Volume3D volume, (ddouble x, ddouble y, ddouble z) translate) {
            return new(
                (u, v, w) => {
                    (ddouble x, ddouble y, ddouble z) = volume.Value(u, v, w);

                    return (x + translate.x, y + translate.y, z + translate.z);
                },
                volume.Diff,
                volume.Ds
            );
        }

        public static Volume3D operator -(Volume3D volume, (ddouble x, ddouble y, ddouble z) translate) {
            return new(
                (u, v, w) => {
                    (ddouble x, ddouble y, ddouble z) = volume.Value(u, v, w);

                    return (x - translate.x, y - translate.y, z - translate.z);
                },
                volume.Diff,
                volume.Ds
            );
        }

        public static Volume3D operator *(Volume3D volume, ddouble scale) {
            ddouble cb_scale = ddouble.Abs(scale * scale * scale);

            return new(
                (u, v, w) => {
                    (ddouble x, ddouble y, ddouble z) = volume.Value(u, v, w);

                    return (x * scale, y * scale, z * scale);
                },
                (u, v, w) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv),
                     (ddouble dxdw, ddouble dydw, ddouble dzdw)) = volume.Diff(u, v, w);

                    return (
                        (dxdu * scale, dydu * scale, dzdu * scale),
                        (dxdv * scale, dydv * scale, dzdv * scale),
                        (dxdw * scale, dydw * scale, dzdw * scale)
                    );
                },
                (u, v, w) => volume.Ds(u, v, w) * cb_scale
            );
        }

        public static Volume3D operator *(Volume3D volume, (ddouble x, ddouble y, ddouble z) scale) {
            return new(
                (u, v, w) => {
                    (ddouble x, ddouble y, ddouble z) = volume.Value(u, v, w);

                    return (x * scale.x, y * scale.y, z * scale.z);
                },
                (u, v, w) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv),
                     (ddouble dxdw, ddouble dydw, ddouble dzdw)) = volume.Diff(u, v, w);

                    return (
                        (dxdu * scale.x, dydu * scale.y, dzdu * scale.z),
                        (dxdv * scale.x, dydv * scale.y, dzdv * scale.z),
                        (dxdw * scale.x, dydw * scale.y, dzdw * scale.z)
                    );
                }
            );
        }

        public static Volume3D Rotate(Volume3D volume, (ddouble x, ddouble y, ddouble z) axis, ddouble theta) {
            ddouble r = ddouble.Hypot(axis.x, axis.y, axis.z);
            (ddouble nx, ddouble ny, ddouble nz) = (axis.x / r, axis.y / r, axis.z / r);

            ddouble c = ddouble.Cos(theta), cm1 = 1d - c, s = ddouble.Sin(theta);

            ddouble m00 = nx * nx * cm1 + c, m01 = nx * ny * cm1 - nz * s, m02 = nx * nz * cm1 + ny * s;
            ddouble m10 = nx * ny * cm1 + nz * s, m11 = ny * ny * cm1 + c, m12 = ny * nz * cm1 - nx * s;
            ddouble m20 = nx * nz * cm1 - ny * s, m21 = nz * ny * cm1 + nx * s, m22 = nz * nz * cm1 + c;

            return new(
                (u, v, w) => {
                    (ddouble x, ddouble y, ddouble z) = volume.Value(u, v, w);

                    return (
                        x * m00 + y * m01 + z * m02,
                        x * m10 + y * m11 + z * m12,
                        x * m20 + y * m21 + z * m22
                    );
                },
                (u, v, w) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv),
                     (ddouble dxdw, ddouble dydw, ddouble dzdw)) = volume.Diff(u, v, w);

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
                    ),
                    (
                        dxdw * m00 + dydw * m01 + dzdw * m02,
                        dxdw * m10 + dydw * m11 + dzdw * m12,
                        dxdw * m20 + dydw * m21 + dzdw * m22
                    ));
                },
                volume.Ds
            );
        }

        public static Volume3D Rotate(Volume3D volume, (ddouble x, ddouble y, ddouble z) v0, (ddouble x, ddouble y, ddouble z) v1) {
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
                return volume;
            }

            ddouble theta = ddouble.Acos(v0.x * v1.x + v0.y * v1.y + v0.z * v1.z);

            return Rotate(volume, axis, theta);
        }

        public static Volume3D operator *(Volume3D volume, ddouble[,] matrix) {
            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 4) {
                throw new ArgumentException("Invalid matrix size. expected: 3x4", nameof(matrix));
            }

            ddouble m00 = matrix[0, 0], m01 = matrix[0, 1], m02 = matrix[0, 2], m03 = matrix[0, 3];
            ddouble m10 = matrix[1, 0], m11 = matrix[1, 1], m12 = matrix[1, 2], m13 = matrix[1, 3];
            ddouble m20 = matrix[2, 0], m21 = matrix[2, 1], m22 = matrix[2, 2], m23 = matrix[2, 3];

            return new(
                (u, v, w) => {
                    (ddouble x, ddouble y, ddouble z) = volume.Value(u, v, w);

                    return (
                        x * m00 + y * m01 + z * m02 + m03,
                        x * m10 + y * m11 + z * m12 + m13,
                        x * m20 + y * m21 + z * m22 + m23
                    );
                },
                (u, v, w) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv),
                     (ddouble dxdw, ddouble dydw, ddouble dzdw)) = volume.Diff(u, v, w);

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
                    ),
                    (
                        dxdw * m00 + dydw * m01 + dzdw * m02,
                        dxdw * m10 + dydw * m11 + dzdw * m12,
                        dxdw * m20 + dydw * m21 + dzdw * m22
                    ));
                }
            );
        }

        public static Volume3D operator +(Volume3D volume) {
            return volume;
        }

        public static Volume3D operator -(Volume3D volume) {
            return new(
                (u, v, w) => {
                    (ddouble x, ddouble y, ddouble z) = volume.Value(u, v, w);

                    return (-x, -y, -z);
                },
                (u, v, w) => {
                    ((ddouble dxdu, ddouble dydu, ddouble dzdu),
                     (ddouble dxdv, ddouble dydv, ddouble dzdv),
                     (ddouble dxdw, ddouble dydw, ddouble dzdw)) = volume.Diff(u, v, w);

                    return (
                        (-dxdu, -dydu, -dzdu),
                        (-dxdv, -dydv, -dzdv),
                        (-dxdw, -dydw, -dzdw)
                    );
                },
                volume.Ds
            );
        }
    }
}
