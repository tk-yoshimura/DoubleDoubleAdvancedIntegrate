using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class Volume3DTest {
        [TestMethod]
        public void DiffTest() {
            Volume3D[] testcases = [
                Volume3D.Ortho,
                +Volume3D.Ortho,
                -Volume3D.Ortho,
                Volume3D.Ortho + (3, 4, 5),
                Volume3D.Ortho - (3, 4, 5),
                Volume3D.Ortho * 2,
                Volume3D.Ortho * (3, 4, 5),
                Volume3D.Ortho * (3, 4, 5) + (3, 4, 5),
                Volume3D.Rotate(Volume3D.Ortho, (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Ortho + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Ortho * (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Ortho * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Ortho * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Volume3D.InfinityOrtho,
                +Volume3D.InfinityOrtho,
                -Volume3D.InfinityOrtho,
                Volume3D.InfinityOrtho + (3, 4, 5),
                Volume3D.InfinityOrtho - (3, 4, 5),
                Volume3D.InfinityOrtho * 2,
                Volume3D.InfinityOrtho * (3, 4, 5),
                Volume3D.InfinityOrtho * (3, 4, 5) + (3, 4, 5),
                Volume3D.Rotate(Volume3D.InfinityOrtho, (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.InfinityOrtho + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.InfinityOrtho * (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.InfinityOrtho * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.InfinityOrtho * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Volume3D.Sphere,
                Volume3D.Sphere + (3, 4, 5),
                Volume3D.Sphere - (3, 4, 5),
                Volume3D.Sphere * 2,
                Volume3D.Sphere * (3, 4, 5),
                Volume3D.Sphere * (3, 4, 5) + (3, 4, 5),
                +(Volume3D.Sphere + (3, 4, 5)),
                +(Volume3D.Sphere * (3, 4, 5)),
                -(Volume3D.Sphere + (3, 4, 5)),
                -(Volume3D.Sphere * (3, 4, 5)),
                Volume3D.Rotate(Volume3D.Sphere, (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Sphere, (1, 2, 3), (3, 1, -4)),
                Volume3D.Rotate(Volume3D.Sphere + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Sphere * (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Sphere * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Sphere * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Volume3D.InfinitySphere,
                Volume3D.InfinitySphere + (3, 4, 5),
                Volume3D.InfinitySphere - (3, 4, 5),
                Volume3D.InfinitySphere * 2,
                Volume3D.InfinitySphere * (3, 4, 5),
                Volume3D.InfinitySphere * (3, 4, 5) + (3, 4, 5),
                +(Volume3D.InfinitySphere + (3, 4, 5)),
                +(Volume3D.InfinitySphere * (3, 4, 5)),
                -(Volume3D.InfinitySphere + (3, 4, 5)),
                -(Volume3D.InfinitySphere * (3, 4, 5)),
                Volume3D.Rotate(Volume3D.InfinitySphere, (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.InfinitySphere, (1, 2, 3), (3, 1, -4)),
                Volume3D.Rotate(Volume3D.InfinitySphere + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.InfinitySphere * (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.InfinitySphere * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.InfinitySphere * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)),
                Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) + (3, 4, 5),
                Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * 2,
                Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5),
                Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5) + (3, 4, 5),
                +(Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) + (3, 4, 5)),
                +(Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5)),
                -(Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) + (3, 4, 5)),
                -(Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5)),
                Volume3D.Rotate(Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Tetrahedron((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)),
                Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) + (3, 4, 5),
                Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * 2,
                Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5),
                Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5) + (3, 4, 5),
                +(Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) + (3, 4, 5)),
                +(Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5)),
                -(Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) + (3, 4, 5)),
                -(Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5)),
                Volume3D.Rotate(Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Parallelepiped((1, 0, 0), (2, 1, 3), (3, -1, -1), (0, 4, -5)) * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Volume3D.Cylinder,
                Volume3D.Cylinder + (3, 4, 5),
                Volume3D.Cylinder - (3, 4, 5),
                Volume3D.Cylinder * 2,
                Volume3D.Cylinder * (3, 4, 5),
                Volume3D.Cylinder * (3, 4, 5) + (3, 4, 5),
                +(Volume3D.Cylinder + (3, 4, 5)),
                +(Volume3D.Cylinder * (3, 4, 5)),
                -(Volume3D.Cylinder + (3, 4, 5)),
                -(Volume3D.Cylinder * (3, 4, 5)),
                Volume3D.Rotate(Volume3D.Cylinder, (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Cylinder, (1, 2, 3), (3, 1, -4)),
                Volume3D.Rotate(Volume3D.Cylinder + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Cylinder * (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.Cylinder * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Cylinder * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },

                Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)),
                Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) + (3, 4, 5),
                Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) * 2,
                Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) * (3, 4, 5),
                Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) * (3, 4, 5) + (3, 4, 5),
                +(Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) + (3, 4, 5)),
                +(Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) * (3, 4, 5)),
                -(Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) + (3, 4, 5)),
                -(Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) * (3, 4, 5)),
                Volume3D.Rotate(Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) * (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.Rotate(Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) * (3, 4, 5) + (3, 4, 5), (1, 2, 3), 0.25),
                Volume3D.TrigonalPrism((1, 0), (2, 1), (3, -1)) * new ddouble[,]{ { 1, 2, 3, -1 }, { 4, -2, -3, 5 }, { -5, 7, -4, -3 } },
            ];

            foreach (Volume3D testcase in testcases) {
                Volume3D testcase_resetds = new(testcase.Value, testcase.Diff);

                foreach (ddouble u in new ddouble[] { 0.25, 0.5, 0.75 }) {
                    foreach (ddouble v in new ddouble[] { 0.25, 0.5, 0.75 }) {
                        foreach (ddouble w in new ddouble[] { 0.25, 0.5, 0.75 }) {
                            (ddouble x_umeps, ddouble y_umeps, ddouble z_umeps) = testcase.Value(u - 1e-15, v, w);
                            (ddouble x_upeps, ddouble y_upeps, ddouble z_upeps) = testcase.Value(u + 1e-15, v, w);
                            (ddouble x_vmeps, ddouble y_vmeps, ddouble z_vmeps) = testcase.Value(u, v - 1e-15, w);
                            (ddouble x_vpeps, ddouble y_vpeps, ddouble z_vpeps) = testcase.Value(u, v + 1e-15, w);
                            (ddouble x_wmeps, ddouble y_wmeps, ddouble z_wmeps) = testcase.Value(u, v, w - 1e-15);
                            (ddouble x_wpeps, ddouble y_wpeps, ddouble z_wpeps) = testcase.Value(u, v, w + 1e-15);

                            (ddouble dxdu_expected, ddouble dydu_expected, ddouble dzdu_expected)
                                = ((x_upeps - x_umeps) / 2e-15, (y_upeps - y_umeps) / 2e-15, (z_upeps - z_umeps) / 2e-15);
                            (ddouble dxdv_expected, ddouble dydv_expected, ddouble dzdv_expected)
                                = ((x_vpeps - x_vmeps) / 2e-15, (y_vpeps - y_vmeps) / 2e-15, (z_vpeps - z_vmeps) / 2e-15);
                            (ddouble dxdw_expected, ddouble dydw_expected, ddouble dzdw_expected)
                                = ((x_wpeps - x_wmeps) / 2e-15, (y_wpeps - y_wmeps) / 2e-15, (z_wpeps - z_wmeps) / 2e-15);
                            ((ddouble dxdu, ddouble dydu, ddouble dzdu), (ddouble dxdv, ddouble dydv, ddouble dzdv), (ddouble dxdw, ddouble dydw, ddouble dzdw))
                                = testcase.Diff(u, v, w);

                            ddouble ds_expected = testcase_resetds.Ds(u, v, w);
                            ddouble ds = testcase.Ds(u, v, w);

                            Assert.IsTrue(ddouble.Abs(dxdu - dxdu_expected) < 1e-10);
                            Assert.IsTrue(ddouble.Abs(dydu - dydu_expected) < 1e-10);
                            Assert.IsTrue(ddouble.Abs(dzdu - dzdu_expected) < 1e-10);
                            Assert.IsTrue(ddouble.Abs(dxdv - dxdv_expected) < 1e-10);
                            Assert.IsTrue(ddouble.Abs(dydv - dydv_expected) < 1e-10);
                            Assert.IsTrue(ddouble.Abs(dzdv - dzdv_expected) < 1e-10);
                            Assert.IsTrue(ddouble.Abs(dxdw - dxdw_expected) < 1e-10);
                            Assert.IsTrue(ddouble.Abs(dydw - dydw_expected) < 1e-10);
                            Assert.IsTrue(ddouble.Abs(dzdw - dzdw_expected) < 1e-10);
                            Assert.IsTrue(ddouble.Abs(ds - ds_expected) < 1e-10);
                        }
                    }
                }
            }
        }
    }
}