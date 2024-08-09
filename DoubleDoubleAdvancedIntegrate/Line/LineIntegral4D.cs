using DoubleDouble;
using DoubleDoubleIntegrate;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DoubleDoubleAdvancedIntegrate {
    public static partial class LineIntegral {
        public static (ddouble value, ddouble error) Integrate(
            Func<ddouble, ddouble, ddouble, ddouble, ddouble> f,
            Line4D line,
            (ddouble min, ddouble max) range, GaussKronrodOrder order = GaussKronrodOrder.G31K63) {

            ReadOnlyCollection<(ddouble x, ddouble wg, ddouble wk)> ps = GaussKronrodPoints.Table[order];

            ddouble sg = ddouble.Zero, sk = ddouble.Zero;
            ddouble r = range.max - range.min;

            if (!ddouble.IsFinite(r)) {
                throw new ArgumentException("Invalid integation interval.", nameof(range));
            }

            for (int i = 0; i < ps.Count; i++) {
                ddouble t = ps[i].x * r + range.min;

                (ddouble x, ddouble y, ddouble z, ddouble w) = line.Value(t);
                (ddouble dx, ddouble dy, ddouble dz, ddouble dw) = line.Diff(t);

                ddouble dsdt = ddouble.Hypot(ddouble.Hypot(dx, dy), ddouble.Hypot(dz, dw));

                ddouble v = f(x, y, z, w) * dsdt;

                sk += ps[i].wk * v;

                if ((i & 1) == 1) {
                    sg += ps[i].wg * v;
                }
            }

            sk *= r;
            sg *= r;

            ddouble error = ddouble.Abs(sk - sg);

            return (sk, error);
        }

        private static (ddouble value, ddouble error, long eval_points) UnlimitedIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble, ddouble> f,
            Line4D line,
            (ddouble min, ddouble max) range, ddouble eps, GaussKronrodOrder order) {

            Stack<((ddouble min, ddouble max) range, ddouble eps)> stack = new();
            stack.Push((range, eps));

            long eval_points_sum = 0;
            ddouble value_sum = 0d, error_sum = 0d;

            while (stack.Count > 0) {
                (range, eps) = stack.Pop();

                (ddouble value, ddouble error) = Integrate(f, line, range, order);

                long eval_points = 1 + 2 * (int)order;
                eval_points_sum += eval_points;

                if (!(error > eps)) {
                    value_sum += value;
                    error_sum += error;
                    continue;
                }

                ddouble c = ddouble.Ldexp(range.min + range.max, -1);
                ddouble eps_half = ddouble.Ldexp(eps, -1);
                stack.Push(((range.min, c), eps_half));
                stack.Push(((c, range.max), eps_half));
            }

            return (value_sum, error_sum, eval_points_sum);
        }

        private static (ddouble value, ddouble error, long eval_points) LimitedDepthIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble, ddouble> f,
            Line4D line,
            (ddouble min, ddouble max) range, ddouble eps, GaussKronrodOrder order, int maxdepth) {

            Debug.Assert(maxdepth >= 0);

            Stack<((ddouble min, ddouble max) range, ddouble eps, int depth)> stack = new();
            stack.Push((range, eps, maxdepth));

            long eval_points_sum = 0;
            ddouble value_sum = 0d, error_sum = 0d;

            while (stack.Count > 0) {
                (range, eps, int depth) = stack.Pop();

                (ddouble value, ddouble error) = Integrate(f, line, range, order);

                long eval_points = 1 + 2 * (int)order;
                eval_points_sum += eval_points;

                if (!(error > eps) || depth <= 0) {
                    value_sum += value;
                    error_sum += error;
                    continue;
                }

                ddouble c = ddouble.Ldexp(range.min + range.max, -1);
                ddouble eps_half = ddouble.Ldexp(eps, -1);
                depth -= 1;
                stack.Push(((range.min, c), eps_half, depth));
                stack.Push(((c, range.max), eps_half, depth));
            }

            return (value_sum, error_sum, eval_points_sum);
        }

        private static (ddouble value, ddouble error, long eval_points) LimitedEvalIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble, ddouble> f,
            Line4D line,
            (ddouble min, ddouble max) range, ddouble eps, GaussKronrodOrder order, long discontinue_eval_points) {

            Debug.Assert(discontinue_eval_points >= 0);

            PriorityQueue<((ddouble min, ddouble max) range, ddouble eps), long> queue = new();
            queue.Enqueue((range, eps), 0);

            long eval_points_sum = 0;
            ddouble value_sum = 0d, error_sum = 0d;

            while (queue.Count > 0) {
                (range, eps) = queue.Dequeue();

                (ddouble value, ddouble error) = Integrate(f, line, range, order);

                long eval_points = 1 + 2 * (int)order;
                eval_points_sum += eval_points;

                if (!(error > eps) || eval_points_sum > discontinue_eval_points) {
                    value_sum += value;
                    error_sum += error;
                    continue;
                }

                ddouble c = ddouble.Ldexp(range.min + range.max, -1);
                ddouble eps_half = ddouble.Ldexp(eps, -1);
                long priority = double.ILogB((double)error);
                queue.Enqueue(((range.min, c), eps_half), -priority);
                queue.Enqueue(((c, range.max), eps_half), -priority);
            }

            return (value_sum, error_sum, eval_points_sum);
        }

        private static (ddouble value, ddouble error, long eval_points) LimitedDepthAndEvalIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble, ddouble> f,
            Line4D line,
            (ddouble min, ddouble max) range, ddouble eps, GaussKronrodOrder order, int maxdepth, long discontinue_eval_points) {

            Debug.Assert(maxdepth >= 0);
            Debug.Assert(discontinue_eval_points >= 0);

            PriorityQueue<((ddouble min, ddouble max) range, ddouble eps, int depth), long> queue = new();
            queue.Enqueue((range, eps, maxdepth), 0);

            long eval_points_sum = 0;
            ddouble value_sum = 0d, error_sum = 0d;

            while (queue.Count > 0) {
                (range, eps, int depth) = queue.Dequeue();

                (ddouble value, ddouble error) = Integrate(f, line, range, order);

                long eval_points = 1 + 2 * (int)order;
                eval_points_sum += eval_points;

                if (!(error > eps) || depth <= 0 || eval_points_sum > discontinue_eval_points) {
                    value_sum += value;
                    error_sum += error;
                    continue;
                }

                ddouble c = ddouble.Ldexp(range.min + range.max, -1);
                ddouble eps_half = ddouble.Ldexp(eps, -1);
                long priority = double.ILogB((double)error);
                depth -= 1;
                queue.Enqueue(((range.min, c), eps_half, depth), -priority);
                queue.Enqueue(((c, range.max), eps_half, depth), -priority);
            }

            return (value_sum, error_sum, eval_points_sum);
        }

        public static (ddouble value, ddouble error, long eval_points) AdaptiveIntegrate(
            Func<ddouble, ddouble, ddouble, ddouble, ddouble> f,
            Line4D line,
            (ddouble min, ddouble max) range, ddouble eps,
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
            if (!ddouble.IsFinite(range.min) || !ddouble.IsFinite(range.max) || !ddouble.IsFinite(range.min - range.max)) {
                throw new ArgumentOutOfRangeException(nameof(range), "Invalid param. interval must be finite");
            }

            if (ddouble.IsZero(eps)) {
                (ddouble value, ddouble error) = Integrate(f, line, range, order);
                eps = ddouble.Ldexp(ddouble.Abs(value), -98);
                eps = ddouble.Max(eps, 2.2e-308);

                if (error < eps) {
                    long eval_points = 1 + 2 * (long)order;
                    return (value, error, eval_points);
                }
            }

            if (maxdepth >= 0 && discontinue_eval_points >= 0) {
                return LimitedDepthAndEvalIntegrate(f, line, range, eps, order, maxdepth, discontinue_eval_points);
            }
            if (maxdepth >= 0) {
                return LimitedDepthIntegrate(f, line, range, eps, order, maxdepth);
            }
            if (discontinue_eval_points >= 0) {
                return LimitedEvalIntegrate(f, line, range, eps, order, discontinue_eval_points);
            }

            return UnlimitedIntegrate(f, line, range, eps, order);
        }
    }
}
