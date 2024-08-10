using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class Surface3DTest {
        [TestMethod]
        public void DiffTest() {
            Surface3D[] testcases = [
                Surface3D.Circle,
                Surface3D.Circle + (3, 4, 5),
                Surface3D.Circle - (3, 4, 5),
                Surface3D.Circle * 2,
                Surface3D.Circle * (3, 4, 5),
                Surface3D.Circle * (3, 4, 5) + (3, 4, 5),
                +(Surface3D.Circle + (3, 4, 5)),
                +(Surface3D.Circle * (3, 4, 5)),
                -(Surface3D.Circle + (3, 4, 5)),
                -(Surface3D.Circle * (3, 4, 5)),
                Surface3D.Rotate(Surface3D.Circle, (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Circle, (1, 2, 3), (3, 1, -4)),
                Surface3D.Rotate(Surface3D.Circle + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Circle * (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Circle * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Circle * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)),
                Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) + (3, 4, 5),
                Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) * 2,
                Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5),
                Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5) + (3, 4, 5),
                +(Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) + (3, 4, 5)),
                +(Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5)),
                -(Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) + (3, 4, 5)),
                -(Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5)),
                Surface3D.Rotate(Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Triangle((1, 0, 0), (2, 1, 3), (3, -1, -1)) * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)),
                Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) + (3, 4, 5),
                Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) * 2,
                Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5),
                Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5) + (3, 4, 5),
                +(Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) + (3, 4, 5)),
                +(Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5)),
                -(Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) + (3, 4, 5)),
                -(Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5)),
                Surface3D.Rotate(Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rhombus((1, 0, 0), (2, 1, 3), (3, -1, -1)) * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Surface3D.Sphere,
                Surface3D.Sphere + (3, 4, 5),
                Surface3D.Sphere - (3, 4, 5),
                Surface3D.Sphere * 2,
                Surface3D.Sphere * (3, 4, 5),
                Surface3D.Sphere * (3, 4, 5) + (3, 4, 5),
                +(Surface3D.Sphere + (3, 4, 5)),
                +(Surface3D.Sphere * (3, 4, 5)),
                -(Surface3D.Sphere + (3, 4, 5)),
                -(Surface3D.Sphere * (3, 4, 5)),
                Surface3D.Rotate(Surface3D.Sphere, (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Sphere, (1, 2, 3), (3, 1, -4)),
                Surface3D.Rotate(Surface3D.Sphere + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Sphere * (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Sphere * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Sphere * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Surface3D.Cylinder,
                Surface3D.Cylinder + (3, 4, 5),
                Surface3D.Cylinder - (3, 4, 5),
                Surface3D.Cylinder * 2,
                Surface3D.Cylinder * (3, 4, 5),
                Surface3D.Cylinder * (3, 4, 5) + (3, 4, 5),
                +(Surface3D.Cylinder + (3, 4, 5)),
                +(Surface3D.Cylinder * (3, 4, 5)),
                -(Surface3D.Cylinder + (3, 4, 5)),
                -(Surface3D.Cylinder * (3, 4, 5)),
                Surface3D.Rotate(Surface3D.Cylinder, (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Cylinder, (1, 2, 3), (3, 1, -4)),
                Surface3D.Rotate(Surface3D.Cylinder + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Cylinder * (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Rotate(Surface3D.Cylinder * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Surface3D.Cylinder * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },
            ];

            foreach (Surface3D testcase in testcases) {
                Surface3D testcase_resetds = new(testcase.Value, testcase.Diff);

                foreach (ddouble u in new ddouble[] { 0.25, 0.5, 0.75 }) {
                    foreach (ddouble v in new ddouble[] { 0.25, 0.5, 0.75 }) {
                        (ddouble x_umeps, ddouble y_umeps, ddouble z_umeps) = testcase.Value(u - 1e-15, v);
                        (ddouble x_upeps, ddouble y_upeps, ddouble z_upeps) = testcase.Value(u + 1e-15, v);
                        (ddouble x_vmeps, ddouble y_vmeps, ddouble z_vmeps) = testcase.Value(u, v - 1e-15);
                        (ddouble x_vpeps, ddouble y_vpeps, ddouble z_vpeps) = testcase.Value(u, v + 1e-15);

                        (ddouble dxdu_expected, ddouble dydu_expected, ddouble dzdu_expected)
                            = ((x_upeps - x_umeps) / 2e-15, (y_upeps - y_umeps) / 2e-15, (z_upeps - z_umeps) / 2e-15);
                        (ddouble dxdv_expected, ddouble dydv_expected, ddouble dzdv_expected)
                            = ((x_vpeps - x_vmeps) / 2e-15, (y_vpeps - y_vmeps) / 2e-15, (z_vpeps - z_vmeps) / 2e-15);
                        ((ddouble dxdu, ddouble dydu, ddouble dzdu), (ddouble dxdv, ddouble dydv, ddouble dzdv)) = testcase.Diff(u, v);

                        ddouble ds_expected = testcase_resetds.Ds(u, v);
                        ddouble ds = testcase.Ds(u, v);

                        Assert.IsTrue(ddouble.Abs(dxdu - dxdu_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(dydu - dydu_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(dzdu - dzdu_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(dxdv - dxdv_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(dydv - dydv_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(dzdv - dzdv_expected) < 1e-10);
                        Assert.IsTrue(ddouble.Abs(ds - ds_expected) < 1e-10);
                    }
                }
            }
        }
    }
}