using DoubleDouble;
using DoubleDoubleIntegrate;
using System.Collections.ObjectModel;

namespace DoubleDoubleAdvancedIntegrate {
    public static partial class SurfaceIntegral {
        public static (ddouble value, ddouble error) Integrate(
            Func<ddouble, ddouble, ddouble> f,
            Surface2D surface,
            (ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range,
            GaussKronrodOrder order = GaussKronrodOrder.G31K63) {

            ReadOnlyCollection<(ddouble x, ddouble wg, ddouble wk)> ps = GaussKronrodPoints.Table[order];

            ddouble sg = ddouble.Zero, sk = ddouble.Zero;
            ddouble ru = u_range.max - u_range.min, rv = v_range.max - v_range.min;

            if (!ddouble.IsFinite(ru)) {
                throw new ArgumentException("Invalid integation interval.", nameof(u_range));
            }
            if (!ddouble.IsFinite(rv)) {
                throw new ArgumentException("Invalid integation interval.", nameof(v_range));
            }

            for (int i = 0; i < ps.Count; i++) {
                ddouble u = ps[i].x * ru + u_range.min;

                for (int j = 0; j < ps.Count; j++) {
                    ddouble v = ps[j].x * rv + v_range.min;

                    (ddouble x, ddouble y) = surface.Value(u, v);
                    ((ddouble dxdu, ddouble dydu), (ddouble dxdv, ddouble dydv)) = surface.Diff(u, v);
                    ddouble value = f(x, y);

                    ddouble dsduv = ddouble.Abs(dxdu * dydv - dydu * dxdv);

                    ddouble g = value * dsduv;

                    sk += ps[i].wk * ps[j].wk * g;

                    if ((i & 1) == 1 && (j & 1) == 1) {
                        sg += ps[i].wg * ps[j].wg * g;
                    }
                }
            }

            ddouble uv = ru * rv;

            sk *= uv;
            sg *= uv;

            ddouble error = ddouble.Abs(sk - sg);

            return (sk, error);
        }
    }
}
