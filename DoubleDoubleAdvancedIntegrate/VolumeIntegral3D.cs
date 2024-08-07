using DoubleDouble;
using DoubleDoubleIntegrate;
using System.Collections.ObjectModel;

namespace DoubleDoubleAdvancedIntegrate {
    public static partial class VolumeIntegral {
        public static (ddouble value, ddouble error) Integrate(
            Func<ddouble, ddouble, ddouble, ddouble> f,
            Volume3D volume,
            (ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range,
            GaussKronrodOrder order = GaussKronrodOrder.G31K63) {

            ReadOnlyCollection<(ddouble x, ddouble wg, ddouble wk)> ps = GaussKronrodPoints.Table[order];

            ddouble sg = ddouble.Zero, sk = ddouble.Zero;
            ddouble ru = u_range.max - u_range.min, rv = v_range.max - v_range.min, rw = w_range.max - w_range.min;

            if (!ddouble.IsFinite(ru)) {
                throw new ArgumentException("Invalid integation interval.", nameof(u_range));
            }
            if (!ddouble.IsFinite(rv)) {
                throw new ArgumentException("Invalid integation interval.", nameof(v_range));
            }
            if (!ddouble.IsFinite(rw)) {
                throw new ArgumentException("Invalid integation interval.", nameof(w_range));
            }

            for (int i = 0; i < ps.Count; i++) {
                ddouble u = ps[i].x * ru + u_range.min;

                for (int j = 0; j < ps.Count; j++) {
                    ddouble v = ps[j].x * rv + v_range.min;

                    for (int k = 0; k < ps.Count; k++) {
                        ddouble w = ps[k].x * rw + w_range.min;

                        (ddouble x, ddouble y, ddouble z) = volume.Value(u, v, w);
                        ((ddouble dxdu, ddouble dydu, ddouble dzdu), 
                         (ddouble dxdv, ddouble dydv, ddouble dzdv),
                         (ddouble dxdw, ddouble dydw, ddouble dzdw)) = volume.Diff(u, v, w);
                        ddouble value = f(x, y, z);

                        ddouble jacobian = ddouble.Abs(
                            dxdu * (dydv * dzdw - dydw * dzdv) -
                            dxdv * (dydu * dzdw - dydw * dzdu) +
                            dxdw * (dydu * dzdv - dydv * dzdu)
                        );

                        ddouble g = value * jacobian;

                        sk += ps[i].wk * ps[j].wk * ps[k].wk * g;

                        if ((i & 1) == 1 && (j & 1) == 1 && (k & 1) == 1) {
                            sg += ps[i].wg * ps[j].wg * ps[k].wg * g;
                        }
                    }
                }
            }

            ddouble uvw = ru * rv * rw;

            sk *= uvw;
            sg *= uvw;

            ddouble error = ddouble.Abs(sk - sg);

            return (sk, error);
        }
    }
}
