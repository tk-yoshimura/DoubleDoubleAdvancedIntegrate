using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class Line4DTest {
        [TestMethod]
        public void DiffTest() {
            Line4D[] testcases = [
                Line4D.Line((0, 0, 1, 2), (0, 2, 3, 1)),
                Line4D.Line((0, 0, 1, 2), (2, 0, 4, 2)),
                Line4D.Line((1, 1, 1, 2), (0, 2, 3, 3)),
                Line4D.Line((1, 1, 1, 2), (2, 0, 4, 1)),
                +Line4D.Line((1, 1, 1, 2), (2, 0, 4, 2)),
                -Line4D.Line((1, 1, 1, 2), (2, 0, 4, 3)),
                Line4D.Line((1, 1, 1, 2), (2, 0, 3, 1)) + (3, 4, 5, 6),
                Line4D.Line((1, 1, 1, 2), (2, 0, 3, 1)) - (3, 4, 5, 6),
                Line4D.Line((1, 1, 1, 2), (2, 0, 4, 2)) * 3,
                Line4D.Line((1, 1, 1, 2), (2, 0, 4, 3)) * (3.0, 4, 5, 6),
                Line4D.Line((1, 1, 1, 2), (2, 0, 3, 1)) * (3.0, 4, 5, 6) + (3, 4, 5, 6),
                Line4D.Line((1, 1, 0, 2), (2, 0, 1, 2)) * new ddouble[,]{ { 1, 2, 3, -1, -3 }, { 4, -2, -3, 5, 2 }, { -5, 7, -4, -3, -2 }, { -3, 3, -1, -3, -4 } },
            ];

            foreach (Line4D testcase in testcases) {
                Line4D testcase_resetds = new(testcase.Value, testcase.Diff);

                foreach (ddouble t in new ddouble[] { 0.25, 0.5, 0.75 }) {
                    (ddouble x_meps, ddouble y_meps, ddouble z_meps, ddouble w_meps) = testcase.Value(t - 1e-15);
                    (ddouble x_peps, ddouble y_peps, ddouble z_peps, ddouble w_peps) = testcase.Value(t + 1e-15);

                    (ddouble dxdt_expected, ddouble dydt_expected, ddouble dzdt_expected, ddouble dwdt_expected) 
                        = ((x_peps - x_meps) / 2e-15, (y_peps - y_meps) / 2e-15, (z_peps - z_meps) / 2e-15, (w_peps - w_meps) / 2e-15);
                    (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt) = testcase.Diff(t);

                    ddouble ds_expected = testcase_resetds.Ds(t);
                    ddouble ds = testcase.Ds(t);

                    Assert.IsTrue(ddouble.Abs(dxdt - dxdt_expected) < 1e-10);
                    Assert.IsTrue(ddouble.Abs(dydt - dydt_expected) < 1e-10);
                    Assert.IsTrue(ddouble.Abs(dzdt - dzdt_expected) < 1e-10);
                    Assert.IsTrue(ddouble.Abs(dwdt - dwdt_expected) < 1e-10);
                    Assert.IsTrue(ddouble.Abs(ds - ds_expected) < 1e-10);
                }
            }
        }
    }
}