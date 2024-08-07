using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Volume3D {
        public Func<ddouble, ddouble, ddouble, (ddouble x, ddouble y, ddouble z)> Value { get; }
        public Func<ddouble, ddouble, ddouble,
            ((ddouble dx, ddouble dy, ddouble dz) du, (ddouble dx, ddouble dy, ddouble dz) dv, (ddouble dx, ddouble dy, ddouble dz) dw)> Diff { get; }

        public Volume3D(
            Func<ddouble, ddouble, ddouble, (ddouble x, ddouble y, ddouble z)> value,
            Func<ddouble, ddouble, ddouble, ((ddouble dx, ddouble dy, ddouble dz) du, (ddouble dx, ddouble dy, ddouble dz) dv, (ddouble dx, ddouble dy, ddouble dz) dw)> diff) {

            Value = value;
            Diff = diff;
        }

        public Volume3D(
            Func<ddouble, ddouble, ddouble, ddouble> x, Func<ddouble, ddouble, ddouble, ddouble> y, Func<ddouble, ddouble, ddouble, ddouble> z,
            Func<ddouble, ddouble, ddouble, ddouble> dxdu, Func<ddouble, ddouble, ddouble, ddouble> dydu, Func<ddouble, ddouble, ddouble, ddouble> dzdu,
            Func<ddouble, ddouble, ddouble, ddouble> dxdv, Func<ddouble, ddouble, ddouble, ddouble> dydv, Func<ddouble, ddouble, ddouble, ddouble> dzdv,
            Func<ddouble, ddouble, ddouble, ddouble> dxdw, Func<ddouble, ddouble, ddouble, ddouble> dydw, Func<ddouble, ddouble, ddouble, ddouble> dzdw) {

            Value = (u, v, w) => (x(u, v, w), y(u, v, w), z(u, v, w));
            Diff = (u, v, w) => (
                (dxdu(u, v, w), dydu(u, v, w), dzdu(u, v, w)),
                (dxdv(u, v, w), dydv(u, v, w), dzdv(u, v, w)),
                (dxdw(u, v, w), dydw(u, v, w), dzdw(u, v, w))
            );
        }

        public static Volume3D Ortho => new(
            (u, v, w) => (u, v, w),
            (u, v, w) => ((1d, 0d, 0d), (0d, 1d, 0d), (0d, 0d, 1d))
        );

        public static Volume3D Tetrahedron(
            (ddouble x, ddouble y, ddouble z) v0,
            (ddouble x, ddouble y, ddouble z) v1,
            (ddouble x, ddouble y, ddouble z) v2,
            (ddouble x, ddouble y, ddouble z) v3) {

            ddouble dx01 = v1.x - v0.x, dy01 = v1.y - v0.y, dz01 = v1.z - v0.z;
            ddouble dx02 = v2.x - v0.x, dy02 = v2.y - v0.y, dz02 = v2.z - v0.z;
            ddouble dx03 = v3.x - v0.x, dy03 = v3.y - v0.y, dz03 = v3.z - v0.z;

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
                )
            );
        }

        public static Volume3D Sphere() => new(
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
            }
        );
    }
}
