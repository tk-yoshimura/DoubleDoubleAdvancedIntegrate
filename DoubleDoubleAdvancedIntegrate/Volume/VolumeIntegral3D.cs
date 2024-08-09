using DoubleDouble;
using DoubleDoubleIntegrate;
using System.Collections.ObjectModel;
using System.Diagnostics;

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

        private static (ddouble value, ddouble error, long eval_points) UnlimitedIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble> f,
            Volume3D volume,
            (ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range,
            ddouble eps, GaussKronrodOrder order) {

            Stack<((ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range, ddouble eps)> stack = new();
            stack.Push((u_range, v_range, w_range, eps));

            long eval_points_sum = 0;
            ddouble value_sum = 0d, error_sum = 0d;

            while (stack.Count > 0) {
                (u_range, v_range, w_range, eps) = stack.Pop();

                (ddouble value, ddouble error) = Integrate(f, volume, u_range, v_range, w_range, order);

                long eval_points = 1 + 2 * (long)order;
                eval_points_sum += eval_points * eval_points * eval_points;

                if (!(error > eps)) {
                    value_sum += value;
                    error_sum += error;
                    continue;
                }

                ddouble uc = ddouble.Ldexp(u_range.min + u_range.max, -1);
                ddouble vc = ddouble.Ldexp(v_range.min + v_range.max, -1);
                ddouble wc = ddouble.Ldexp(w_range.min + w_range.max, -1);

                ddouble eps_div = ddouble.Ldexp(eps, -3);

                stack.Push(((u_range.min, uc), (v_range.min, vc), (w_range.min, wc), eps_div));
                stack.Push(((uc, u_range.max), (v_range.min, vc), (w_range.min, wc), eps_div));
                stack.Push(((u_range.min, uc), (vc, v_range.max), (w_range.min, wc), eps_div));
                stack.Push(((uc, u_range.max), (vc, v_range.max), (w_range.min, wc), eps_div));
                stack.Push(((u_range.min, uc), (v_range.min, vc), (wc, w_range.max), eps_div));
                stack.Push(((uc, u_range.max), (v_range.min, vc), (wc, w_range.max), eps_div));
                stack.Push(((u_range.min, uc), (vc, v_range.max), (wc, w_range.max), eps_div));
                stack.Push(((uc, u_range.max), (vc, v_range.max), (wc, w_range.max), eps_div));
            }

            return (value_sum, error_sum, eval_points_sum);
        }

        private static (ddouble value, ddouble error, long eval_points) LimitedDepthIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble> f,
            Volume3D volume,
            (ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range,
            ddouble eps, GaussKronrodOrder order, int maxdepth) {

            Debug.Assert(maxdepth >= 0);

            Stack<((ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range, ddouble eps, int depth)> stack = new();
            stack.Push((u_range, v_range, w_range, eps, maxdepth));

            long eval_points_sum = 0;
            ddouble value_sum = 0d, error_sum = 0d;

            while (stack.Count > 0) {
                (u_range, v_range, w_range, eps, int depth) = stack.Pop();

                (ddouble value, ddouble error) = Integrate(f, volume, u_range, v_range, w_range, order);

                long eval_points = 1 + 2 * (long)order;
                eval_points_sum += eval_points * eval_points * eval_points;

                if (!(error > eps) || depth <= 0) {
                    value_sum += value;
                    error_sum += error;
                    continue;
                }

                ddouble uc = ddouble.Ldexp(u_range.min + u_range.max, -1);
                ddouble vc = ddouble.Ldexp(v_range.min + v_range.max, -1);
                ddouble wc = ddouble.Ldexp(w_range.min + w_range.max, -1);

                ddouble eps_div = ddouble.Ldexp(eps, -3);
                depth -= 1;

                stack.Push(((u_range.min, uc), (v_range.min, vc), (w_range.min, wc), eps_div, depth));
                stack.Push(((uc, u_range.max), (v_range.min, vc), (w_range.min, wc), eps_div, depth));
                stack.Push(((u_range.min, uc), (vc, v_range.max), (w_range.min, wc), eps_div, depth));
                stack.Push(((uc, u_range.max), (vc, v_range.max), (w_range.min, wc), eps_div, depth));
                stack.Push(((u_range.min, uc), (v_range.min, vc), (wc, w_range.max), eps_div, depth));
                stack.Push(((uc, u_range.max), (v_range.min, vc), (wc, w_range.max), eps_div, depth));
                stack.Push(((u_range.min, uc), (vc, v_range.max), (wc, w_range.max), eps_div, depth));
                stack.Push(((uc, u_range.max), (vc, v_range.max), (wc, w_range.max), eps_div, depth));
            }

            return (value_sum, error_sum, eval_points_sum);
        }

        private static (ddouble value, ddouble error, long eval_points) LimitedEvalIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble> f,
            Volume3D volume,
            (ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range,
            ddouble eps, GaussKronrodOrder order, long discontinue_eval_points) {

            Debug.Assert(discontinue_eval_points >= 0);

            PriorityQueue<((ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range, ddouble eps), long> queue = new();
            queue.Enqueue((u_range, v_range, w_range, eps), 0);

            long eval_points_sum = 0;
            ddouble value_sum = 0d, error_sum = 0d;

            while (queue.Count > 0) {
                (u_range, v_range, w_range, eps) = queue.Dequeue();

                (ddouble value, ddouble error) = Integrate(f, volume, u_range, v_range, w_range, order);

                long eval_points = 1 + 2 * (long)order;
                eval_points_sum += eval_points * eval_points * eval_points;

                if (!(error > eps) || eval_points_sum > discontinue_eval_points) {
                    value_sum += value;
                    error_sum += error;
                    continue;
                }

                ddouble uc = ddouble.Ldexp(u_range.min + u_range.max, -1);
                ddouble vc = ddouble.Ldexp(v_range.min + v_range.max, -1);
                ddouble wc = ddouble.Ldexp(w_range.min + w_range.max, -1);

                ddouble eps_div = ddouble.Ldexp(eps, -3);
                long priority = double.ILogB((double)error);

                queue.Enqueue(((u_range.min, uc), (v_range.min, vc), (w_range.min, wc), eps_div), -priority);
                queue.Enqueue(((uc, u_range.max), (v_range.min, vc), (w_range.min, wc), eps_div), -priority);
                queue.Enqueue(((u_range.min, uc), (vc, v_range.max), (w_range.min, wc), eps_div), -priority);
                queue.Enqueue(((uc, u_range.max), (vc, v_range.max), (w_range.min, wc), eps_div), -priority);
                queue.Enqueue(((u_range.min, uc), (v_range.min, vc), (wc, w_range.max), eps_div), -priority);
                queue.Enqueue(((uc, u_range.max), (v_range.min, vc), (wc, w_range.max), eps_div), -priority);
                queue.Enqueue(((u_range.min, uc), (vc, v_range.max), (wc, w_range.max), eps_div), -priority);
                queue.Enqueue(((uc, u_range.max), (vc, v_range.max), (wc, w_range.max), eps_div), -priority);
            }

            return (value_sum, error_sum, eval_points_sum);
        }

        private static (ddouble value, ddouble error, long eval_points) LimitedDepthAndEvalIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble> f,
            Volume3D volume,
            (ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range,
            ddouble eps, GaussKronrodOrder order, int maxdepth, long discontinue_eval_points) {

            Debug.Assert(maxdepth >= 0);
            Debug.Assert(discontinue_eval_points >= 0);

            PriorityQueue<((ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range, ddouble eps, int depth), long> queue = new();
            queue.Enqueue((u_range, v_range, w_range, eps, maxdepth), 0);

            long eval_points_sum = 0;
            ddouble value_sum = 0d, error_sum = 0d;

            while (queue.Count > 0) {
                (u_range, v_range, w_range, eps, int depth) = queue.Dequeue();

                (ddouble value, ddouble error) = Integrate(f, volume, u_range, v_range, w_range, order);

                long eval_points = 1 + 2 * (long)order;
                eval_points_sum += eval_points * eval_points * eval_points;

                if (!(error > eps) || depth <= 0 || eval_points_sum > discontinue_eval_points) {
                    value_sum += value;
                    error_sum += error;
                    continue;
                }

                ddouble uc = ddouble.Ldexp(u_range.min + u_range.max, -1);
                ddouble vc = ddouble.Ldexp(v_range.min + v_range.max, -1);
                ddouble wc = ddouble.Ldexp(w_range.min + w_range.max, -1);

                ddouble eps_div = ddouble.Ldexp(eps, -3);
                long priority = double.ILogB((double)error);
                depth -= 1;

                queue.Enqueue(((u_range.min, uc), (v_range.min, vc), (w_range.min, wc), eps_div, depth), -priority);
                queue.Enqueue(((uc, u_range.max), (v_range.min, vc), (w_range.min, wc), eps_div, depth), -priority);
                queue.Enqueue(((u_range.min, uc), (vc, v_range.max), (w_range.min, wc), eps_div, depth), -priority);
                queue.Enqueue(((uc, u_range.max), (vc, v_range.max), (w_range.min, wc), eps_div, depth), -priority);
                queue.Enqueue(((u_range.min, uc), (v_range.min, vc), (wc, w_range.max), eps_div, depth), -priority);
                queue.Enqueue(((uc, u_range.max), (v_range.min, vc), (wc, w_range.max), eps_div, depth), -priority);
                queue.Enqueue(((u_range.min, uc), (vc, v_range.max), (wc, w_range.max), eps_div, depth), -priority);
                queue.Enqueue(((uc, u_range.max), (vc, v_range.max), (wc, w_range.max), eps_div, depth), -priority);
            }

            return (value_sum, error_sum, eval_points_sum);
        }

        public static (ddouble value, ddouble error, long eval_points) AdaptiveIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble> f,
            Volume3D volume,
            (ddouble min, ddouble max) u_range, (ddouble min, ddouble max) v_range, (ddouble min, ddouble max) w_range,
            ddouble eps,
            GaussKronrodOrder order = GaussKronrodOrder.G31K63, int maxdepth = -1, long discontinue_eval_points = -1) {

            if (maxdepth < -1) {
                throw new ArgumentOutOfRangeException(nameof(maxdepth), "Invalid param. maxdepth=-1: infinite, maxdepth>=0: finite");
            }
            if (discontinue_eval_points < -1) {
                throw new ArgumentOutOfRangeException(nameof(discontinue_eval_points), "Invalid param. discontinue_eval_points=-1: infinite, discontinue_eval_points>=0: finite");
            }
            if (!(eps >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(eps), "Invalid param. eps must be nonnegative value");
            }
            if (!ddouble.IsFinite(u_range.min) || !ddouble.IsFinite(u_range.max) || !ddouble.IsFinite(u_range.min - u_range.max)) {
                throw new ArgumentOutOfRangeException(nameof(u_range), "Invalid param. interval must be finite");
            }
            if (!ddouble.IsFinite(v_range.min) || !ddouble.IsFinite(v_range.max) || !ddouble.IsFinite(v_range.min - v_range.max)) {
                throw new ArgumentOutOfRangeException(nameof(v_range), "Invalid param. interval must be finite");
            }

            if (ddouble.IsZero(eps)) {
                (ddouble value, ddouble error) = Integrate(f, volume, u_range, v_range, w_range, order);
                eps = ddouble.Ldexp(ddouble.Abs(value), -98);
                eps = ddouble.Max(eps, 2.2e-308);

                if (error < eps) {
                    long eval_points = 1 + 2 * (long)order;
                    return (value, error, eval_points * eval_points * eval_points);
                }
            }

            if (maxdepth >= 0 && discontinue_eval_points >= 0) {
                return LimitedDepthAndEvalIntegrate(f, volume, u_range, v_range, w_range, eps, order, maxdepth, discontinue_eval_points);
            }
            if (maxdepth >= 0) {
                return LimitedDepthIntegrate(f, volume, u_range, v_range, w_range, eps, order, maxdepth);
            }
            if (discontinue_eval_points >= 0) {
                return LimitedEvalIntegrate(f, volume, u_range, v_range, w_range, eps, order, discontinue_eval_points);
            }

            return UnlimitedIntegrate(f, volume, u_range, v_range, w_range, eps, order);
        }
    }
}
