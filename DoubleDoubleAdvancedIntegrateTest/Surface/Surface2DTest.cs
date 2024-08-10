using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class Surface2DTest {
        [TestMethod]
        public void DiffTest() {
            Surface2D[] testcases = [
                Surface2D.Ortho,
                +Surface2D.Ortho,
                -Surface2D.Ortho,
                Surface2D.Ortho + (3, 4),
                Surface2D.Ortho - (3, 4),
                Surface2D.Ortho * 2,
                Surface2D.Ortho * (3, 4),
                Surface2D.Ortho * (3, 4) + (3, 4),
                Surface2D.Rotate(Surface2D.Ortho, 0.25),
                Surface2D.Rotate(Surface2D.Ortho + (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.Ortho * (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.Ortho * (3, 4) + (3, 4), 0.25),
                Surface2D.Ortho * new ddouble[,]{ { 1, 2, 3 }, { 4, -2, -3 } },

                Surface2D.InfinityOrtho,
                +Surface2D.InfinityOrtho,
                -Surface2D.InfinityOrtho,
                Surface2D.InfinityOrtho + (3, 4),
                Surface2D.InfinityOrtho - (3, 4),
                Surface2D.InfinityOrtho * 2,
                Surface2D.InfinityOrtho * (3, 4),
                Surface2D.InfinityOrtho * (3, 4) + (3, 4),
                Surface2D.Rotate(Surface2D.InfinityOrtho, 0.25),
                Surface2D.Rotate(Surface2D.InfinityOrtho + (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.InfinityOrtho * (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.InfinityOrtho * (3, 4) + (3, 4), 0.25),
                Surface2D.InfinityOrtho * new ddouble[,]{ { 1, 2, 3 }, { 4, -2, -3 } },

                Surface2D.Circle,
                Surface2D.Circle + (3, 4),
                Surface2D.Circle - (3, 4),
                Surface2D.Circle * 2,
                Surface2D.Circle * (3, 4),
                Surface2D.Circle * (3, 4) + (3, 4),
                +(Surface2D.Circle + (3, 4)),
                +(Surface2D.Circle * (3, 4)),
                -(Surface2D.Circle + (3, 4)),
                -(Surface2D.Circle * (3, 4)),
                Surface2D.Rotate(Surface2D.Circle, 0.25),
                Surface2D.Rotate(Surface2D.Circle + (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.Circle * (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.Circle * (3, 4) + (3, 4), 0.25),
                Surface2D.Circle * new ddouble[,]{ { 1, 2, 3 }, { 4, -2, -3 } },

                Surface2D.InfinityCircle,
                Surface2D.InfinityCircle + (3, 4),
                Surface2D.InfinityCircle - (3, 4),
                Surface2D.InfinityCircle * 2,
                Surface2D.InfinityCircle * (3, 4),
                Surface2D.InfinityCircle * (3, 4) + (3, 4),
                +(Surface2D.InfinityCircle + (3, 4)),
                +(Surface2D.InfinityCircle * (3, 4)),
                -(Surface2D.InfinityCircle + (3, 4)),
                -(Surface2D.InfinityCircle * (3, 4)),
                Surface2D.Rotate(Surface2D.InfinityCircle, 0.25),
                Surface2D.Rotate(Surface2D.InfinityCircle + (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.InfinityCircle * (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.InfinityCircle * (3, 4) + (3, 4), 0.25),
                Surface2D.InfinityCircle * new ddouble[,]{ { 1, 2, 3 }, { 4, -2, -3 } },

                Surface2D.Triangle((1, 0), (2, 1), (3, -1)),
                Surface2D.Triangle((1, 0), (2, 1), (3, -1)) + (3, 4),
                Surface2D.Triangle((1, 0), (2, 1), (3, -1)) * 2,
                Surface2D.Triangle((1, 0), (2, 1), (3, -1)) * (3, 4),
                Surface2D.Triangle((1, 0), (2, 1), (3, -1)) * (3, 4) + (3, 4),
                +(Surface2D.Triangle((1, 0), (2, 1), (3, -1)) + (3, 4)),
                +(Surface2D.Triangle((1, 0), (2, 1), (3, -1)) * (3, 4)),
                -(Surface2D.Triangle((1, 0), (2, 1), (3, -1)) + (3, 4)),
                -(Surface2D.Triangle((1, 0), (2, 1), (3, -1)) * (3, 4)),
                Surface2D.Rotate(Surface2D.Triangle((1, 0), (2, 1), (3, -1)), 0.25),
                Surface2D.Rotate(Surface2D.Triangle((1, 0), (2, 1), (3, -1)) + (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.Triangle((1, 0), (2, 1), (3, -1)) * (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.Triangle((1, 0), (2, 1), (3, -1)) * (3, 4) + (3, 4), 0.25),
                Surface2D.Triangle((1, 0), (2, 1), (3, -1)) * new ddouble[,]{ { 1, 2, 3 }, { 4, -2, -3 } },

                Surface2D.Rhombus((1, 0), (2, 1), (3, -1)),
                Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) + (3, 4),
                Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) * 2,
                Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) * (3, 4),
                Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) * (3, 4) + (3, 4),
                +(Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) + (3, 4)),
                +(Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) * (3, 4)),
                -(Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) + (3, 4)),
                -(Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) * (3, 4)),
                Surface2D.Rotate(Surface2D.Rhombus((1, 0), (2, 1), (3, -1)), 0.25),
                Surface2D.Rotate(Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) + (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) * (3, 4), 0.25),
                Surface2D.Rotate(Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) * (3, 4) + (3, 4), 0.25),
                Surface2D.Rhombus((1, 0), (2, 1), (3, -1)) * new ddouble[,]{ { 1, 2, 3 }, { 4, -2, -3 } },
            ];

            foreach (Surface2D testcase in testcases) {
                Surface2D testcase_resetds = new(testcase.Value, testcase.Diff);

                foreach (ddouble u in new ddouble[] { 0.25, 0.5, 0.75 }) {
                    foreach (ddouble v in new ddouble[] { 0.25, 0.5, 0.75 }) {
                        (ddouble x_umeps, ddouble y_umeps) = testcase.Value(u - 1e-15, v);
                        (ddouble x_upeps, ddouble y_upeps) = testcase.Value(u + 1e-15, v);
                        (ddouble x_vmeps, ddouble y_vmeps) = testcase.Value(u, v - 1e-15);
                        (ddouble x_vpeps, ddouble y_vpeps) = testcase.Value(u, v + 1e-15);

                        (ddouble dxdu_expected, ddouble dydu_expected) = ((x_upeps - x_umeps) / 2e-15, (y_upeps - y_umeps) / 2e-15);
                        (ddouble dxdv_expected, ddouble dydv_expected) = ((x_vpeps - x_vmeps) / 2e-15, (y_vpeps - y_vmeps) / 2e-15);
                        ((ddouble dxdu, ddouble dydu), (ddouble dxdv, ddouble dydv)) = testcase.Diff(u, v);

                        ddouble ds_expected = testcase_resetds.Ds(u, v);
                        ddouble ds = testcase.Ds(u, v);

                        Assert.IsTrue(ddouble.Abs(dxdu - dxdu_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(dydu - dydu_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(dxdv - dxdv_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(dydv - dydv_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(ds - ds_expected) < 1e-10);
                    }
                }
            }
        }
    }
}