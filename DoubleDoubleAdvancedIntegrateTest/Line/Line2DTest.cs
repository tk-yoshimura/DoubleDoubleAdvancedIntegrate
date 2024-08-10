using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class Line2DTest {
        [TestMethod]
        public void DiffTest() {
            Line2D[] testcases = [
                Line2D.Line((0, 0), (0, 2)),
                Line2D.Line((0, 0), (2, 0)),
                Line2D.Line((1, 1), (0, 2)),
                Line2D.Line((1, 1), (2, 0)),
                +Line2D.Line((1, 1), (2, 0)),
                -Line2D.Line((1, 1), (2, 0)),
                Line2D.Line((1, 1), (2, 0)) + (3, 4),
                Line2D.Line((1, 1), (2, 0)) - (3, 4),
                Line2D.Line((1, 1), (2, 0)) * 2,
                Line2D.Line((1, 1), (2, 0)) * (3, 4),
                Line2D.Line((1, 1), (2, 0)) * (3, 4) + (3, 4),
                Line2D.Rotate(Line2D.Line((1, 1), (2, 0)), 0.25),
                Line2D.Rotate(Line2D.Line((1, 1), (2, 0)) + (3, 4), 0.25),
                Line2D.Rotate(Line2D.Line((1, 1), (2, 0)) * (3, 4), 0.25),
                Line2D.Rotate(Line2D.Line((1, 1), (2, 0)) * (3, 4) + (3, 4), 0.25),
                Line2D.Line((1, 1), (2, 0)) * new ddouble[,]{ { 1, 2, 3 }, { 4, -2, -3 } },

                Line2D.Circle,
                Line2D.Circle + (3, 4),
                Line2D.Circle - (3, 4),
                Line2D.Circle * 2,
                Line2D.Circle * (3, 4),
                Line2D.Circle * (3, 4) + (3, 4),
                +(Line2D.Circle + (3, 4)),
                +(Line2D.Circle * (3, 4)),
                -(Line2D.Circle + (3, 4)),
                -(Line2D.Circle * (3, 4)),
                Line2D.Rotate(Line2D.Circle, 0.25),
                Line2D.Rotate(Line2D.Circle + (3, 4), 0.25),
                Line2D.Rotate(Line2D.Circle * (3, 4), 0.25),
                Line2D.Rotate(Line2D.Circle * (3, 4) + (3, 4), 0.25),
                Line2D.Circle * new ddouble[,]{ { 1, 2, 3 }, { 4, -2, -3 } },
            ];

            foreach (Line2D testcase in testcases) {
                Line2D testcase_resetds = new(testcase.Value, testcase.Diff);

                foreach (ddouble t in new ddouble[] { 0.25, 0.5, 0.75 }) {
                    (ddouble x_meps, ddouble y_meps) = testcase.Value(t - 1e-15);
                    (ddouble x_peps, ddouble y_peps) = testcase.Value(t + 1e-15);

                    (ddouble dxdt_expected, ddouble dydt_expected) = ((x_peps - x_meps) / 2e-15, (y_peps - y_meps) / 2e-15);
                    (ddouble dxdt, ddouble dydt) = testcase.Diff(t);

                    ddouble ds_expected = testcase_resetds.Ds(t);
                    ddouble ds = testcase.Ds(t);

                    Assert.IsTrue(ddouble.Abs(dxdt - dxdt_expected) < 1e-10);
                    Assert.IsTrue(ddouble.Abs(dydt - dydt_expected) < 1e-10);
                    Assert.IsTrue(ddouble.Abs(ds - ds_expected) < 1e-10);
                }
            }
        }
    }
}