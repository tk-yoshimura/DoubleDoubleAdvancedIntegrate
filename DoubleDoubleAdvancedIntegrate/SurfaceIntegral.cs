using DoubleDouble;
using DoubleDoubleIntegrate;
using System.Collections.ObjectModel;

namespace DoubleDoubleAdvancedIntegrate {
    public static class SurfaceIntegral {
        public class Surface(ddouble x, ddouble y, ddouble z) {
            public ddouble x = x, y = y, z = z;
        }

        public class Partials((ddouble dxdu, ddouble dydu, ddouble dzdu) du, (ddouble dxdv, ddouble dydv, ddouble dzdv) dv) {
            public ddouble dxdu = du.dxdu, dydu = du.dydu, dzdu = du.dzdu, dxdv = dv.dxdv, dydv = dv.dydv, dzdv = dv.dzdv;
        }

        public static (ddouble value, ddouble error) Integrate(
            Func<ddouble, ddouble, ddouble, ddouble> f,
            Func<ddouble, ddouble, Surface> s,
            Func<ddouble, ddouble, Partials> d,
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

                    Surface surface = s(u, v);
                    Partials partial = d(u, v);
                    ddouble value = f(surface.x, surface.y, surface.z);

                    ddouble dsduv = ddouble.Hypot(
                        partial.dydu * partial.dzdv - partial.dzdu * partial.dydv, 
                        partial.dzdu * partial.dxdv - partial.dxdu * partial.dzdv, 
                        partial.dxdu * partial.dydv - partial.dydu * partial.dxdv
                    );

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
