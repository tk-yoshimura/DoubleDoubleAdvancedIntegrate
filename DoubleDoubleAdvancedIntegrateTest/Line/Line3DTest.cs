using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class Line3DTest {
        [TestMethod]
        public void DiffTest() {
            Line3D[] testcases = [
                Line3D.Line((0, 0, 1), (0, 2, 3)),
                Line3D.Line((0, 0, 1), (2, 0, 4)),
                Line3D.Line((1, 1, 1), (0, 2, 3)),
                Line3D.Line((1, 1, 1), (2, 0, 4)),
                +Line3D.Line((1, 1, 1), (2, 0, 4)),
                -Line3D.Line((1, 1, 1), (2, 0, 4)),
                Line3D.Line((1, 1, 1), (2, 0, 3)) + (3, 4, 5),
                Line3D.Line((1, 1, 1), (2, 0, 3)) - (3, 4, 5),
                Line3D.Line((1, 1, 1), (2, 0, 4)) * 3,
                Line3D.Line((1, 1, 1), (2, 0, 4)) * (3, 4, 5),
                Line3D.Line((1, 1, 1), (2, 0, 3)) * (3, 4, 5) + (3, 4, 5),
                Line3D.Rotate(Line3D.Line((1, 2, 1), (2, 0, 4)), (1, 2, 3), 0.25),
                Line3D.Rotate(Line3D.Line((1, 2, 1), (2, 0, 4)), (1, 2, 3), (3, 1, 4)),
                Line3D.Rotate(Line3D.Line((2, 2, 1), (2, 0, 3)), (2, 2, 3), 0.25) + (3, 4, 5),
                Line3D.Rotate(Line3D.Line((1, 1, 1), (2, 0, 4)), (1, 2, 1), 0.25) * (3, 4, 5),
                Line3D.Rotate(Line3D.Line((1, 2, 2), (2, 0, 3)), (1, 1, 3), 0.25) * (3, 4, 5) + (3, 4, 5),
                Line3D.Line((1, 1, 0), (2, 0, 1)) * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Line3D.Circle,
                Line3D.Circle + (3, 4, 5),
                Line3D.Circle - (3, 4, 5),
                Line3D.Circle * 3,
                Line3D.Circle * (3, 4, 5),
                Line3D.Circle * (3, 4, 5) + (3, 4, 5),
                Line3D.Rotate(Line3D.Circle, (1, 2, 3), 0.25),
                Line3D.Rotate(Line3D.Circle, (1, 2, 3), (3, 1, 4)),
                Line3D.Rotate(Line3D.Circle + (3, 4, 5), (1, 2, 3), 0.25),
                Line3D.Rotate(Line3D.Circle * (3, 4, 5), (1, 2, 3), (3, 1, 4)),
                Line3D.Rotate(Line3D.Circle * (3, 4, 5), (2, 2, 3), 0.25) + (3, 4, 5),
                Line3D.Rotate(Line3D.Circle * (3, 4, 5), (1, 2, 1), 0.25) * (3, 4, 5),
                Line3D.Rotate(Line3D.Circle * (3, 4, 5), (1, 1, 3), 0.25) * (3, 4, 5) + (3, 4, 5),
                Line3D.Circle * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Line3D.Helix,
                Line3D.Helix + (3, 4, 5),
                Line3D.Helix - (3, 4, 5),
                Line3D.Helix * 3,
                Line3D.Helix * (3, 4, 5),
                Line3D.Helix * (3, 4, 5) + (3, 4, 5),
                Line3D.Rotate(Line3D.Helix, (1, 2, 3), 0.25),
                Line3D.Rotate(Line3D.Helix, (1, 2, 3), (3, 1, 4)),
                Line3D.Rotate(Line3D.Helix + (3, 4, 5), (1, 2, 3), 0.25),
                Line3D.Rotate(Line3D.Helix * (3, 4, 5), (1, 2, 3), (3, 1, 4)),
                Line3D.Rotate(Line3D.Helix * (3, 4, 5), (2, 2, 3), 0.25) + (3, 4, 5),
                Line3D.Rotate(Line3D.Helix * (3, 4, 5), (1, 2, 1), 0.25) * (3, 4, 5),
                Line3D.Rotate(Line3D.Helix * (3, 4, 5), (1, 1, 3), 0.25) * (3, 4, 5) + (3, 4, 5),
                Line3D.Helix * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },
            ];

            foreach (Line3D testcase in testcases) {
                Line3D testcase_resetds = new(testcase.Value, testcase.Diff);

                foreach (ddouble t in new ddouble[] { 0.25, 0.5, 0.75 }) {
                    (ddouble x_meps, ddouble y_meps, ddouble z_meps) = testcase.Value(t - 1e-15);
                    (ddouble x_peps, ddouble y_peps, ddouble z_peps) = testcase.Value(t + 1e-15);

                    (ddouble dxdt_expected, ddouble dydt_expected, ddouble dzdt_expected) 
                        = ((x_peps - x_meps) / 2e-15, (y_peps - y_meps) / 2e-15, (z_peps - z_meps) / 2e-15);
                    (ddouble dxdt, ddouble dydt, ddouble dzdt) = testcase.Diff(t);

                    ddouble ds_expected = testcase_resetds.Ds(t);
                    ddouble ds = testcase.Ds(t);

                    Assert.IsTrue(ddouble.Abs(dxdt - dxdt_expected) < 1e-10);
                    Assert.IsTrue(ddouble.Abs(dydt - dydt_expected) < 1e-10);
                    Assert.IsTrue(ddouble.Abs(dzdt - dzdt_expected) < 1e-10);
                    Assert.IsTrue(ddouble.Abs(ds - ds_expected) < 1e-10);
                }
            }
        }
    }
}